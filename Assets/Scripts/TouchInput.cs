using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* How to use this
* Use GetComponent<TouchInput>() to get a reference
* and use getCurrentInputVector() and getCurrentblowingPower() to retrive the data
*/

public class TouchInput : InputMetod {

	//debug fun!
	public bool m_debug_mode = false;
	
	public float m_verticalAreaInInch = 2.5f; 
	public float m_edgeThresholdInInch = 1f;

	private Vector2 m_currentInput = new Vector2(0,0);
	private Rect m_leftArea =  new Rect();
	private Rect m_rightArea = new Rect();
	private float m_vertical_area; 

	private float m_y_offset;
	private float m_edgeThreshold; //distance from the edge until it starts to blow
	private float m_blowingPower = 0f;

	//private Queue<Vector2> m_delayedInput = new Queue<Vector2>();
	//public int m_delayedFrames = 10;
	private Vector2 m_delayed;
	
	void Start () {
		//feel free to touch 
		int parts_covered = 2;  // ex 4; left side will cover from the left edge to one 4th of the screen, rigth side will mirror this
		m_leftArea = new Rect (0,0, Screen.width/parts_covered, Screen.height);
		m_rightArea = new Rect ((Screen.width/parts_covered)*(parts_covered-1), 0, Screen.width/parts_covered, Screen.height);

		//do not touch
		m_vertical_area = GUIMath.InchToPixels(m_verticalAreaInInch); //the "effective" area for turning controls
		m_edgeThreshold = GUIMath.InchToPixels(m_edgeThresholdInInch); 
		m_y_offset = (m_leftArea.height/2)-(m_vertical_area);

	}
	
	// Update is called once per frame
	void Update () {
		
		Touch[] touches = Input.touches;
		m_currentInput = Vector2.zero;
		//m_delayed = m_delayedInput.Dequeue();
		
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
		if (Input.GetMouseButton (0) && (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)) {
			Vector2 pos = Input.mousePosition;
			calcMovementMagnitudes(pos);
			float input_magnitude = calcBlowingMagnitude(pos);
			    if(input_magnitude > higestPower){
				higestPower = input_magnitude;
			}

		}
		//end of pc debug stuff

		m_blowingPower = higestPower;
		
	}

	//this and calcBlowing. migth need to take a Touch as argument to allow more info
	void calcMovementMagnitudes(Vector2 pos){
		//calcluate the movement stuff
		if(m_leftArea.Contains(pos)){ //check which half of the screen the input is
			m_currentInput = new Vector2(calculateMagnitude(pos.y),m_currentInput.y);
		}else if(m_rightArea.Contains(pos)){
			m_currentInput = new Vector2(m_currentInput.x,calculateMagnitude(pos.y));
		}
	}

	float calculateMagnitude(float y){
		return Mathf.Clamp((y - (m_vertical_area + m_y_offset)) / m_vertical_area,-1,1);
	}
	

	float calcBlowingMagnitude(Vector2 pos){
		if(pos.x > m_edgeThreshold && pos.x < (Screen.width - m_edgeThreshold)){
			return 1;
		}
		return 0;
	}

	
	void OnGUI(){
		if (m_debug_mode) {
			GUI.Label(new Rect(Screen.width/2-50,0,100,50),"("+m_currentInput.x.ToString("F2") + ","+m_currentInput.y.ToString("F2") +")");
			GUI.Label(new Rect(Screen.width/2-100,50,200,50),"Blowing Power! " + m_blowingPower.ToString("F2"));
			//GUI.Label(new Rect(Screen.width/2-100,100,200,50),"Touches " + Input.touches.Length + " / " + Input.touchCount);
			GUI.Box(new Rect(0,m_y_offset,m_edgeThreshold,m_vertical_area*2),"");
		}
	}
	//used by the GUI
	public Vector2 getGUIStickSize(){
		return new Vector2(m_edgeThresholdInInch,m_verticalAreaInInch);
	}

	//inherit from InputMetod
	public override Vector2 getCurrentInputVector(){
		return m_currentInput;
		//return m_delayed;
	}

	public override float getCurrentBlowingPower(){
		return m_blowingPower;
	}
	public override void setCurrentInputVector(Vector2 v){
		m_currentInput = v;
	}
	
	public override void setCurrentBlowingPower(float f){
		m_blowingPower = f;
	}
	

}
