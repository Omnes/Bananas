using UnityEngine;
using System.Collections;

public class playerAnimation : MonoBehaviour {

	public upperBodyAnimation m_upperBodyScript;
	private bool m_isRunningAnim = false;

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

		//Debug.Log(rigidbody.velocity.magnitude);
		//inte bra, men bestämmer runanimation for client
		if(Network.isClient){
			if(rigidbody.velocity.magnitude > 0.01){
				if(m_isRunningAnim == false){
					m_isRunningAnim = true;
					runningAnim();
				}
			}else if(m_isRunningAnim == true){
				m_isRunningAnim = false;
				idleAnim();
			}
		}

	}

	//upperbody
	//running
	public void runningAnim(){
		//Debug.Log("RUNNING");
		//m_upperBodyScript.changeAnimation(upperBodyAnimation.state.RUNNING);
		m_upperBodyScript.runAnimation();
	}
	//idle
	public void idleAnim(){
		//Debug.Log("IDLE");
		//m_upperBodyScript.changeAnimation(upperBodyAnimation.state.IDLE);
		m_upperBodyScript.idleAnimation();
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
