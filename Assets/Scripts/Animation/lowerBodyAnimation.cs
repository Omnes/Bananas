using UnityEngine;
using System.Collections;

public class lowerBodyAnimation : MonoBehaviour {

	// ska dessa vara med m_ ?
	public enum state { 
		IDLE, 
		RUNNING,
		BLOW
	};

	private state myEnum;
	private state currentEnum;

	private Animator m_playerAnimator;

	//
	public bool m_running;
	public bool m_blowing;
	//public bool m_idle;




	// Use this for initialization
	void Start () {
		//enum
		//upperBodyEnum myEnum;
		myEnum = state.IDLE;
		currentEnum = myEnum;
		//hämta animationer osv
		m_playerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(myEnum != currentEnum){
			switch (myEnum){
				case state.RUNNING:
				{
					runningAnimation();
					currentEnum = state.RUNNING;
					break;	
				}
					
				case state.IDLE:
				{
					idleAnimation();
					currentEnum = state.IDLE;
					break;
				}
				case state.BLOW:
				{
					blowAnimation();
					currentEnum = state.BLOW;
					break;
				}
			}
		}
	}

	public void runningAnimation(){
		//Debug.Log("LowerBody : runningAnimation");
		m_playerAnimator.SetFloat("playerSpeed", 1);
		m_running = true;
		//switch to run in animator
		//m_playerAnimator.SetLayerWeight(1, 1);
	}

	public void idleAnimation(){
		//Debug.Log("LowerBody : idleAnimation");
		
		m_playerAnimator.SetFloat("playerSpeed", 0);
		m_running = false;
		//switch to idle in animator
		//m_playerAnimator.SetLayerWeight(1, 0);
	}

	public void blowAnimation(){
		//Debug.Log("LowerBody : blowAnimation");
		if(!m_running){
			m_playerAnimator.SetFloat("playerSpeed", 0);
		}
	}

	public void changeAnimation(state LBAnim){
		myEnum = LBAnim;
	}
}
