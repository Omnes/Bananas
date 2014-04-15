using UnityEngine;
using System.Collections;

public class upperBodyAnimation : MonoBehaviour {

	// ska dessa vara med m_ ?
	public enum state {
		BLOWIDLE, 
		TACKLE, 
		BLOWRUNNING, 
		IDLE, 
		RUNNING
	};

	private state m_myState;
	private state m_currentState;
	private state m_previousState;
	public bool m_isTackling;

	private Animator m_playerAnimator;

	// Use this for initialization
	void Start () {
		//enum
		//upperBodyState m_myState;
		m_myState = state.IDLE;
		m_currentState = m_myState;
		m_isTackling = false;

		//hämta animationer osv
		m_playerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//fixa så if state != currentstate kör func
		if(m_myState != m_currentState || m_isTackling != true){
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

				case state.TACKLE:
				{
					tackleAnimation();
					m_currentState = state.TACKLE;
					break;
				}

				case state.BLOWIDLE:
				{
					blowIdleAnimation();
					m_currentState = state.BLOWIDLE;
					break;
				}

				case state.BLOWRUNNING:
				{
					blowRunningAnimation();
					m_currentState = state.BLOWRUNNING;
					break;
				}
			}
		}

		//tackle
		if(m_isTackling){
			//wait for tackle to complete
			//if(tackle is complete){
			//	m_isTackling = false;
			//m_myState = m_previousState;
			//}
		}

	}



	//states

	//IDLE
	public void idleAnimation(){
		Debug.Log("UpperBody : idleAnimation");
		//start idle animation
		//m_playerAnimator.SetLayerWeight(2, 1);
		m_playerAnimator.SetFloat("playerSpeed", 0);			//ta bort sen.
	}

	//running
	public void runningAnimation(){
		Debug.Log("UpperBody : runningAnimation");
		//start running animation
		//m_playerAnimator.SetLayerWeight(2, 1);
		m_playerAnimator.SetFloat("playerSpeed", 1);			//ta bort sen.
	}

	//tackle animation
	public void tackleAnimation(){
		Debug.Log("UpperBody : tackleAnimation");
		//m_isTackling = true;
		//should be previous state hopefully
		//m_previousState = m_currentState;

		//do tackle
	}

	//blowing while idle animation
	public void blowIdleAnimation(){
		Debug.Log("UpperBody : blowIDLEAnimation");
		//start blowIDLE animation
		//m_playerAnimator.SetFloat("playerSpeed", 1);			//ta bort sen. ersätt med set layer
	}

	//blowing while running animation
	public void blowRunningAnimation(){
		Debug.Log("UpperBody : blowRUNNINGAnimation");
		//start blowRUNNING animation


	}

	//utomstående funktion kallar denna för att ändra state
	public void changeAnimation(state UBAnim){
		m_myState = UBAnim;
	}


}
