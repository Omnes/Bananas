using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : InputMetod {

	//debug fun!
	public bool m_debug_mode = false;
	
	public float m_verticalAreaInInch = 2.5f;
	public float m_verticalAreaMaxPercent = 0.7f; 
	public float m_edgeThresholdInInch = 1f;
	public float m_edgeThresholdMaxPercent = 0.15f;

	private Vector2 m_currentInput = new Vector2(0,0);
	private Rect m_leftArea =  new Rect();
	private Rect m_rightArea = new Rect();
	private float m_verticalArea; 
	private float m_edgeThreshold; //distance from the edge until it starts to blow

	private float m_yOffset;

	private bool m_blowing = false;
	private bool m_canToggle = true;

	//private Queue<Vector2> m_delayedInput = new Queue<Vector2>();
	//public int m_delayedFrames = 10;
	private Vector2 m_delayed;

	private bool isPC = false;
	
	void Start () {
		//this is the areas that take input
		int parts_covered = 2; 
		m_leftArea = new Rect (0,0, Screen.width/parts_covered, Screen.height);
		m_rightArea = new Rect ((Screen.width/parts_covered)*(parts_covered-1), 0, Screen.width/parts_covered, Screen.height);

		// sets the areas to be atleast a % of the screen and at most a inch value
		m_verticalArea = Mathf.Min( GUIMath.InchToPixels(m_verticalAreaInInch), Screen.height * m_verticalAreaMaxPercent); 
		m_edgeThreshold = Mathf.Min( GUIMath.InchToPixels(m_edgeThresholdInInch), Screen.width * m_edgeThresholdMaxPercent);

		m_yOffset = (m_leftArea.height/2)-(m_verticalArea);

		isPC = (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer);

	}
	
	// Update is called once per frame
	void Update () {
		
		Touch[] touches = Input.touches;
		m_currentInput = Vector2.zero;
		//m_delayed = m_delayedInput.Dequeue();
		

		int toggleBlowing = 0;
		//check all touches, if they are in the control areas.
		foreach (Touch t in touches) {
			Vector2 pos = t.position;
			calcMovementMagnitudes(pos);
			if(calcBlowing(pos)){
				toggleBlowing++;
			}
		}

		//pc stuff for debug purpose
		if (Input.GetMouseButton (0) && isPC) {
			Vector2 pos = Input.mousePosition;
			calcMovementMagnitudes(pos);
			if(calcBlowing(pos)){
				toggleBlowing++;
			}

		}

		int requiredFingersToToggle = isPC ? 1 : 2; // 1 if pc 2 if phone
		if(toggleBlowing >= requiredFingersToToggle){
			if(m_canToggle == true){
				m_blowing = !m_blowing;
				m_canToggle = false;
			}
		}else{
			m_canToggle = true;
		}
		
	}


	void OnGUI(){
		if(m_debug_mode){
			GUI.Label(new Rect(Screen.width / 2, 0,200,100),"(" + m_currentInput.x + "," + m_currentInput.y + ")");
		}
	}

	//This function should not ahve to be altered, it decides what side of the screen the input is on
	void calcMovementMagnitudes(Vector2 pos){
		//calcluate the movement stuff
		if(m_leftArea.Contains(pos)){ //check which half of the screen the input is
			m_currentInput = new Vector2(calculateMagnitudeNoReverse(pos.y),m_currentInput.y);
		}else if(m_rightArea.Contains(pos)){
			m_currentInput = new Vector2(m_currentInput.x,calculateMagnitudeNoReverse(pos.y));
		}
	}

	//modify this if you want other steering
	float calculateMagnitude(float y){
		return Mathf.Clamp((y - (m_verticalArea + m_yOffset)) / m_verticalArea,-1,1);
	}

	float calculateMagnitudeNoReverse(float y){
		float center = Screen.height / 2;
		float offset = center  - m_verticalArea / 2;
		return Mathf.Clamp( (y - offset) / m_verticalArea ,0,1);
	}
	

	bool calcBlowing(Vector2 pos){
		if(pos.x > m_edgeThreshold && pos.x < (Screen.width - m_edgeThreshold)){
			return true;
		}
		return false;
	}
	
	//used by the GUI
	public Vector2 getGUIStickSize(){
		return GUIMath.PixelsToInch(new Vector2( m_edgeThreshold,m_verticalArea));
	}

	//inherit from InputMetod
	public override Vector2 getCurrentInputVector(){
		return m_currentInput;
		//return m_delayed;
	}

	public override float getCurrentBlowingPower(){
		return m_blowing ? 1f : 0f;
	}
	public override void setCurrentInputVector(Vector2 v){
		m_currentInput = v;
	}
	
	public override void setCurrentBlowingPower(float f){
		m_blowing = f > 0.5f ? true : false;
	}
	

}
