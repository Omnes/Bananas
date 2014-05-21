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
	
	public List<int> m_IDs;
	public List<bool> m_isLocal;
	public List<string> m_names;

	private int m_gameTime; 
	private int m_loadedPlayers = 0;
	public float m_startDelay = 3f;

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
	}


	public void setConnectedPlayers(List<PlayerData> arr){
		m_connectedPlayers = arr;
	}

	//TA BORT DENNA SEN; ENDAST TEST
	public void testLookAtStuff(){
		Debug.Log("playess still in game "+m_connectedPlayers.Count);

		foreach(PlayerData e in m_connectedPlayers){
			m_IDs.Add(e.m_id);
			m_isLocal.Add (e.m_isLocal);
			m_names.Add(e.m_name);
		}
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
	
	void OnGUI(){
		if(m_gameEnded){
			Vector2 size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

			Vector2 leaveButtonPos = new Vector2(Screen.width - size.x, Screen.height - size.y);
			Vector2 rematchButtonPos = new Vector2(0, Screen.height - size.y);

			if(GUI.Button(new Rect(leaveButtonPos.x, leaveButtonPos.y, size.x, size.y), "Leave game")){
				//load level
				networkView.RPC("stopGameRPC", RPCMode.All, "MainMenuScene", "MainMenu");
				//disconnect form game
				disconnect();
			}
			if(GUI.Button(new Rect(rematchButtonPos.x, rematchButtonPos.y, size.x, size.y), "Rematch")){
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


	public void loadLevel(string level){
		networkView.RPC("loadLevelRPC", RPCMode.All,level);
	}

	[RPC]
	private void loadLevelRPC(string level){
		m_loadedPlayers = 0; //reset number of loaded players
		StartCoroutine(networkLoadLevel(level));
		//		Application.LoadLevel(level);
	}
	
	//not a unity default....
	void NetworkLevelLoaded(int level){
		if(level != 0){		
			if(Network.isClient){
				networkView.RPC("RPCConfirmLoaded",RPCMode.Server);
			}else{
				RPCConfirmLoaded(); //in singelplayer the server do not consider itself a server
			}
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
		if(Network.isClient){
			StartCoroutine(StartGameDelayed(gameTime,m_startDelay - Network.GetLastPing(Network.connections[0])/1000f));
		}else{
			StartCoroutine(StartGameDelayed(gameTime,m_startDelay));
		}
		//start the music!
		SoundManager.Instance.StartIngameMusic();
		SoundManager.Instance.playOneShot(SoundManager.COUNTDOWN);
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
	private void stopGameRPC(string nextScene, string menuState){
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
	
}



