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
	private bool m_leaveGame = false;


	private bool m_startTimer = false;
	private float m_endScreenDelay = 20;
	private float m_endScreenCounter = 0;


	//Button properties
	private float LeaveButtonXpos;
	private float LeaveButtonYpos;
	private Vector2 LeaveButtonSize;

	private float RematchButtonXpos;
	private float RematchButtonYpos;
	private Vector2 RematchButtonSize;

	// Use this for initialization
	void Start () {

		//Zero the cooldown that "came" from lobby
		MenuManager.m_lobbyCoolDown = 0.0f;


		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		m_connectedPlayers = SeaNet.Instance.getPlayerArr ();

		m_size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		m_winNamePos = new Vector2((Screen.width / 2) - (m_size.x / 2), Screen.height - (m_size.y + 30));
		m_leaveButtonPos = new Vector2(Screen.width - m_size.x, Screen.height - m_size.y);
		m_rematchButtonPos = new Vector2(0, Screen.height - m_size.y);
	
		LeaveButtonSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.33f, 0.15f));
		LeaveButtonXpos = screenWidth - LeaveButtonSize.x;
		LeaveButtonYpos = screenHeight - LeaveButtonSize.y;
		m_leaveButton = new LobbyButton(LeaveButtonXpos, LeaveButtonYpos, LeaveButtonSize.x, LeaveButtonSize.y,		new Rect(0.0f, 0.0f, 0.56f, 0.14f), new Vector2(LeaveButtonXpos, LeaveButtonYpos), 3.0f, LeanTweenType.easeOutElastic);


		RematchButtonSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.33f, 0.15f));
		RematchButtonXpos = 0.0f;
		RematchButtonYpos = screenHeight - RematchButtonSize.y;
		m_rematchButton = new LobbyButton(RematchButtonXpos, RematchButtonYpos, RematchButtonSize.x, RematchButtonSize.y,	new Rect(0.0f, 0.14f, 0.56f, 0.14f), new Vector2(RematchButtonXpos, RematchButtonYpos), 3.0f, LeanTweenType.easeOutElastic);

		
		for (int i = 0; i < m_rematchChecks.Length; i++) {
			m_rematchChecks[i] = state.NONE;
		}

	}


	public void playWinScene(int id){
		StartCoroutine(playWinSceneCorutine(id));
	}

	private IEnumerator playWinSceneCorutine(int id){
		SyncMovement[] syncMovements = SyncMovement.s_syncMovements;
		BuffManager[] buffManagers = BuffManager.m_buffManagers;

		//clear all buffs from all players and restun them
		for(int i = 0; i < buffManagers.Length; i++){
			if(buffManagers[i] != null){
				buffManagers[i].RemoveAll();
				buffManagers[i].GetComponent<InputHub>().StunLeafBlower();
				buffManagers[i].GetComponent<InputHub>().StunMovement();
				buffManagers[i].GetComponentInChildren<LeafBlower>().requestDropAll();
			}
		}

		//play the animations 
		float moveDuration = 1f;
		float fadeDuration = 0.5f;
		TimesUpAnimation.instance.Play(new Vector2(0,0),moveDuration,0.5f);
		GUITimer.s_lazyInstance.Play(new Vector2(0,1f),4f);
		//move player to center
//		StartCoroutine(LerpPlayerToPosition(syncMovements[id].transform,new Vector3(0,syncMovements[id].transform.position.y,0),moveDuration+fadeDuration));

		

		//wait for the timesup to appear
		yield return new WaitForSeconds(moveDuration);
		SoundManager.Instance.StartWinMusic();
		yield return new WaitForSeconds(fadeDuration);
		//disable this or the LerpPlayer thingy
		syncMovements[id].transform.position = Vector3.zero + new Vector3(0,syncMovements[id].transform.position.y,0);

		//disable all non-winning players
		for(int i = 0; i < buffManagers.Length; i++){
			if(buffManagers[i] != null){
				if(i != id){
					buffManagers[i].gameObject.SetActive(false);
				}
			}
		}

		//begin lerp the camera to the player 
		float cameraLerpDuration = 2f;
		Vector3 camPos = new Vector3(-4,3,0);
		Quaternion rot = Quaternion.LookRotation(Vector3.zero-camPos); //playerposition - cameraposition
		Camera.main.GetComponent<CameraFollow>().MoveToPosition(camPos,rot,cameraLerpDuration);
		
		//make mplayer look at camera
		Vector3 lookAtPos = camPos - Vector3.up * camPos.y;
		syncMovements[id].transform.LookAt(lookAtPos);
		syncMovements[id].transform.Rotate(Vector3.up*12f);

		yield return new WaitForSeconds(cameraLerpDuration-0.3f);
		
		SoundManager.Instance.playOneShot(SoundManager.VOICE_VICTORY[id]);
		syncMovements[id].GetComponent<playerAnimation>().winAnim();

	}

	IEnumerator LerpPlayerToPosition(Transform player,Vector3 endPos,float duration){
		float startTime = Time.time;
		Vector3 startPosition = player.position;
		while(startTime + duration > Time.time){
			float t = (Time.time - startTime)/duration;
			player.position = Vector3.Lerp(startPosition,endPos,t);
			yield return null;
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
					SeaNet.Instance.setRematchCheck((int)state.LEAVE);

					m_leaveGame = true;

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
					SeaNet.Instance.setRematchCheck((int)state.REMATCH);

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

		if (!m_leaveGame) {
			if (newState == (int)state.LEAVE) {
				//do gui check


	
			} else {
				//do GUI check

				//### REMATCH ###
				if(Network.isServer){
					int rematchAmount = 0;
					//if all => rematch
					for (int i = 0; i < m_rematchChecks.Length; i++) {
						if (m_rematchChecks[i] == state.REMATCH) {
							rematchAmount++;
						}
					}
					//if rematchplayers are same as amount of players, play rematch
					if (rematchAmount >= m_playerAmount) {
						//																		REMATAACHACHAHCHACH <----------------------------
						//load level, MenuState ("MainMenu") doesnt matter here
						SeaNet.Instance.stopGame ("LemonPark", "");
					}
				}
			}

			//### REMATCH FROM LOBBY ###
			if(Network.isServer){
				//players who press leave
				int leaveAmount = 0;
				//players who clicked something
				int clickedAmount = 0;

				//if alone => menulobby
				//if !alone => lobby
				for (int i = 0; i < m_rematchChecks.Length; i++) {
					if (m_rematchChecks [i] != state.NONE) {
						clickedAmount++;
					}
					if (m_rematchChecks [i] == state.LEAVE) {
						leaveAmount++;
					}
				}
				if (leaveAmount > 0 && clickedAmount >= m_playerAmount) {
					//																		LEAVE TO LOBBY <----------------------------------
					SeaNet.Instance.stopGame ("MainMenuScene", "Lobby");
				}
			}

		}
	}
}
