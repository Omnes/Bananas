﻿using UnityEngine;
using System.Collections;

public class playerAnimation : MonoBehaviour {

	public upperBodyAnimation m_upperBodyScript;

	// Use this for initialization
	void Start () {
		m_upperBodyScript = GetComponent<upperBodyAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.T)){
			tackleAnim(1.0f);
		}
		if(Input.GetKey(KeyCode.Y)){
			tackleLoseAnim(2.0f);
		}

		if(Input.GetKey(KeyCode.B)){
			blowAnim();
		}

	}

	//upperbody
	//running
	public void runningAnim(){
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.RUNNING);
	}
	//idle
	public void idleAnim(){
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.IDLE);
}
	//tackle
	public void tackleAnim(float dizzyTime){
		m_upperBodyScript.tackleAnimation(dizzyTime);
	}

	public void tackleLoseAnim(float dizzyTime){
		m_upperBodyScript.tackleLoseAnimation(dizzyTime);
	}

	public void winAnim(){
		m_upperBodyScript.winAnimation();
	}

	public void stunAnim(){
		m_upperBodyScript.stunAnimation ();
	}

	public void stopStunAnim(){
		m_upperBodyScript.stopStunAnimation ();
	}

	//blow
	public void blowAnim(){
		m_upperBodyScript.blowAnimation();

		//set upperbody blow
		//change anim blow
		//try to set lowerbody blow animation
	}
	//stopblow
	public void stopBlowAnim(){
		m_upperBodyScript.stopBlowAnimation();
	}
}
