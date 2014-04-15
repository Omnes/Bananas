using UnityEngine;
using System.Collections;

public class playerAnimation : MonoBehaviour {

	public upperBodyAnimation m_upperBodyScript;
	public lowerBodyAnimation m_lowerBodyScript;

	// Use this for initialization
	void Start () {
		m_upperBodyScript = GetComponent<upperBodyAnimation>();
		m_lowerBodyScript = GetComponent<lowerBodyAnimation>();
	}
	
	// Update is called once per frame
	void Update () {

		/*if(Input.GetKey(KeyCode.I)){
			m_upperBodyScript.changeAnimation(upperBodyAnimation.state.Idle);
			m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.IDLE);

			//changeAnimation(upperBodyAnimation.state.Running, lowerBodyAnimation.state.Running);
		}
		//if ngt händer do animation
		if(Input.GetKey(KeyCode.O)){
			m_upperBodyScript.changeAnimation(upperBodyAnimation.state.Running);
			m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.RUNNING);

			//changeAnimation(upperBodyAnimation.state.Idle, lowerBodyAnimation.state.Idle);
		}*/

		/*if(Input.GetKey(KeyCode.K)){
			myEnum = animationState.Tackle;
		}
		if(Input.GetKey(KeyCode.L)){
			myEnum = animationState.BlowIdle;
		}
		if(Input.GetKey(KeyCode.G)){
			Debug.Log(myEnum.ToString());
		}*/

	}

	//upperbody
	//running
	public void runningAnim(){
		//set upperbody running
		//changeAnimation(running blablabl);
		//set lowerbody animation to running
		//changeAnimation(m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.Running));
		m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.RUNNING);
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.RUNNING);
	}
	//idle
	public void idleAnim(){
		//set upperbody idle
		m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.IDLE);
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.IDLE);
		//set lowerbody idle
		//changeAnimation(m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.Idle));
	}
	//tackle
	public void tackleAnim(){
		//set upperbody tackle
		//change animation (blablab)
	}
	//blow
	public void blowAnim(){

		m_lowerBodyScript.changeAnimation(lowerBodyAnimation.state.BLOW);
		m_upperBodyScript.changeAnimation(upperBodyAnimation.state.BLOWIDLE);

		//set upperbody blow
		//change anim blow
		//try to set lowerbody blow animation
	}
	//stopblow
	public void stopBlowAnim(){
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
