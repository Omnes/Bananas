using UnityEngine;
using System.Collections;

public class RemoteInput : InputMetod {

	private Vector2 m_currentInput;
	private float m_blowingPower;

	// Use this for initialization
	void Start () {
		//setCurrentInputVector(new Vector2(1f,0f));
	}

	public override void setCurrentInputVector(Vector2 v){
		m_currentInput = v;
	}
	
	public override void setCurrentBlowingPower(float f){
		m_blowingPower = f;
	}
	
	public override Vector2 getCurrentInputVector(){
		return m_currentInput;
	}
	
	public override float getCurrentBlowingPower(){
		//temp since we are always going to blow
		return 1f;
//		return m_blowingPower;
	}
}
