using UnityEngine;
using System.Collections;

public class scr_touchInput : MonoBehaviour {
	
	public Vector2 m_current_input = new Vector2(0,0);
	//debug fun!
	public bool m_debug_mode = false;
	public float m_input_y_scale = 1f;

	//can add min sizes of areas on request /robin

	private Rect m_leftArea =  new Rect();
	private Rect m_rightArea = new Rect();
	private float m_vertical_area; 
	private float m_screenHeigth;
	private float m_y_offset;
	private int m_edgeThreshold; //for blowing power, huehue
	private float m_blowing_power = 0f;
	
	void Start () {
		//feel free to touch -- Note for programmers :  add min and max size in pixels for the areas
		int parts_covered = 2;  // ex 4; left side will cover from the left edge to one 4th of the screen, rigth side will mirror this
		m_leftArea = new Rect (0,0, Screen.width/parts_covered, Screen.height);
		m_rightArea = new Rect ((Screen.width/parts_covered)*(parts_covered-1), 0, Screen.width/parts_covered, Screen.height);
		m_vertical_area = (Screen.height / 4)*m_input_y_scale; //the "effective" area for turning controls
		m_edgeThreshold = Screen.width/8; //this is the "dead" area on the sides for the blowing power,

		//do not touch
		m_y_offset = (m_leftArea.height/2)-(m_vertical_area);
		m_screenHeigth = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
		Touch[] touches = Input.touches;
		m_current_input = Vector2.zero;
		
		//check all touches, if they are in the control areas.
		float higestPower = 0; //for finding the highest "power" off the current inputs  (this is most likly temp when desigerns change their mind)
		foreach (Touch t in touches) {
			Vector2 pos = t.position;
			//calcluate the movement stuff
			if(m_leftArea.Contains(pos)){ //check which half of the screen the input is
				m_current_input.x = calculateMagnitude(pos.y);
			}else if(m_rightArea.Contains(pos)){
				m_current_input.y = calculateMagnitude(pos.y);
			}
			//calculate blowing pooooooowwwwwwwwwwweeeer!
			int screen_center = Screen.width/2;
			float input_magnitude = Mathf.Abs (pos.x - screen_center);
			//input_magnitude -= m_edgeThreshold;
			input_magnitude = screen_center - input_magnitude;//inverts it!
			//input_magnitude -= m_edgeThreshold;
			input_magnitude = input_magnitude/(screen_center-m_edgeThreshold); // current/max = percent!
			input_magnitude = Mathf.Clamp01(input_magnitude); //clamp between 0-1
			//input_magnitude = 1-input_magnitude; // aaand we invert it!
			if(input_magnitude > higestPower){
				higestPower = input_magnitude;
			}
		}
		
		//pc stuff for debug purpose
		if (m_debug_mode && Input.GetMouseButton (0)) {
			Vector2 pos = Input.mousePosition;
			if(m_leftArea.Contains(pos)){ //check which half of the screen the input is
				m_current_input.x = calculateMagnitude(pos.y);
			}else if(m_rightArea.Contains(pos)){
				m_current_input.y = calculateMagnitude(pos.y);
			}
			//calculate blowing pooooooowwwwwwwwwwweeeer!
			int screen_center = Screen.width/2;
			float input_magnitude = Mathf.Abs (pos.x - screen_center) - m_edgeThreshold;
			input_magnitude = input_magnitude/(screen_center-m_edgeThreshold); // current/max = percent!
			input_magnitude = Mathf.Clamp01(input_magnitude); //clamp between 0-1
			input_magnitude = 1-input_magnitude; // aaand we invert it!
			if(input_magnitude > higestPower){
				higestPower = input_magnitude;
			}
		}
		//end of pc debug stuff

		m_blowing_power = higestPower;
		
	}
	
	void OnGUI(){
		if (m_debug_mode) {
			GUI.Box(m_leftArea,"Left");
			GUI.Box(m_rightArea,"Rigth");
			GUI.Label(new Rect(Screen.width/2-50,0,100,25),"("+m_current_input.x.ToString("F2") + ","+m_current_input.y.ToString("F2") +")");
			GUI.Label(new Rect(Screen.width/2-100,25,200,25),"Blowing Power! " + m_blowing_power.ToString("F2"));
			
			GUI.Box(new Rect(0,m_y_offset,10,m_vertical_area*2),"");
		}
	}

	
	float calculateMagnitude(float y){
		return Mathf.Clamp((y - (m_vertical_area + m_y_offset)) / m_vertical_area,-1,1);
	}
	
	
	public Vector2 getCurrentInputVector(){
		return m_current_input;
	}

	public float getCurrentBlowingPower(){
		return m_blowing_power;
	}
}
