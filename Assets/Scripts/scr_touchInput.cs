using UnityEngine;
using System.Collections;

/*
* How to use this
* Use GetComponent<scr_touchInput>() to get a reference
* and use getCurrentInputVector() and getCurrentblowingPower() to retrive the data
*/

public class scr_touchInput : InputMetod {

	//debug fun!
	public bool m_debug_mode = false;

	//these are in percent
	public float m_input_y_scale = 0.4f; 
	public float m_edge_threshold_scale = 0.2f;
	public float m_mid_threshold_scale = 0.2f;
	public enum PowerMetod {Step,Smooth};
	public PowerMetod m_powerMetod = PowerMetod.Step; 
	public int m_powerSteps = 2; 
	//public int m_beginsAtStep = 0;
	
	//can add min sizes of areas on request /robin

	private Vector2 m_current_input = new Vector2(0,0);
	private Rect m_leftArea =  new Rect();
	private Rect m_rightArea = new Rect();
	private float m_vertical_area; 

	private float m_y_offset;
	private int m_edgeThreshold; //distance from the edge until it starts to blow
	private int m_midThreshold; //distance from mid it reaches max power
	private float m_blowing_power = 0f;
	
	void Start () {
		//feel free to touch -- Note for programmers :  add min and max size in pixels for the areas
		int parts_covered = 2;  // ex 4; left side will cover from the left edge to one 4th of the screen, rigth side will mirror this
		m_leftArea = new Rect (0,0, Screen.width/parts_covered, Screen.height);
		m_rightArea = new Rect ((Screen.width/parts_covered)*(parts_covered-1), 0, Screen.width/parts_covered, Screen.height);
		m_vertical_area = Screen.height*(m_input_y_scale/2); //the "effective" area for turning controls
		m_edgeThreshold = (int)(Screen.width*m_edge_threshold_scale); 
		m_midThreshold = (int)(Screen.width*m_mid_threshold_scale); 

		//do not touch
		m_y_offset = (m_leftArea.height/2)-(m_vertical_area);

	}
	
	// Update is called once per frame
	void Update () {
		
		Touch[] touches = Input.touches;
		m_current_input = Vector2.zero;
		
		//check all touches, if they are in the control areas.
		float higestPower = 0; //for finding the highest "power" off the current inputs  (this is most likly temp when desigerns change their mind)
		foreach (Touch t in touches) {
			Vector2 pos = t.position;
			calcMovementMagnitudes(pos);
			float input_magnitude = calcBlowingMagnitude(pos);
			if(input_magnitude > higestPower){
				higestPower = input_magnitude;
			}
		}

		//pc stuff for debug purpose
		if (Input.GetMouseButton (0) && Application.isEditor) {
			Vector2 pos = Input.mousePosition;
			calcMovementMagnitudes(pos);
			float input_magnitude = calcBlowingMagnitude(pos);
			    if(input_magnitude > higestPower){
				higestPower = input_magnitude;
			}

		}
		//end of pc debug stuff

		m_blowing_power = higestPower;
		
	}

	//this and calcBlowing. migth need to take a Touch as argument to allow more info
	void calcMovementMagnitudes(Vector2 pos){
		//calcluate the movement stuff
		if(m_leftArea.Contains(pos)){ //check which half of the screen the input is
			m_current_input = new Vector2(calculateMagnitude(pos.y),m_current_input.y);
		}else if(m_rightArea.Contains(pos)){
			m_current_input = new Vector2(m_current_input.x,calculateMagnitude(pos.y));
		}
	}

	float calculateMagnitude(float y){
		return Mathf.Clamp((y - (m_vertical_area + m_y_offset)) / m_vertical_area,-1,1);
	}


	float calcBlowingMagnitude(Vector2 pos){
		//calculate blowing pooooooowwwwwwwwwwweeeer!
		int screen_center = Screen.width/2;
		float input_magnitude = Mathf.Abs (pos.x - screen_center);
		//input_magnitude += m_edgeThreshold;
		input_magnitude = screen_center - input_magnitude;//inverts it!
		input_magnitude -= m_edgeThreshold;
		input_magnitude = input_magnitude/(screen_center-(m_edgeThreshold+m_midThreshold)); // current/max = percent!

		if(m_powerMetod == PowerMetod.Step){
			//this line "steps" the function ex: 0.25 -> 0.5 -> 0.75
			input_magnitude = ((int)(input_magnitude*m_powerSteps))*(1f/m_powerSteps);
		}
		input_magnitude = Mathf.Clamp01(input_magnitude); //clamp between 0-1

		return input_magnitude;
	}

	
	void OnGUI(){
		if (m_debug_mode) {
			GUI.Label(new Rect(Screen.width/2-50,0,100,50),"("+m_current_input.x.ToString("F2") + ","+m_current_input.y.ToString("F2") +")");
			GUI.Label(new Rect(Screen.width/2-100,50,200,50),"Blowing Power! " + m_blowing_power.ToString("F2"));
			//GUI.Label(new Rect(Screen.width/2-100,100,200,50),"Touches " + Input.touches.Length + " / " + Input.touchCount);
			GUI.Box(new Rect(0,m_y_offset,m_edgeThreshold,m_vertical_area*2),"");
		}
	}

	public override Vector2 getCurrentInputVector(){
		return m_current_input;
	}

	public override float getCurrentBlowingPower(){
		return m_blowing_power;
	}

	//warning this does not consider eventual min/max pixelsizes on objects dont forget to update this aswell
	public Vector2 getGUIStickSize(){
		return new Vector2(m_edge_threshold_scale,m_input_y_scale);
	}
}
