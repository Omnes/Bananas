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
		STUN,
		NONE
	};

	private state m_myState;
	private state m_currentState;
//	private state m_previousState;
	public bool m_isTackling;

	public state[] m_priorityList = new state[3];
	//public state[] m_priorityList = {state.NONE, state.NONE, state.IDLE};

	public Animator m_playerAnimator;

	private Rigidbody m_player;

	// Use this for initialization
	void Start () {
		//enum
		//upperBodyState m_myState;
		m_myState = state.IDLE;
		m_currentState = m_myState;
		m_isTackling = false;

		//player
		m_player = rigidbody;

		//currentstates
		//NONE / TACKLE / TACKLED
		m_priorityList[0] = state.NONE;
		//NONE / BLOWRUN / BLOWIDLE
		m_priorityList[1] = state.NONE;
		//RUN / IDLE
		m_priorityList[2] = state.IDLE;

		//hämta animationer osv
		//m_playerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
//	void Update () {
//
////		if (m_currentState == state.RUNNING) {
////			//Debug.Log(m_player.rigidbody.velocity.magnitude);
////			m_playerAnimator.SetFloat("playerSpeed", m_player.velocity.magnitude);
////		}
//
//		//fixa så if state != currentstate kör func
//		if(m_playerAnimator != null){
//			if(m_myState != m_currentState){
//				if(m_myState == state.RUNNING){
//						int statePriority = 1;
//
//						if(!checkHighPriority(statePriority)){
//							m_priorityList[2] = state.RUNNING;
//							m_playerAnimator.SetBool("running", true);
//							m_currentState = state.RUNNING;
//						}
//				
//					}else if(m_myState == state.IDLE){
//						int statePriority = 1;
//
//						if(!checkHighPriority(statePriority)){
//							m_priorityList[2] = state.IDLE;
//							m_playerAnimator.SetBool("running", false);
//							m_currentState = state.IDLE;
//						}
//					}
//			}
//		}
//	}



		void Update () {
	
		}


	//states

	public void runAnimation(){

		if(m_playerAnimator != null){

			m_myState = state.RUNNING;
			if(m_myState != m_currentState){

				int statePriority = 1;

				if(!checkHighPriority(statePriority)){

					m_priorityList[2] = state.RUNNING;
					m_playerAnimator.SetBool("running", true);
					m_currentState = state.RUNNING;
				}
			}
		}
	}

	public void idleAnimation(){
		if(m_playerAnimator != null){

			m_myState = state.IDLE;
			if(m_myState != m_currentState){
				int statePriority = 1;

				if(!checkHighPriority(statePriority)){
					m_priorityList[2] = state.IDLE;
					m_playerAnimator.SetBool("running", false);
					m_currentState = state.IDLE;
				}
			}
		}
	}

	//tackle animation
	public void tackleAnimation(float dizzyTime){
		if(m_currentState != state.TACKLE){
			//Debug.Log("TACKLE");
			m_priorityList[0] = state.TACKLE;
			m_playerAnimator.SetBool("tackle", true);
			m_currentState = state.TACKLE;
			m_playerAnimator.SetFloat("playerSpeed", 5);

			StartCoroutine(tackleCoroutine("tackle", dizzyTime));

		}
	}

	//tackle animation
	public void tackleLoseAnimation(float dizzyTime){
		if(m_currentState != state.TACKLE){
			//Debug.Log("TACKLE");
			m_priorityList[0] = state.TACKLE;
			m_playerAnimator.SetBool("tackleLose", true);
			m_currentState = state.TACKLE;
			m_playerAnimator.SetFloat("playerSpeed", 5);

			StartCoroutine(tackleCoroutine("tackleLose", dizzyTime));
			
		}
	}

	IEnumerator tackleCoroutine(string varibleName, float dizzytime){
		yield return new WaitForSeconds(dizzytime);
		
		m_priorityList[0] = state.NONE;
		m_playerAnimator.SetBool(varibleName, false);
		m_currentState = state.NONE;
	}

	public void stunAnimation(){
		if(m_currentState != state.STUN){
			//Debug.Log("TACKLE");
			m_priorityList[0] = state.STUN;
			m_playerAnimator.SetBool("stun", true);
			m_currentState = state.STUN;
			m_playerAnimator.SetFloat("playerSpeed", 5);
		}
	}

	public void stopStunAnimation(){
		m_priorityList[0] = state.NONE;
		m_playerAnimator.SetBool("stun", false);
	}

	//blowing while running animation
	public void blowAnimation(){
	}

	public void stopBlowAnimation(){
		//Debug.Log("STOPBLOW");
		m_priorityList[1] = state.NONE;
		m_playerAnimator.SetBool("isBlowing", false);
	}

	public void winAnimation(){
		m_playerAnimator.SetBool("win", true);
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

	//utomstående funktion kallar denna för att ändra state
	public void changeAnimation(state UBAnim){
		m_myState = UBAnim;
	}

	public void setAnimator(Animator anim){
		m_playerAnimator = anim;
	}

}
