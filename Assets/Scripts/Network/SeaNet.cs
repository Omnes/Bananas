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
	public NetworkView m_networkView;


	//dessa två ska tas bort. ### ANVÄND EJ I SERIÖS SYFTE ###
	public List<int> m_IDs;
	public List<bool> m_isLocal;
	public List<string> m_names;

	public static bool isNull(){
		return instance == null;
	}

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);

		m_winstate = gameObject.AddComponent<Winstate>();

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

	public void startGame(){
		m_winstate.gameStart();
	}

//	public void stopGame(){
//		Application.LoadLevel(m_nextScene);
//	}

	//save and shut down the game. this happens when time is up
	public void savePlayersAndShutDown(){
		networkView.RPC("stopGameRPC", RPCMode.All);
	}

	[RPC]
	private void stopGameRPC(){
		Application.LoadLevel(m_nextScene);
	}

}
