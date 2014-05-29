using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MenuBase 
{


	//private string m_remoteIP = "127.0.0.1";
	//private int m_remotePort = 7777;
	private int m_listenPort = 7777;
	private bool m_useNAT = false;


	private int m_maxPlayers = 3; // server doesnt count, maybe?
	public string[] m_levels = {"LemonPark"};

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
	private int m_maxTimeField = 90;

	//scroll startpos
	private Vector2 m_scrollRectPos = new Vector2(100,100);
	public int m_maxGames = 10;

	//
	private Vector2 m_textFieldSize;


	//eh seans nya knapp
	//lobby part 1 knappar
	public List<LobbyButton> m_buttonsPart1 = new List<LobbyButton>();
	//Lobby part 2 knapar
	public List<LobbyButton> m_buttonsPart2 = new List<LobbyButton>();
	//gamelist
	public List<LobbyButton> m_games = new List<LobbyButton>();

	//hoistlist
	public HostData[] m_hostlist;

	//Graphical Properties..
	//Part 1
	private float HostNewGameXpos;
	private float HostNewGameYpos;
	private Vector2 HostNewGameSize;

	private float RefreshXpos;
	private float RefreshYpos;
	private Vector2 RefreshSize;

	private float BackBoardXpos;
	private float BackBoardYpos;
	private Vector2 BackBoardSize;

	private float ServersBackBoardXpos;
	private float ServersBackBoardYpos;
	private Vector2 ServersBackBoardSize;

	private float ScrollXpos;
	private float ScrollYpos;
	private Vector2 ScrollSize;

	private float UsernameFieldXpos;
	private float UsernameFieldYpos;


	//Part2
	private float Part2BackBoardXpos;
	private float Part2BackBoardYpos;
	private Vector2 Part2BackBoardSize;

	private float StartGameXpos;
	private float StartGameYpos;
	private Vector2 StartGameSize;

	private float CancelXpos;
	private float CancelYpos;
	private Vector2 CancelSize;

	// Use this for initialization
	void Start () {
		//menuitems
		m_menuItems = new List<BaseMenuItem> ();
		//add menuitem
		addMenuItem(MenuManager.Instance.getMenuItem(MenuManager.BACK_TO_PREV));
		//lägg till mer knappar'

		screenWidth = Screen.width;
		screenHeight = Screen.height;
		size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenHeight/6;
		
		leftX = centerX - size.x;
		rightX = centerX + size.x;


		//Backboard props..
		BackBoardSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.35f, 0.9f));
		BackBoardXpos = screenWidth / 6.5f;
		BackBoardYpos = screenHeight / 6.2f;

		//Server backboard props..
		ServersBackBoardSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.35f, 0.9f));
		ServersBackBoardXpos = screenWidth / 1.9f;
		ServersBackBoardYpos = screenHeight / 6.2f;

		//Scroll props..
		ScrollSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.35f, 0.9f));
		//fortsätt här snart .. 


		//Textfield props
		m_textFieldSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.25f, 0.1f));
		UsernameFieldXpos = screenWidth / 4.85f;
		UsernameFieldYpos = screenHeight / 1.95f;
		

		//knappar
		//first lobbyapart
		
		//Host new game buttonprops..
		HostNewGameSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.33f, 0.15f));
		HostNewGameXpos = screenWidth / 6f;
		HostNewGameYpos = screenHeight/ 1.45f;
		m_buttonsPart1.Add(new LobbyButton(-100,HostNewGameYpos, HostNewGameSize.x, HostNewGameSize.y, new Rect(0.0f, 0.566f, 0.60f, 0.139f), new Vector2(HostNewGameXpos,HostNewGameYpos), 0.5f, LeanTweenType.easeOutSine));

		//Refreshbtn props
		HostNewGameSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.33f, 0.15f));
//		HostNewGameXpos = screenWidth / 1.5f;
//		HostNewGameYpos = screenHeight/ 1.4f;
		m_buttonsPart1.Add(new LobbyButton(-200,HostNewGameYpos, HostNewGameSize.x, HostNewGameSize.y,	new Rect(0.1f, 0.1f, 0.3f, 0.2f), new Vector2(-4000, HostNewGameYpos), 0.5f, LeanTweenType.easeOutSine));



		//second lobbyPart


		//Backboard part 2
		Part2BackBoardSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.75f, 0.7f));
		Part2BackBoardXpos = screenWidth / 8.5f;
		Part2BackBoardYpos = screenWidth / 12f;

		//StartGameBtn props..
		StartGameSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.4f, 0.12f));
		StartGameXpos = screenWidth / 7f;
		StartGameYpos = screenHeight/ 1.5f;
		m_buttonsPart2.Add(new LobbyButton(-100,StartGameYpos, StartGameSize.x, StartGameSize.y, new Rect(0.0f, 0.427f, 0.7f, 0.138f), new Vector2(StartGameXpos,StartGameYpos), 0.5f, LeanTweenType.easeOutSine));

		//CancelGameBtn props..
		CancelSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.4f, 0.12f));
		CancelXpos = screenWidth / 1.9f;
		CancelYpos = screenHeight/ 1.495f;

		m_buttonsPart2.Add(new LobbyButton(-100,CancelYpos, CancelSize.x, CancelSize.y,	new Rect(0.0f, 0.705f, 0.7f, 0.138f), new Vector2(CancelXpos,CancelYpos), 0.5f, LeanTweenType.easeOutSine));



		MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");

		//check if seanet exist
		if(!SeaNet.isNull()){
			if(SeaNet.Instance.m_connectedPlayers != null){
				m_connectedPlayers = SeaNet.Instance.m_connectedPlayers;
				Network.maxConnections = m_maxPlayers;
				Debug.Log("RESTART");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//From Daniel
	public override void InitMenuItems()
	{
	}


	public override void DoGUI(){


		//Draw background..
		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_backGround);
		//Draw servers background
//		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_backGround);
			//### server not started ###
		if(Network.peerType == NetworkPeerType.Disconnected){
			//Draw backgrounds for first part of lobby..
			GUI.DrawTextureWithTexCoords(new Rect (BackBoardXpos, BackBoardYpos, BackBoardSize.x, BackBoardSize.y), Prefactory.texture_backgrounds, new Rect(0.0f, 0.0f, 0.33f, 0.553f)); 
			GUI.DrawTextureWithTexCoords(new Rect (ServersBackBoardXpos, ServersBackBoardYpos, ServersBackBoardSize.x, ServersBackBoardSize.y), Prefactory.texture_backgrounds, new Rect(0.33f, 0.0f, 0.35f, 0.553f));

			//animation
			for(int i = 0; i < m_buttonsPart1.Count; i++){
				m_buttonsPart1[i].move();
			}

			//start server (server)
			m_tempServerName = GUI.TextField(new Rect(UsernameFieldXpos, UsernameFieldYpos, m_textFieldSize.x, m_textFieldSize.y), m_tempServerName, 25);

//			m_tempPlayerName = GUI.TextField(new Rect(rightX + 30.0f, centerY + size.y, m_textFieldSize.x, m_textFieldSize.y), m_tempPlayerName, 25);

			//  START SERVER BUTTON

			//if(GUI.Button(new Rect(centerX, centerY, size.x, size.y), "Start Server")){
			//start server
			if(m_buttonsPart1[0].isClicked()){

				Debug.Log("Button 0 clicked");
				for(int i = 0; i < m_buttonsPart2.Count; i++){
					m_buttonsPart2[i].resetButton();
				}

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

			// REFRESH BUTTON

			//client in lobby
			if(m_buttonsPart1[1].isClicked()){
				MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");

				m_hostlist = MasterServer.PollHostList();
				m_games.Clear();

				int j = 0;
				for(int i = 0; i < m_hostlist.Length; i++){
					if(m_hostlist[i].connectedPlayers < m_hostlist[i].playerLimit){
						j++;
						string text = m_hostlist[i].gameName + "\n" + (m_hostlist[i].connectedPlayers)+"/"+m_hostlist[i].playerLimit;
						m_games.Add(new LobbyButton(-100,centerY + size.y * j, size.x, size.y, new Rect(0.1f, 0.1f, 0.3f, 0.2f), new Vector2(centerX - size.x,centerY + size.y * j), 3.0f + j, LeanTweenType.easeOutElastic));
					}
				}
			}

			//MasterServer.PollHostList("hej");
			
//			GUILayout.BeginArea(new Rect(leftX, centerY, size.x, size.y * m_maxGames));
//			//GUILayout.FlexibleSpace();
//			GUILayout.BeginVertical();
//			//leftX, centerY + i * size.y, size.x, size.y
//			GUILayout.MinHeight(size.y);
			
			//m_scrollRectPos = GUILayout.BeginScrollView(m_scrollRectPos, GUILayout.Width(size.x), GUILayout.Height(screenHeight));

			if(m_hostlist != null){
				for( int i = 0; i < m_games.Count; i++){
					m_games[i].move();

					if(m_games[i].isClicked()){
						
						if(m_tempPlayerName.Length == 0){
							m_tempPlayerName = "NOOB";
						}
						//connecting to server
						//send networkplayer as patramaeter for cleanup ?
						connectToServer(m_hostlist[i]);
						Debug.Log("NETWORKID: "+ Network.player.guid);
						
					}
				}
			}
//			
//			GUILayout.EndScrollView();
//			
//			GUILayout.EndVertical();
//			GUILayout.EndArea();

		}

		// START GAME BUTTON

			//started server
		if(Network.peerType == NetworkPeerType.Server){

			GUI.DrawTextureWithTexCoords(new Rect(Part2BackBoardXpos, Part2BackBoardYpos, Part2BackBoardSize.x, Part2BackBoardSize.y), 
			                             Prefactory.texture_backgrounds, new Rect(0.0f, 0.555f, 0.7f, 0.5f));
			//animation
			for(int i = 0; i < m_buttonsPart2.Count; i++){
				m_buttonsPart2[i].move();
			}

			//start game
			if(m_buttonsPart2[0].isClicked()){
				startGame();
			}

		// STOP GAME BUTTON

			if(m_buttonsPart2[1].isClicked()){
				stopServer();

				for(int i = 0; i < m_buttonsPart1.Count; i++){
					m_buttonsPart1[i].resetButton();
				}
			}

			string tempMax = GUI.TextField(new Rect(centerX, centerY + (size.y * 3), size.x, size.y), m_maxTimeField.ToString(), 25);
			m_maxTimeField = int.Parse(tempMax);

		}

		for(int i = 0; i < m_connectedPlayers.Count; i++){
			GUILayout.Label("Client Name: "+ m_connectedPlayers[i].m_name+" Client GUID"+m_connectedPlayers[i].m_guid);
		}
//		GUILayout.Label("ListSize (players): "+m_connectedPlayers.Count);

	}

	public void startGame(){
		//set maxtime
		SeaNet.Instance.setMaxTime(m_maxTimeField);
		
		//create ID for allplayers
		createId();
		//loads next level
		Network.maxConnections = -1;
		//denna fungerar inte
		MasterServer.UnregisterHost();
		SeaNet.Instance.loadLevel(m_levels[0]);

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


}
