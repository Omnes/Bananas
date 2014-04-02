﻿using UnityEngine;
using System.Collections;

public class scr_touchInput : MonoBehaviour {

	// Use this for initialization

	public Vector2 m_current_input = new Vector2(0,0);
	public bool m_debug_draw = false;
	public float m_input_y_scale = 1f;

	private Rect m_leftInputArea =  new Rect();
	private Rect m_rightInputArea = new Rect();
	private float m_vertical_area = 0; 
	private float m_screenHeigth = 0;

	void Start () {
		m_screenHeigth = Screen.height;
		m_leftInputArea = new Rect (0,0, Screen.width/8, Screen.height);
		m_rightInputArea = new Rect ((Screen.width/8)*7, 0, Screen.width/8, Screen.height);
		m_vertical_area = Screen.height / 4;
	}
	
	// Update is called once per frame
	void Update () {

		Touch[] touches = Input.touches;
		m_current_input = Vector2.zero;
		//check all touches, if they are in the controal areas.
		foreach (Touch t in touches) {
			Vector2 pos = t.position;
			if(m_leftInputArea.Contains(pos)){
				m_current_input.x = calculateMagnitude(pos.y);
			}else if(m_rightInputArea.Contains(pos)){
				m_current_input.y = calculateMagnitude(pos.y);
			}
		}

		//pc stuff for debug purpose
		if (Input.GetMouseButton (0)) {
			Vector2 pos = Input.mousePosition;
			if (m_leftInputArea.Contains (pos)) {
				m_current_input.x = calculateMagnitude (pos.y);
			} else if (m_rightInputArea.Contains (pos)) {
				m_current_input.y = calculateMagnitude (pos.y);
			}
		}

	}

	void OnGUI(){
		if (m_debug_draw) {
			GUI.Box(m_leftInputArea,"Left input area");
			GUI.Box(m_rightInputArea,"Rigth input area");
			GUI.Label(new Rect(0,0,200,50),"("+m_current_input.x + ","+m_current_input.y +")");
		}
	}


	float calculateMagnitude(float y){
		float halfHeigth = m_leftInputArea.height/2;
		float y_offset = (m_leftInputArea.height/2)-(m_vertical_area/2);
		return Mathf.Clamp((y - (m_vertical_area )+ y_offset) / m_vertical_area,-1,1);
	}


	public Vector2 getCurrentInputVector(){
		return m_current_input;
	}
}
