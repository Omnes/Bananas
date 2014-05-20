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

		//knappar
		Vector2 size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		Vector2 leaveButtonPos = new Vector2(Screen.width - size.x, Screen.height - size.y);
		Vector2 rematchButtonPos = new Vector2(0, Screen.height - size.y);

		m_leaveButton = new LobbyButton(leaveButtonPos.x, leaveButtonPos.y + 100, size.x, size.y,		"Leave Game", leaveButtonPos, 3.0f, LeanTweenType.easeOutElastic);
		m_rematchButton = new LobbyButton(rematchButtonPos.x, rematchButtonPos.y + 100, size.x, size.y,	"Rematch", rematchButtonPos, 3.0f, LeanTweenType.easeOutElastic);

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
		m_winstate.gameStart();
	}

	public void setMaxTime(int maxTime){
		m_winstate.m_MAXTIME = maxTime;
	}

	//save and shut down the game. this happens when time is up
	public void savePlayersAndShutDown(int id){
		networkView.RPC("endGameSceneRPC", RPCMode.All, id);
	}

	void OnGUI(){
		if(m_gameEnded){
			Vector2 size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

			Vector2 leaveButtonPos = new Vector2(Screen.width - size.x, Screen.height - size.y);
			Vector2 rematchButtonPos = new Vector2(0, Screen.height - size.y);

			m_leaveButton.move();
			if(m_leaveButton.isClicked()){

			//if(GUI.Button(new Rect(leaveButtonPos.x, leaveButtonPos.y, size.x, size.y), "Leave game")){
				//load level
				networkView.RPC("stopGameRPC", RPCMode.All, "MainMenuScene", "MainMenu");
				//disconnect form game
				disconnect();
			}
			m_rematchButton.move();
			if(m_rematchButton.isClicked()){
			
			//if(GUI.Button(new Rect(rematchButtonPos.x, rematchButtonPos.y, size.x, size.y), "Rematch")){
				//load level, menustate doesnt matter here
				networkView.RPC("stopGameRPC", RPCMode.All, m_nextScene, "MainMenu");
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



