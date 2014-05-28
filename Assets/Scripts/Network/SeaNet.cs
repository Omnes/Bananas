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
	public Winstate m_winstate;
	public WinstateAnimation m_winstateAnimation;
	//public NetworkView m_networkView;

//	public bool m_gameEnded = false;


	private Vector2 m_size;

	private int m_gameTime; 
	private int m_loadedPlayers = 0;
	public float m_startDelay = 3f;

	public static bool isNull(){
		return instance == null;
	}

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);

		gameObject.AddComponent<NetworkView>();
		gameObject.networkView.stateSynchronization = NetworkStateSynchronization.Off;

		m_size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
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
	
	public void setMaxTime(int maxTime){
		m_gameTime = maxTime;
	}

	//save and shut down the game. this happens when time is up
	public void savePlayersAndShutDown(int id){
		networkView.RPC("endGameSceneRPC", RPCMode.All, id);
	}

	public void loadLevel(string level){
		networkView.RPC("loadLevelRPC", RPCMode.All,level);
	}

	public void stopGame(string nextscene, string nextstate){
		networkView.RPC("stopGameRPC", RPCMode.All, nextscene, nextstate);
	}

	//disconnect a player from the severr
	public void disconnect(){
		//if server leaves, remove everyone from game
		if(Network.isServer){
			networkView.RPC("disconnectServer", RPCMode.Others);
			stopGameRPC("MainMenuScene", "StartingScreen");
			m_connectedPlayers.Clear();
			Network.Disconnect();
															//RPC KICK ALL PLAYERS


		//if client leaves, remove him from everyone
		}else if(Network.isClient){
			networkView.RPC ("disconnectRPC", RPCMode.Others, getLocalPlayer());
			stopGameRPC("MainMenuScene", "StartingScreen");
			m_connectedPlayers.Clear();
			Network.Disconnect();
		}
	}

	[RPC]
	private void disconnectRPC(int playerId){
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			if(playerId == m_connectedPlayers[i].m_id){
				m_connectedPlayers.RemoveAt(i);
			}
		}
	}

	[RPC]
	private void disconnectServer(){
		stopGameRPC("MainMenuScene", "StartingScreen");
		m_connectedPlayers.Clear();
		Network.Disconnect();
	}

	[RPC]
	private void stopGameRPC(string nextScene, string menuState){
		SoundManager.Instance.ResetMusic();
		ScoreKeeper.ResetScore();
		MenuManager.remoteMenu = menuState;
		loadLevelRPC(nextScene);

	}

	[RPC]
	private void loadLevelRPC(string level){
		//reset winstate and so
		resetComponents ();

		m_loadedPlayers = 0; //reset number of loaded players
		StartCoroutine(networkLoadLevel(level));
		//		Application.LoadLevel(level);
	}
	
	//not a unity default....
	void NetworkLevelLoaded(int level){
		if(level != 0){	
			LoadingScreen.SetLoadingText("Waiting for other players...");
			if(Network.isClient){
				networkView.RPC("RPCConfirmLoaded",RPCMode.Server);
			}else{
				RPCConfirmLoaded(); //in singelplayer the server do not consider itself a server
			}
		}else{
			LoadingScreen.CloseLoadingScreen();
		}

	}

	[RPC]
	public void RPCConfirmLoaded(){
		m_loadedPlayers++;
		if(m_loadedPlayers >= m_connectedPlayers.Count){
			startGame();

		}
	}

	public void startGame(){
		if(Network.isServer){
			networkView.RPC ("RPCStartGame",RPCMode.All,m_gameTime);
		}
	}

	[RPC]
	public void RPCStartGame(int gameTime){
		//EVERYONE IS LOADED AND READY TO GO
		LoadingScreen.CloseLoadingScreen();
		StartCoroutine(StartGameDelayed(gameTime,m_startDelay));
		CountdownAnimation.instance.Play();
		//start the music!
		SoundManager.Instance.StartIngameMusic();
		SoundManager.Instance.playOneShot(SoundManager.COUNTDOWN);

		m_winstateAnimation.m_playerAmount = m_connectedPlayers.Count;
	}

	private IEnumerator StartGameDelayed(int gameTime,float delay){
		yield return new WaitForSeconds(delay);
		m_winstate.StartGameTimer(gameTime);
		for(int i = 0; i < 4;i++){
			if(SyncMovement.s_syncMovements[i] != null){
				SyncMovement.s_syncMovements[i].GetComponent<InputHub>().ClearMovementStuns();
				SyncMovement.s_syncMovements[i].GetComponent<InputHub>().ClearLeafBlowerStuns();
			}
		}
	}
	
	private IEnumerator networkLoadLevel(string level){

//		Network.SetSendingEnabled(0, false);
//		Network.isMessageQueueRunning = false;
		LoadingScreen.OpenLoadingScreen("Loading...");
		yield return new WaitForEndOfFrame();
		Application.LoadLevel(level);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

//		Network.isMessageQueueRunning = true;
//		Network.SetSendingEnabled(0, true);

		NetworkLevelLoaded(Application.loadedLevel);
//		// Notify our objects that the level and the network is ready
//		foreach (GameObject go in FindObjectsOfType(typeof(GameObject))){
//			go.BroadcastMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
//		}
	}


	[RPC]
	private void endGameSceneRPC(int id){
		m_winstateAnimation.m_gameEnded = true;
		SoundManager.Instance.StartWinMusic();
		m_winstateAnimation.playWinScene(id);
	}

	private void resetComponents(){
		//remove so they can be set again anew
		Destroy (m_winstate);
		Destroy (m_winstateAnimation);

		m_winstate = gameObject.AddComponent<Winstate>();
		m_winstateAnimation = gameObject.AddComponent<WinstateAnimation>();
	}

	public void setRematchCheck(int state){
		networkView.RPC ("setCheckRPC", RPCMode.All, getLocalPlayer(), state);
	}

	[RPC]
	private void setCheckRPC(int playerId, int state){
		m_winstateAnimation.SetRematchCheck(playerId, state);
	}
	
}



