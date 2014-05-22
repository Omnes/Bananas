using UnityEngine;
using System.Collections;

public class lowerBodyAnimation : MonoBehaviour {

	// ska dessa vara med m_ ?
	public enum state { 
		IDLE, 
		RUNNING,
		BLOW,
		STOPBLOW
	};

	private state m_myState;
	private state m_currentState;
//	private state m_previousState;

	private Animator m_playerAnimator;

	//
	public bool m_running;
	public bool m_blowing;
	//public bool m_idle;




	// Use this for initialization
	void Start () {
		//enum
		//upperBodyEnum m_myState;
		m_myState = state.IDLE;
		m_currentState = m_myState;
		//hämta animationer osv
		m_playerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_myState != m_currentState){
			switch (m_myState){
				case state.RUNNING:
				{
					runningAnimation();
					m_currentState = state.RUNNING;
					break;	
				}
					
				case state.IDLE:
				{
					idleAnimation();
					m_currentState = state.IDLE;
					break;
				}
				case state.BLOW:
				{
					if(!m_running){
						blowAnimation();
					}
					m_currentState = state.BLOW;
					break;
				}
				case state.STOPBLOW:
				{
					stopBlowAnimation();
					m_currentState = state.STOPBLOW;
					break;
				}
			}
		}
	}

	public void runningAnimation(){
//		m_previousState = m_currentState;

		m_playerAnimator.SetFloat("playerSpeed", 1);
		m_running = true;
	}

	public void idleAnimation(){
//		m_previousState = m_currentState;
		
		m_playerAnimator.SetFloat("playerSpeed", 0);
		m_running = false;
	}

	public void blowAnimation(){
//		m_previousState = m_currentState;

		m_playerAnimator.SetBool("isBlowing", true);
	}

	public void stopBlowAnimation(){
//		m_previousState = m_currentState;

		m_playerAnimator.SetBool("isBlowing", false);
	}
//
//	public void previousAnimation(){
//		m_myState = m_previousState;
//	}

	public void changeAnimation(state LBAnim){
		m_myState = LBAnim;
	}
}
