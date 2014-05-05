using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class upperBodyAnimation : MonoBehaviour {

	// ska dessa vara med m_ ?
	public enum state { 
		BLOW,
		RUNNING,
		IDLE,
		TACKLE,
		NONE
	};

	private state m_myState;
	private state m_currentState;
	private state m_previousState;
	public bool m_isTackling;

	public state[] m_priorityList = new state[3];
	//public state[] m_priorityList = {state.NONE, state.NONE, state.IDLE};

	private Animator m_playerAnimator;

	// Use this for initialization
	void Start () {
		//enum
		//upperBodyState m_myState;
		m_myState = state.IDLE;
		m_currentState = m_myState;
		m_isTackling = false;


		//currentstates
		//NONE / TACKLE / TACKLED
		m_priorityList[0] = state.NONE;
		//NONE / BLOWRUN / BLOWIDLE
		m_priorityList[1] = state.NONE;
		//RUN / IDLE
		m_priorityList[2] = state.IDLE;

		//hämta animationer osv
		m_playerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//fixa så if state != currentstate kör func
		if(m_myState != m_currentState){

//			if(m_myState == state.BLOW){
//				int statePriority = 0;
//
//				if(!checkHighPriority(statePriority)){
//					m_priorityList[1] = state.BLOW;
//					m_playerAnimator.SetBool("isBlowing", true);
//					m_currentState = state.BLOW;
//				}
//
//			}else 
		if(m_myState == state.RUNNING){
				int statePriority = 1;

				if(!checkHighPriority(statePriority)){
					m_priorityList[2] = state.RUNNING;
					m_playerAnimator.SetFloat("playerSpeed", 1);
					m_currentState = state.RUNNING;
				}
		
			}else if(m_myState == state.IDLE){
				int statePriority = 1;

				if(!checkHighPriority(statePriority)){
					m_priorityList[2] = state.IDLE;
					m_playerAnimator.SetFloat("playerSpeed", 0);
					m_currentState = state.IDLE;
				}
			}
		}



//				case state.STOPBLOW:
//				{
//					m_playerAnimator.SetBool("isBlowing", false);
//					m_currentState = state.STOPBLOW;
//					break;
//				}

//				case state.TACKLE:
//				{
//					tackleAnimation();
//					m_currentState = state.TACKLE;
//					break;
//				}


		//tackle
//		if(m_isTackling){
//			//wait for tackle to complete
//			//if(tackle is complete){
//			//	m_isTackling = false;
//			//m_myState = m_previousState;
//			//}
//		}

	}



	//states

	//IDLE
	public void idleAnimation(){
		m_previousState = m_currentState;

		m_playerAnimator.SetFloat("playerSpeed", 0);			//ta bort sen.
	}

	//running
	public void runningAnimation(){
		m_previousState = m_currentState;

		m_playerAnimator.SetFloat("playerSpeed", 1);			//ta bort sen.
	}

	//tackle animation
	public void tackleAnimation(){
		m_priorityList[0] = state.TACKLE;
		m_playerAnimator.SetBool("tackle", true);
		m_currentState = state.TACKLE;
	}

	//tackle animation
	public void stopTackleAnimation(){
//		m_priorityList[0] = state.TACKLE;
//		m_playerAnimator.SetBool("tackle", true);
//		m_currentState = state.TACKLE;
	}

	//blowing while idle animation
//	public void blowIdleAnimation(){
//		m_previousState = m_currentState;
//
//		m_playerAnimator.SetBool("isBlowing", true);
//	}

	//blowing while running animation
	public void blowAnimation(){
		int statePriority = 0;

		if(!checkHighPriority(statePriority)){
			m_priorityList[1] = state.BLOW;
			m_playerAnimator.SetBool("isBlowing", true);
			m_currentState = state.BLOW;
		}
	}

	public void stopBlowAnimation(){
		m_priorityList[1] = state.NONE;
		m_playerAnimator.SetBool("isBlowing", false);
	}

	//check if higher priority states exist
	public bool checkHighPriority(int priority){
		for(int i = 0; i < priority; i++){
			if(m_priorityList[i] != state.NONE){
				return true;
			}
		}
		return false;
	}

//	public void previousAnimation(){
//		Debug.Log("UpperBody : PreviousAnimation");
//		m_myState = m_previousState;
//	}

	//utomstående funktion kallar denna för att ändra state
	public void changeAnimation(state UBAnim){
		m_myState = UBAnim;
	}


}
