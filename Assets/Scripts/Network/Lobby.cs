using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour {

	//private string m_remoteIP = "127.0.0.1";
	//private int m_remotePort = 7777;
	private int m_listenPort = 7777;
	private bool m_useNAT = false;

	private int m_maxPlayers = 4; // server doesnt count, maybe?
	public string[] m_levels = {"test_johannes"};

	//might not be use3d
	//private string m_myIP = "";
	//private int m_myPort = 0;


//	private int m_playerCount = 0;
	public List<PlayerData> m_connectedPlayers = new List<PlayerData>();

	//local playerdata
	public PlayerData m_myPlayerData;


	//name of user
	private string m_tempPlayerName = "";
	private string m_tempServerName = "";

	//maxtime field
	private int m_maxTimeField = 600;

	//scroll startpos
	private Vector2 m_scrollRectPos = new Vector2(100,100);
	public int m_maxGames = 10;

	// Use this for initialization
	void Start () {
		MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");

		//check if seanet exist
		if(!SeaNet.isNull()){
			m_connectedPlayers = SeaNet.Instance.m_connectedPlayers;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		int scrWidth = Screen.width;
		int scrHeight = Screen.height;
		Vector2 size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
		Vector2 textFieldSize = GUIMath.InchToPixels(new Vector2(2f, 0.6f));

		float centerX = scrWidth/2 - (size.x / 2);
		float centerY = scrHeight/6;

		float leftX = centerX - size.x;
		float rightX = centerX + size.x;



			//### server not started ###
		if(Network.peerType == NetworkPeerType.Disconnected){

			//start server (server)
			m_tempServerName = GUI.TextField(new Rect(rightX, centerY, textFieldSize.x, textFieldSize.y), m_tempServerName, 25);
			m_tempPlayerName = GUI.TextField(new Rect(rightX, centerY + size.y, textFieldSize.x, textFieldSize.y), m_tempPlayerName, 25);

			if(GUI.Button(new Rect(centerX, centerY, size.x, size.y), "Start Server")){
				if(m_tempPlayerName.Length == 0){
					m_tempPlayerName = "NOOB";
				}
				if(m_tempServerName.Length == 0){
					m_tempServerName = "NOOBS ONLY";
				}
				startServer(m_tempServerName);
				//add ip for player who started server
				m_myPlayerData = new PlayerData(m_tempPlayerName, Network.player.guid);
				m_myPlayerData.local = true;
				addPlayerToClientList(m_myPlayerData);
			}
		}
			//started server
		if(Network.peerType == NetworkPeerType.Server){
			if(GUI.Button(new Rect(centerX, centerY, size.x, size.y), "Start Game")){
				//set maxtime
				SeaNet.Instance.setMaxTime(m_maxTimeField);

				//create ID for allplayers
				createId();
				//loads next level
				Network.maxConnections = -1;
				//denna fungerar inte
				MasterServer.UnregisterHost();
				loadLevel();
			}
			if(GUI.Button(new Rect(centerX, centerY + size.y, size.x, size.y), "Stop Server")){
				m_tempServerName = "";
				stopServer();
			}
			if(GUI.Button(new Rect(centerX, centerY + (size.y * 2), size.x, size.y), "SEE STUFF")){
				networkView.RPC("showStuff", RPCMode.All);
			}

			string tempMax = GUI.TextField(new Rect(centerX, centerY + (size.y * 3), size.x, size.y), m_maxTimeField.ToString(), 25);
			m_maxTimeField = int.Parse(tempMax);

		}/*else if(Network.peerType == NetworkPeerType.Client){
			//client in serverlobby
		}*/else{
			//client in lobby
			if(GUI.Button(new Rect(centerX, centerY + size.y, size.x, size.y), "Refresh")){
				MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");
			}
			
			//
			HostData[] data = MasterServer.PollHostList();
			//MasterServer.PollHostList("hej");

			GUILayout.BeginArea(new Rect(leftX, centerY, size.x, size.y * m_maxGames));
			//GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			//leftX, centerY + i * size.y, size.x, size.y
			GUILayout.MinHeight(size.y);

			m_scrollRectPos = GUILayout.BeginScrollView(m_scrollRectPos, GUILayout.Width(size.x), GUILayout.Height(scrHeight));

				for( int i = 0; i < data.Length; i++){
					if(GUILayout.Button(data[i].gameName, GUILayout.MinHeight(size.y))){

							if(m_tempPlayerName.Length == 0){
								m_tempPlayerName = "NOOB";
							}
							//connecting to server
							//send networkplayer as patramaeter for cleanup ?
							connectToServer(data[i]);
							Debug.Log("NETWORKID: "+ Network.player.guid);

					}
				}


			GUILayout.EndScrollView();

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		for(int i = 0; i < m_connectedPlayers.Count; i++){
			GUILayout.Label("Client Name: "+ m_connectedPlayers[i].m_name+" Client GUID"+m_connectedPlayers[i].m_guid);
		}
		GUILayout.Label("ListSize (players): "+m_connectedPlayers.Count);
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
		networkView.RPC("RemoveAllRPC", RPCMode.Others);
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

	//lägger till spelare så den kan följa med till nästa scen
	public void addPlayerToClientList(PlayerData p){
		m_connectedPlayers.Add(p);
		SeaNet.Instance.setConnectedPlayers(m_connectedPlayers);
	}

	//create id for all players on server
	public void createId(){
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			m_connectedPlayers[i].m_id = i;

			string guid = m_connectedPlayers[i].m_guid;
			networkView.RPC("createIdRPC", RPCMode.Others, guid, i);
		}
	}

	//	void OnPlayerConnected(NetworkPlayer player) {
	//		Debug.Log("Player " + m_playerCount + " connected from " + player.ipAddress + ":" + player.port);
	//	}

	//on serverside if client disconnected
	void OnPlayerDisconnected(NetworkPlayer player){
		// FIX SO TAHTA m_connectedPlayers have a networkPlayer to know which to remove
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			string guid = m_connectedPlayers[i].m_guid;

			if(guid == player.guid){
				Debug.Log("Removing : "+m_connectedPlayers[i].m_name);
				networkView.RPC("removePlayerRPC", RPCMode.Others, guid);
				m_connectedPlayers.RemoveAt(i);
			}
		}
		Debug.Log("connection was lost");
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

//	//tar bort alla spelare från lobbyn
//	public void removeAllPlayers(){
//
//	}
//	//tar bort en spelare från lobbyn
//	public void removePlayer(){
//
//	}

	void OnConnectedToServer(){
		//send playerdata to server and alla others
		string name = m_tempPlayerName;
		Debug.Log("sänder till server");
		networkView.RPC("playerNameRPC", RPCMode.Server, name, Network.player);
	}



	// ###			CLIENT			###

	//add new player
	[RPC]
	private void playerDataRPC(string name, string localGuid){
		PlayerData tempPlayerData = new PlayerData(name, localGuid);

		//set if player is local or not
		if(localGuid == Network.player.guid){
			tempPlayerData.local = true;
		}else{
			tempPlayerData.local = false;
		}
	
		//add player in list
		addPlayerToClientList(tempPlayerData);
	}

	//recieve list and add players
	[RPC]
	private void addPlayerListRPC(string name, string guid){ 
		Debug.Log("client networkplayer: "+guid);
		PlayerData tempPlayerData = new PlayerData(name, guid);
		addPlayerToClientList(tempPlayerData);
	}

	[RPC]
	private void RemoveAllRPC(){
		m_connectedPlayers.Clear();
	}

	// ###			SERVER			###

	[RPC]
	private void loadLevelRPC(){
		Application.LoadLevel(m_levels[0]);
	}

	//set name and if it's local
	//THIS IS ONLY DONE ON SERVERSIDE
	[RPC]
	private void playerNameRPC(string name, NetworkPlayer target){
		//add to server list
		PlayerData tempPlayerData = new PlayerData(name, target.guid);
		addPlayerToClientList(tempPlayerData);

		//skcika lista på alla tidigare spelare och om den är local eller inte 
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			if(target.guid != m_connectedPlayers[i].m_guid){
				networkView.RPC("addPlayerListRPC", target, m_connectedPlayers[i].m_name, m_connectedPlayers[i].m_guid); // NETWORKPLAYER KAN INTE SKICKAS TILL CLIENT, skickar guid ist
			}
		}

		//send RPC to all Others
		networkView.RPC("playerDataRPC", RPCMode.Others, name, target.guid);
	}

	[RPC]
	private void createIdRPC(string guid, int id){
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			string tempGuid = m_connectedPlayers[i].m_guid;
			if(tempGuid == guid){
				m_connectedPlayers[i].m_id = id;
			}else{
				Debug.Log("client listLength: "+m_connectedPlayers.Count);
			}
		}
	}

	[RPC]
	private void removePlayerRPC(string guid){
		for(int i = 0; i < m_connectedPlayers.Count; i++){
			string tempGuid = m_connectedPlayers[i].m_guid;
			if(tempGuid == guid){
				Debug.Log("Removing: "+m_connectedPlayers[i].m_name);
				m_connectedPlayers.RemoveAt(i);
			}
		}
	}

	//###		REMOVE THIS LATER ONLY TEST (fråga sean obv)		####
	[RPC]
	private void showStuff(){
		SeaNet.Instance.testLookAtStuff();
	}

}
