using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {

	//private string m_remoteIP = "127.0.0.1";
	//private int m_remotePort = 7777;
	private int m_listenPort = 7777;
	private bool m_useNAT = false;

	private int m_maxPlayers = 4;
	public string[] m_levels = {"test_robin"};

	//might not be use3d
	//private string m_myIP = "";
	//private int m_myPort = 0;


	private int m_playerCount = 1;
	public List<PlayerData> m_connectedPlayers = new List<PlayerData>();

	//local playerdata
	public PlayerData m_myPlayerData;


	//name of user
	private string tempName = "TEMPNAME";
	private string serverName = "TEMPNAME";

	// Use this for initialization
	void Start () {
		MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");

	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void OnGUI(){
			//### server not started ###
		if(Network.peerType == NetworkPeerType.Disconnected){
			//start server (server)
			serverName = GUI.TextField(new Rect(420, 140, 200, 60), serverName, 25 );
			if(GUI.Button(new Rect(210,140,200,60), "Start Server")){
				startServer(serverName);
				//add ip for player who started server
				m_myPlayerData = new PlayerData(tempName, 0);
				m_myPlayerData.local = true;
				addPlayerToClientList(m_myPlayerData);
			}
			tempName = GUI.TextField(new Rect(420, 10, 200, 60), tempName, 25 );
		}
			//started server
		if(Network.peerType == NetworkPeerType.Server){
			if(GUI.Button(new Rect(210, 70, 200, 60), "Start Game")){
				loadLevel();
			}
			if(GUI.Button(new Rect(210, 140, 200, 60), "Stop Server")){
				stopServer();
			}
			if(GUI.Button(new Rect(210, 210, 200, 60), "SEE STUFF")){
				networkView.RPC("showStuff", RPCMode.All);
			}

		}/*else if(Network.peerType == NetworkPeerType.Client){
			//client in serverlobby
		}*/else{
			//client in lobby
			if(GUI.Button(new Rect(210, 10, 200, 60), "Refresh")){
				MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");
			}
			
			//
			HostData[] data = MasterServer.PollHostList();
			//MasterServer.PollHostList("hej");
			
			for( int i = 0; i < data.Length; i++){
				if(GUI.Button(new Rect(10, i * 60 + 10, 200, 60), data[i].gameName)){
					//connecting to server
					//send networkplayer as patramaeter for cleanup ?
					connectToServer(data[i]);
					Debug.Log("NETWORKID: "+ Network.player.guid);
				}
			}
		}

		foreach(PlayerData e in m_connectedPlayers){
			GUILayout.Label("Client Name: "+ e.m_name +" Client Id: " + e.m_id);
		}
	}

	//		For starting server on mobile device use 
	//		Network.TestConnnection to check if device can use NAT punchthrough

	//Start sever
	public void startServer(string name){
		m_useNAT = !Network.HavePublicAddress();
		Network.InitializeServer(m_maxPlayers, m_listenPort, m_useNAT);
		MasterServer.RegisterHost("StoryAboutMarvevellousSwaggerLeif", name, "This is swagger Leif");
		//Debug.Log("lobby.cs : Initializing Server");
	}

	public void stopServer(){
		//remove all players
		m_connectedPlayers.Clear();
		SeaNet.Instance.setConnectedPlayers(m_connectedPlayers);
		Network.Disconnect();
		Debug.Log("lobby.cs : Shuting down Server");
	}

	public void connectToServer(HostData e){
		Network.Connect(e);
		Debug.Log("lobby.cs : Connecting Server");
	}

	public void disconnect(){
		//remove specific player
		//flytta ner denna till removePlayer func när den fungerar
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			if(m_connectedPlayers[i].Equals(m_myPlayerData)){
				Debug.Log("Lobby.cs : Removing Player: "+m_connectedPlayers[i].m_name);
				m_connectedPlayers.RemoveAt(i);
			}
		}

		Network.Disconnect();
		Debug.Log("lobby.cs : Disconnecting from Server");
		//Network.RemoveRPCs(networkplayer);		get networkplayer
		//
	}

	public void loadLevel(){
		networkView.RPC("loadLevelRPC", RPCMode.All);
	}

//	void OnPlayerConnected(NetworkPlayer player) {
//		Debug.Log("Player " + m_playerCount + " connected from " + player.ipAddress + ":" + player.port);
//	}

	//tar bort alla spelare från lobbyn
	public void removeAllPlayers(){

	}
	//tar bort en spelare från lobbyn
	public void removePlayer(){

	}

	void OnConnectedToServer(){
		//send playerdata to server and alla others
		string name = tempName;
		networkView.RPC("playerNameRPC", RPCMode.Server, name, Network.player);
	}

	//lägger till spelare så den kan följa med till nästa scen
	void addPlayerToClientList(PlayerData p){
		m_connectedPlayers.Add(p);
		SeaNet.Instance.setConnectedPlayers(m_connectedPlayers);
	}


	// ###			CLIENT			###

	//add new player
	[RPC]
	private void playerDataRPC(string name, int id, NetworkPlayer localPlayer){
		PlayerData tempPlayerData = new PlayerData(name, id);

		//set if player is local or not
		if(localPlayer == Network.player){
			tempPlayerData.local = true;
		}else{
			tempPlayerData.local = false;
		}

		//add player in list
		addPlayerToClientList(tempPlayerData);
	}

	//recieve list and add players
	[RPC]
	private void addPlayerListRPC(string name, int id){
		PlayerData tempPlayerData = new PlayerData(name, id);
		addPlayerToClientList(tempPlayerData);
	}

	// ###			SERVER			###

	[RPC]
	private void loadLevelRPC(){
		Application.LoadLevel(m_levels[0]);
	}

	//set name and if it's local
	[RPC]
	private void playerNameRPC(string name, NetworkPlayer target){
		int id = m_playerCount++;
		PlayerData tempPlayerData = new PlayerData(name, id);
		addPlayerToClientList(tempPlayerData);
		
		//skcika lista på alla tidigare spelare och om den är local eller inte 
		
		foreach(PlayerData pd in m_connectedPlayers){
			if(tempPlayerData != pd){
				networkView.RPC("addPlayerListRPC", target, pd.m_name, pd.m_id);
			}
		}
		
		//send RPC to all Others
		networkView.RPC("playerDataRPC", RPCMode.Others, name, id, target);
	}


	//###		REMOVE THIS LATER ONLY TEST (fråga sean obv)		####
	[RPC]
	private void showStuff(){
		SeaNet.Instance.testLookAtStuff();
	}

}
