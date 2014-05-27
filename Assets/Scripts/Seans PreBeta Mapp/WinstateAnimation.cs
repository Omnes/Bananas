using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinstateAnimation : MonoBehaviour {

	public enum state { 
		REMATCH,
		LEAVE,
		NONE
	};

	private List<PlayerData> m_connectedPlayers = new List<PlayerData>();
	private state[] m_rematchChecks = new state[4];

	public bool m_gameEnded = false;

	public int m_endButtonTime = 200;
	private int m_endButtonCounter = 0;

	public LobbyButton m_leaveButton;
	public LobbyButton m_rematchButton;

	private Vector2 m_size;
	
	private Vector2 m_leaveButtonPos;
	private Vector2 m_rematchButtonPos;
	private Vector2 m_winNamePos;

	private string m_winnerName = "";
	private Texture2D m_winTexture;
	private GUIStyle m_gui;

	//private int m_rematchCounter = 0;
	private bool m_rematch = false;

	public int m_playerAmount = 0;

	//kanske ta bort
	private bool m_allPlayersRemain = true;

	//my rematch state
	private bool m_myRematchCheck = false;


	private bool m_startTimer = false;
	private float m_endScreenDelay = 20;
	private float m_endScreenCounter = 0;

	// Use this for initialization
	void Start () {
		m_connectedPlayers = SeaNet.Instance.getPlayerArr ();

		m_size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		m_winNamePos = new Vector2((Screen.width / 2) - (m_size.x / 2), Screen.height - (m_size.y + 30));
		m_leaveButtonPos = new Vector2(Screen.width - m_size.x, Screen.height - m_size.y);
		m_rematchButtonPos = new Vector2(0, Screen.height - m_size.y);

		m_leaveButton = new LobbyButton(m_leaveButtonPos.x, m_leaveButtonPos.y + 100, m_size.x, m_size.y,		"Leave Game", m_leaveButtonPos, 3.0f, LeanTweenType.easeOutElastic);
		m_rematchButton = new LobbyButton(m_rematchButtonPos.x, m_rematchButtonPos.y + 100, m_size.x, m_size.y,	"Rematch", m_rematchButtonPos, 3.0f, LeanTweenType.easeOutElastic);


		for (int i = 0; i < m_rematchChecks.Length; i++) {
			m_rematchChecks[i] = state.NONE;	
		}
	}

	void Update(){
		//kass
		if(m_gameEnded && !m_startTimer){
			m_startTimer = true;
			m_endScreenCounter = Time.time;
			Debug.Log("NU STARTAR TIDEN " + m_endScreenCounter);

			int firstPlaceID = ScoreKeeper.GetFirstPlaceID();
			SoundManager.Instance.playOneShot(SoundManager.VOICE_VICTORY[firstPlaceID]);
			BuffManager.m_buffManagers[firstPlaceID].RemoveAll();
			BuffManager.m_buffManagers[firstPlaceID].AddBuff(new StunBuff(BuffManager.m_buffManagers[firstPlaceID].gameObject, 0));
		}
		//start 
		if (m_startTimer) {
			if(Time.time > m_endScreenCounter + m_endScreenDelay){
				Debug.Log("TIDEN SLUTAR NU " + m_endScreenCounter);
				if(!m_allPlayersRemain && m_rematch){
					SeaNet.Instance.stopGame ("MainMenuScene", "Lobby");
				}else{
					SeaNet.Instance.disconnect();
					SeaNet.Instance.stopGame ("MainMenuScene", "StartingScreen");
				}
			}
		}

		//if rematch is true, check if you are allowed to start the match
		if(m_rematch){
			int temp = 0;
			for (int i = 0; i < m_rematchChecks.Length; i++) {
				if(m_rematchChecks[i].Equals(state.REMATCH)){
					temp++;
				}else if(m_rematchChecks[i].Equals(state.LEAVE)){
					m_allPlayersRemain = false;
				}
			}

			if (temp == m_playerAmount) {
				//load level, MenuState ("MainMenu") doesnt matter here
				SeaNet.Instance.stopGame ("LemonPark", "MainMenu");
			}
		}
	}

	public void playWinScene(int id){
		SyncMovement[] m_playerObjs = SyncMovement.s_syncMovements;
		BuffManager[] m_buffManagers = BuffManager.m_buffManagers;

		if(m_playerObjs[id] != null && m_buffManagers[id] != null){
			//pos
			Vector3 tempPos = m_playerObjs[id].transform.position;
			m_playerObjs[id].transform.position = new Vector3(0,tempPos.y,0);

			for(int i = 0; i < m_buffManagers.Length; i++){
				if(m_buffManagers[i] != null){
//					m_buffManagers[i].AddBuff(new StunBuff(m_buffManagers[i].gameObject, 0));
					if(i != id){
						m_buffManagers[i].gameObject.SetActive(false);
					}
				}
			}


			m_playerObjs[id].GetComponent<playerAnimation>().winAnim();

			//set position
			Camera.main.transform.position = new Vector3(-4,3,0);
			//disable smoothfollow
			Camera.main.GetComponent<CameraFollow>().enabled = false;

			//make mplayer look at camera
//			m_playerObjs[id].transform.LookAt(new Vector3(-7,0,0));
			Vector3 lookAtPos = Camera.main.transform.position - new Vector3(0,Camera.main.transform.position.y,0);
			m_playerObjs[id].transform.LookAt(lookAtPos);
			m_playerObjs[id].transform.Rotate(Vector3.up*12f);
			

			//
			Vector3 playerPos = m_playerObjs[id].transform.position - new Vector3(0,1,0);
			Camera.main.transform.LookAt(playerPos);

		}
	}


	void OnGUI(){
		//bool that activates from seanet
		if(m_gameEnded){
			m_endButtonCounter++;

			//timer
			if(m_endButtonCounter > m_endButtonTime){

				//chose texture for either winner or loser
				if(m_winnerName == ""){
					
					m_gui = new GUIStyle();
					m_gui.fontSize = 22;
					
					for(int i = 0; i < m_connectedPlayers.Count; i++){
						if(m_connectedPlayers[i].m_id == ScoreKeeper.GetFirstPlaceID()){
							
							m_winnerName = m_connectedPlayers[i].m_name;
							m_winTexture = Prefactory.texture_winnerOther;
							if(SeaNet.Instance.getLocalPlayer() == i){
								m_winnerName = "";
								m_winTexture = Prefactory.texture_winner;
							}
						}
					}
				}

				//leave button
				m_leaveButton.move();
				if(m_leaveButton.isClicked()){
					m_gameEnded = false;

					m_rematchChecks[SeaNet.Instance.getLocalPlayer()] = state.LEAVE;
					SeaNet.Instance.setRematchCheck((int)state.LEAVE);

					//load level
					//SeaNet.Instance.stopGame("MainMenuScene", "MainMenu");
					reset();
					//disconnect form game
					SeaNet.Instance.disconnect();
				}

				//rematch buttno
				m_rematchButton.move();
				if(m_rematchButton.isClicked()){
					//stop showing GUI
					m_gameEnded = false;

					//check rematchstate
					m_rematchChecks[SeaNet.Instance.getLocalPlayer()] = state.REMATCH;
					SeaNet.Instance.setRematchCheck((int)state.REMATCH);

					m_myRematchCheck = true;

					//reset buttons
					reset();
				}

				//draw stuff
				GUI.DrawTexture(new Rect(m_winNamePos.x, m_winNamePos.y, m_size.x, m_size.y), m_winTexture);
				GUI.Label(new Rect(m_winNamePos.x + 20, m_winNamePos.y + (m_size.y / 2),  m_size.x, m_size.y), m_winnerName, m_gui);

			}
		}
	}

	private void reset(){
		//clean
		m_winnerName = "";
		m_endButtonCounter = 0;
		m_leaveButton.resetButton();
		m_rematchButton.resetButton();
	}

	public void SetRematchCheck(int playerId, int newState){
		m_rematchChecks[playerId] = (state)newState;
		if (newState == (int)state.LEAVE) {
			//do gui check

			int rematchAndLeaveAmount = 0;
			//if alone => menulobby
			//if !alone => lobby
			for (int i = 0; i < m_rematchChecks.Length; i++) {
				if(m_rematchChecks[i] != state.NONE){
					rematchAndLeaveAmount++;
				}
			}
			if(rematchAndLeaveAmount >= m_playerAmount){
				//																		LEAVE TO LOBBY <----------------------------------
			}
			
		} else {
			//do GUI check

			int rematchAmount = 0;
			//if all => rematch
			for (int i = 0; i < m_rematchChecks.Length; i++) {
				if(m_rematchChecks[i].Equals(state.REMATCH)){
					rematchAmount++;
				}
			}
			//if rematchplayers are same as amount of players, play rematch
			if(rematchAmount >= m_playerAmount){
				//																		REMATAACHACHAHCHACH <----------------------------
			}
		}
	}

}
