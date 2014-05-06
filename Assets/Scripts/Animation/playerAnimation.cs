using UnityEngine;
using System.Collections;

public class playerAnimation : MonoBehaviour {

	public upperBodyAnimation m_upperBodyScript;
	//public lowerBodyAnimation m_lowerBodyScript;

	// Use this for initialization
	void Start () {
		m_upperBodyScript = GetComponent<upperBodyAnimation>();
//		m_lowerBodyScript = GetComponent<lowerBodyAnimation>();
	}
	
	// Update is called once per frame
	void Update () {

//		if(Input.GetKey(KeyCode.T)){
//			tackleAnim();
//		}
//		if(Input.GetKey(KeyCode.Y)){
//			stopTackleAnim();
//		}
//		if(Input.GetKey(KeyCode.B)){
//			blowAnim();
//		}

	}

	//upperbody
	//running
	public void runningAnim(){
		//Debug.Log("RUNNING");
		//set upperbody running
		//changeAnimation(running blablabl);
		//set lowerbody animation to running
		//changeAnimation(m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.Running));
		//m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.RUNNING);
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.RUNNING);
	}
	//idle
	public void idleAnim(){
		//Debug.Log("IDLE");
		//set upperbody idle
		//m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.IDLE);
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.IDLE);
		//set lowerbody idle
		//changeAnimation(m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.Idle));
	}
	//tackle
	public void tackleAnim(){
		//Debug.Log("TACKLE");
		m_upperBodyScript.tackleAnimation();
	}

	//tackle
	public void stopTackleAnim(){
		//Debug.Log("TACKLE");
		m_upperBodyScript.stopTackleAnimation();
	}

	//blow
	public void blowAnim(){
		//Debug.Log("BLOW");
		//m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.BLOW);
		m_upperBodyScript.blowAnimation();

		//set upperbody blow
		//change anim blow
		//try to set lowerbody blow animation
	}
	//stopblow
	public void stopBlowAnim(){
		//Debug.Log("STOPBLOWING");
		//m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.STOPBLOW);
		m_upperBodyScript.stopBlowAnimation();
		//set upperbody stopblow
		//change anim dontblow
	}


	/*public void changeUpperBody(upperBodyAnimation.state UBAnim){
		//upperBody Animation
		m_upperBodyScript.changeAnimation(UBAnim);
	}*/

	/*public void changeLowerBody(lowerBodyAnimation.state LBAnim){
		//lowerBody Animation
		m_lowerBodyScript.changeAnimation(LBAnim);
	}*/
}
