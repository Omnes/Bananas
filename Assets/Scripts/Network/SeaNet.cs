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
	public string m_nextScene = "Sean_FakeWinScene";
	public Winstate m_winstate;


	//dessa två ska tas bort. ### ANVÄND EJ I SERIÖS SYFTE ###
	public List<int> m_IDs;
	public List<bool> m_isLocal;
	public List<string> m_names;

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);

		m_winstate = gameObject.AddComponent<Winstate>();

		//give seanet a networkview, so it can use RPC
//		NetworkView myNetwork = gameObject.AddComponent<NetworkView>();
//		myNetwork.observed = null;
//		myNetwork.stateSynchronization = NetworkStateSynchronization.Off;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setConnectedPlayers(List<PlayerData> arr){
		m_connectedPlayers = arr;
	}

	//TA BORT DENNA SEN; ENDAST TEST
	public void testLookAtStuff(){
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

	public void startGame(){
		m_winstate.gameStart();
	}

	//save and shut down the game. this happens when time is up
	public void savePlayersAndShutDown(){
		networkView.RPC("stopGameRPC", RPCMode.All);
	}

	[RPC]
	private void stopGameRPC(){
		Application.LoadLevel(m_nextScene);
	}

}
