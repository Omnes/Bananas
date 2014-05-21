using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeaNet : MonoBehaviour {

	private static SeaNet instance;
	public static SeaNet Instance{
		get{
			if (instance == null) {
				GameObject go = new GameObject();
				instance = go.AddComponent<SeaNet>();
				go.name = "SeaNet";
			}
			return instance;
		}
	}

	public List<PlayerData> m_connectedPlayers;
	private string m_nextScene = "LemonPark";
	public Winstate m_winstate;
	public WinstateAnimation m_winstateAnimation;
	public NetworkView m_networkView;

	public bool m_gameEnded = false;

	public string m_sceneAfterWin = "MainMenuScene";
	public string m_sceneStateAfterWin = "MainMenu";

	public LobbyButton m_leaveButton;
	public LobbyButton m_rematchButton;



	private Vector2 m_size;
	
	private Vector2 m_leaveButtonPos;
	private Vector2 m_rematchButtonPos;
	private Vector2 m_winNamePos;

	private string m_winnerName = "";
	private Texture2D m_winTexture;
	private GUIStyle m_gui;

	public int m_endButtonTime = 200;
	private int m_endButtonCounter = 0;


	private int m_gameTime; 

	public static bool isNull(){
		return instance == null;
	}

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);

		m_winstate = gameObject.AddComponent<Winstate>();
		m_winstateAnimation = gameObject.AddComponent<WinstateAnimation>();

		gameObject.AddComponent<NetworkView>();
		gameObject.networkView.stateSynchronization = NetworkStateSynchronization.Off;

		
		
		m_size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		m_winNamePos = new Vector2((Screen.width / 2) - (m_size.x / 2), Screen.height - (m_size.y + 30));
		m_leaveButtonPos = new Vector2(Screen.width - m_size.x, Screen.height - m_size.y);
		m_rematchButtonPos = new Vector2(0, Screen.height - m_size.y);

		m_leaveButton = new LobbyButton(m_leaveButtonPos.x, m_leaveButtonPos.y + 100, m_size.x, m_size.y,		"Leave Game", m_leaveButtonPos, 3.0f, LeanTweenType.easeOutElastic);
		m_rematchButton = new LobbyButton(m_rematchButtonPos.x, m_rematchButtonPos.y + 100, m_size.x, m_size.y,	"Rematch", m_rematchButtonPos, 3.0f, LeanTweenType.easeOutElastic);
	}


	public void setConnectedPlayers(List<PlayerData> arr){
		m_connectedPlayers = arr;
	}

	//get local player
	public int getLocalPlayer(){
		foreach(PlayerData pd in m_connectedPlayers){
			if(pd.m_isLocal){
				return pd.m_id;
			}
		}
		return -1;
	}

	//return playerlist
	public List<PlayerData> getPlayerArr(){
		return m_connectedPlayers;
	}

	//return playernames
	public string[] getPlayerNames(){
		string[] tempArr = new string[4];
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			tempArr[i] = m_connectedPlayers[i].m_name;
		}
		return tempArr;
	}

	public void startGame(){
		if(Network.isServer){
			networkView.RPC ("RPCStartGame",RPCMode.All,m_gameTime);
		}
	}


	[RPC]
	public void RPCStartGame(int gameTime){
//		m_winstate.gameStart();
		m_winstate.StartGameTimer(gameTime);
	}

	public void setMaxTime(int maxTime){
//		m_winstate.m_MAXTIME = maxTime;
		m_gameTime = maxTime;
	}

	//save and shut down the game. this happens when time is up
	public void savePlayersAndShutDown(int id){
		networkView.RPC("endGameSceneRPC", RPCMode.All, id);
	}

	void OnGUI(){
		if(m_gameEnded){

			m_endButtonCounter++;

			if(m_endButtonCounter > m_endButtonTime){
				m_leaveButton.move();
				if(m_leaveButton.isClicked()){
					//load level
					networkView.RPC("stopGameRPC", RPCMode.All, "MainMenuScene", "MainMenu");
					//disconnect form game
					disconnect();
				}

				if(m_winnerName == ""){

					m_gui = new GUIStyle();
					m_gui.fontSize = 22;

					for(int i = 0; i < m_connectedPlayers.Count; i++){
						if(m_connectedPlayers[i].m_id == ScoreKeeper.GetFirstPlaceID()){

							m_winnerName = m_connectedPlayers[i].m_name;
							m_winTexture = Prefactory.texture_winnerOther;
							if(getLocalPlayer() == i){
								m_winnerName = "";
								m_winTexture = Prefactory.texture_winner;
							}
						}
					}
				}

				GUI.DrawTexture(new Rect(m_winNamePos.x, m_winNamePos.y, m_size.x, m_size.y), m_winTexture);
				GUI.Label(new Rect(m_winNamePos.x + 20, m_winNamePos.y + (m_size.y / 2),  m_size.x, m_size.y), m_winnerName, m_gui);

				m_rematchButton.move();
				if(m_rematchButton.isClicked()){
					//load level, menustate doesnt matter here
					networkView.RPC("stopGameRPC", RPCMode.All, m_nextScene, "MainMenu");
				}
			}
		}
	}

	public void disconnect(){

		//måste säga till alla att spelet är slut
		if(Network.isServer){
			m_connectedPlayers = null;
			Network.Disconnect();
		}else if(Network.isClient){
			m_connectedPlayers = null;
			Network.Disconnect();
		}
	}

	[RPC]
	private void stopGameRPC(string nextScene, string menuState){

		//clean
		m_winnerName = "";
		m_endButtonCounter = 0;
		m_leaveButton.resetButton();
		m_rematchButton.resetButton();

		//stops recieving of information over network
//		Network.SetSendingEnabled(Network.player, 0, false);
//		//stops messsages over network
//		Network.isMessageQueueRunning = false;
		//
		//reset score

		SoundManager.Instance.ResetMusic();
		ScoreKeeper.ResetScore();
		m_gameEnded = false;
		MenuManager.remoteMenu = menuState;
		Application.LoadLevel(nextScene);
	}


	[RPC]
	private void endGameSceneRPC(int id){
		SoundManager.Instance.StartWinMusic();
		m_gameEnded = true;
		m_winstateAnimation.playWinScene(id);
	}

//	[RPC]
//	private void startGameRPC(){
//		//starts sending data
//		//Network.isMessageQueueRunning = true;
//		//starts sending of information over network
//		//Network.SetSendingEnabled(Network.player, 0, true);
//
//		Debug.Log("MEJAHHAEHE");
//	}

}



