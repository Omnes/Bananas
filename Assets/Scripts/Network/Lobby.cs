using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MenuBase 
{
	private int m_listenPort = 7777;
	private bool m_useNAT = false;
	
	private int m_maxPlayers = 3; // server doesnt count, maybe?
	public string[] m_levels = {"LemonPark"};
	
	public List<PlayerData> m_connectedPlayers = new List<PlayerData>();
	//local playerdata
	public PlayerData m_myPlayerData;
	
	//name of user
	private static string m_tempPlayerName = "Anonymous";
	private string m_tempIP = "";

	//maxtime field
	private int m_maxTimeField = 90;

	private GUIStyle myGuiStyle;
//	public string m_masterServerIp = "193.10.184.20";

	//knappar
	public List<LobbyButton> m_buttonsPart1 = new List<LobbyButton>();
	public List<LobbyButton> m_buttonsPart2 = new List<LobbyButton>();
	public List<LobbyButton> m_games = new List<LobbyButton>();
	private LobbyButton m_muteButton;
	private LobbyButton m_refreshButton;
	private LobbyButton m_directConnectButton;
	
	//hostlist
	public HostData[] m_hostlist;
	private float m_lastServerRefresh;
	public float m_ServerRefreshRate = 15f; 
	
	public GUIStyle m_guiStyle;
	private ServerList m_serverList;

	//Graphical Properties..
	//Part 1
	private Vector2 m_textFieldSize;
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
	private float Part2BackBoardXpos;
	private float Part2BackBoardYpos;
	private Vector2 Part2BackBoardSize;
	private float StartGameXpos;
	private float StartGameYpos;
	private Vector2 StartGameSize;
	private float CancelXpos;
	private float CancelYpos;
	private Vector2 CancelSize;
	
	public float  m_serverListYSize = 0.6f;

	public float  ServerListYSize = 0.6f;

	public GUIStyle m_typeNameStyle;
	public Font fontLobby;

	public GUIStyle m_usenameStyle;


	// Use this for initialization
	void Start () {
		//playernames
		myGuiStyle = new GUIStyle();
		myGuiStyle.alignment = TextAnchor.MiddleCenter;
		myGuiStyle.font = fontLobby;//(Font)Resources.Load("Textures/Fonts/FluxArchitectRegular");
		myGuiStyle.fontSize = Screen.height / 20 ;
		
		m_typeNameStyle.alignment = TextAnchor.MiddleCenter;
		m_typeNameStyle.font = fontLobby;//(Font)Resources.Load("Textures/Fonts/FluxArchitectRegular");
		m_typeNameStyle.fontSize = Screen.height / 20;

		m_usenameStyle.alignment = TextAnchor.MiddleCenter;
		m_usenameStyle.font = fontLobby;//(Font)Resources.Load("Textures/Fonts/FluxArchitectRegular");
		m_usenameStyle.fontSize = Screen.height / 20;

		initMenuScales(); //moved out the all the menu scaling to a function at the bottom of the script //robin

		//check if seanet exist
		if(!SeaNet.isNull()){
			if(SeaNet.Instance.m_connectedPlayers != null){
				m_connectedPlayers = SeaNet.Instance.m_connectedPlayers;
				Network.maxConnections = m_maxPlayers;
				Debug.Log("RESTART");
			}
		}

//		MasterServer.ipAddress = "195.198.115.142";
//		Network.natFacilitatorIP = "195.198.115.142";
//		Network.natFacilitatorPort = 50005;
	}

	//gets called from menumanager on switch to this
	public override void InitMenuItems()
	{
		RefreshHostList();
		m_lastServerRefresh = 0f;
	}

	private void RefreshHostList(){
		MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");
		HostData[] hostlist = MasterServer.PollHostList();
		m_serverList.SetHostList(hostlist);
	}

	public override void DoGUI(){
		//Draw background..
		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_backGround);
			//### server not started ###
		if(Network.peerType == NetworkPeerType.Disconnected){
			//Draw backgrounds for first part of lobby..
			GUI.DrawTextureWithTexCoords(new Rect (BackBoardXpos, BackBoardYpos, BackBoardSize.x, BackBoardSize.y), Prefactory.texture_backgrounds, new Rect(0.0f, 0.0f, 0.33f, 0.553f)); 
		
			//animation
			for(int i = 0; i < m_buttonsPart1.Count; i++){
				m_buttonsPart1[i].move();
			}

			//start server (server)

			//username ypos
			float ypos = UsernameFieldYpos - m_textFieldSize.y + 20;

			GUI.Label(new Rect(UsernameFieldXpos, ypos, m_textFieldSize.x, m_textFieldSize.y), "Username", m_usenameStyle);

			m_tempPlayerName = GUI.TextField(new Rect(UsernameFieldXpos, UsernameFieldYpos, m_textFieldSize.x, m_textFieldSize.y), m_tempPlayerName, 10, m_typeNameStyle);

			//start server
			if(m_buttonsPart1[0].isClicked()){

//				Debug.Log("Button 0 clicked");
				for(int i = 0; i < m_buttonsPart2.Count; i++){
					m_buttonsPart2[i].resetButton();
				}

				if(m_tempPlayerName.Equals("")){
					m_tempPlayerName = "Anonymous";
				}

				startServer(m_tempPlayerName + "'s game");
				//add ip for player who started server
				m_myPlayerData = new PlayerData(m_tempPlayerName, Network.player.guid);
				m_myPlayerData.local = true;
				addPlayerToClientList(m_myPlayerData);
			}


			GUIStyle ipStyle = new GUIStyle();
			ipStyle.alignment = TextAnchor.MiddleRight;
			ipStyle.font = (Font)Resources.Load("Textures/Fonts/ARLRDBD");
			ipStyle.fontSize = Screen.height / 30;


			GUI.Label(new Rect(0,Screen.height - m_textFieldSize.y/2,BackBoardXpos,m_textFieldSize.y/2),"IP:",ipStyle);
			m_tempIP = GUI.TextField(new Rect(BackBoardXpos, Screen.height - m_textFieldSize.y/2, m_textFieldSize.x, m_textFieldSize.y/2), m_tempIP, 50);

			if(m_directConnectButton.isClicked()){
				
				Network.Connect(m_tempIP,m_listenPort);
			}

			if(m_refreshButton.isClicked()){
				RefreshHostList();
			}
		
			m_serverList.Draw();
		}

		//started server
		if(Network.peerType == NetworkPeerType.Server){

			GUI.DrawTextureWithTexCoords(new Rect(Part2BackBoardXpos, Part2BackBoardYpos, Part2BackBoardSize.x, Part2BackBoardSize.y), 
			                             Prefactory.texture_backgrounds, GUIMath.CalcTexCordsFromPixelRect(new Rect(0,0,719,457)));

			float buttonWidth = (Part2BackBoardSize.x / 2);
			float buttonHeight = Part2BackBoardSize.y / 5;
			
			for(int i = 0; i < 4; i++){
				Rect uvRect = new Rect(0.7041f,1f - 0.2363f, 0.2929f,0.0859f);
				Rect PosAndSize = new Rect(Part2BackBoardXpos + i%2 * buttonWidth , ((Part2BackBoardSize.y/2) - buttonHeight)  + ((int)i/2) * (buttonHeight + 10), buttonWidth, buttonHeight);
				GUI.DrawTextureWithTexCoords(PosAndSize, Prefactory.texture_backgrounds, uvRect);
			}
			
			for(int i = 0; i < m_connectedPlayers.Count; i++){
				GUI.Label(new Rect(Part2BackBoardXpos + i%2 * buttonWidth, ((Part2BackBoardSize.y/2) - buttonHeight)  + ((int)i/2) * (buttonHeight + 10), buttonWidth, buttonHeight), m_connectedPlayers[i].m_name, myGuiStyle);
			}

			GUI.Label(new Rect(0,Part2BackBoardYpos,Screen.width,100),Network.player.ipAddress,myGuiStyle);

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
	
		}

		//joined as client
		if(Network.peerType == NetworkPeerType.Client){
			GUI.DrawTextureWithTexCoords(new Rect(Part2BackBoardXpos, Part2BackBoardYpos, Part2BackBoardSize.x, Part2BackBoardSize.y), 
			                             Prefactory.texture_backgrounds, GUIMath.CalcTexCordsFromPixelRect(new Rect(0,0,719,457)));

			float buttonWidth = (Part2BackBoardSize.x / 2);
			float buttonHeight = Part2BackBoardSize.y / 5;

			for(int i = 0; i < 4; i++){
				Rect uvRect = new Rect(0.7041f,1f - 0.2363f, 0.2929f,0.0859f);
				Rect PosAndSize = new Rect(Part2BackBoardXpos + i%2 * buttonWidth , ((Part2BackBoardSize.y/2) - buttonHeight)  + ((int)i/2) * (buttonHeight + 10), buttonWidth, buttonHeight);
				GUI.DrawTextureWithTexCoords(PosAndSize, Prefactory.texture_backgrounds, uvRect);
			}

			for(int i = 0; i < m_connectedPlayers.Count; i++){
				GUI.Label(new Rect(Part2BackBoardXpos + i%2 * buttonWidth, ((Part2BackBoardSize.y/2) - buttonHeight)  + ((int)i/2) * (buttonHeight + 10), buttonWidth, buttonHeight), m_connectedPlayers[i].m_name, myGuiStyle);
			}

		}

		Rect texCordsMute = GUIMath.CalcTexCordsFromPixelRect(new Rect(294,0,158,158));
		Rect texCordsUnmute = GUIMath.CalcTexCordsFromPixelRect(new Rect(451,0,158,158));
		
		if(this.m_muteButton.isClicked()){
			SoundManager.Instance.ToggleMute();
			Rect texCord = SoundManager.Instance.m_paused ? texCordsUnmute : texCordsMute;
			m_muteButton.changeUVrect(texCord);
		}


	}

	public override void DoUpdate (){
		if(Network.peerType == NetworkPeerType.Disconnected){
			if(Time.time > m_lastServerRefresh + m_ServerRefreshRate){
				RefreshHostList();
				m_lastServerRefresh = Time.time;
			}
			m_serverList.update();
			//scroll
	//		base.DoUpdate ();
		}
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

	public static void connectToServer(HostData e){
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


	private void initMenuScales(){
		
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
		ServersBackBoardSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.35f, m_serverListYSize));
		ServersBackBoardXpos = screenWidth / 1.9f;
		ServersBackBoardYpos = screenHeight /6.2f;
		
		m_serverList = new ServerList(new Rect(ServersBackBoardXpos,ServersBackBoardYpos,ServersBackBoardSize.x,ServersBackBoardSize.y));
		
		//Scroll props..
		ScrollSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.35f, 0.9f));

		//Textfield props
		m_textFieldSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.25f, 0.1f));
		UsernameFieldXpos = screenWidth / 4.85f;
		UsernameFieldYpos = screenHeight / 1.95f;

		//Host new game buttonprops..
		HostNewGameSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.33f, 0.15f));
		HostNewGameXpos = screenWidth / 6f;
		HostNewGameYpos = screenHeight/ 1.45f;
		m_buttonsPart1.Add(new LobbyButton(-100,HostNewGameYpos, HostNewGameSize.x, HostNewGameSize.y, new Rect(0.0f, 0.566f, 0.60f, 0.139f),
		                                   new Vector2(HostNewGameXpos,HostNewGameYpos), 0.5f, LeanTweenType.easeOutSine));

		//second lobbyPart
		//Backboard part 2
		Part2BackBoardSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.75f, 0.7f));
		Part2BackBoardXpos = screenWidth / 8.5f;
		Part2BackBoardYpos = screenWidth / 12f;
		
		//StartGameBtn props..
		StartGameSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.28f, 0.12f));
		StartGameXpos = screenWidth / 7f;
		StartGameYpos = screenHeight/ 1.5f;
		m_buttonsPart2.Add(new LobbyButton(-100,StartGameYpos, StartGameSize.x, StartGameSize.y, GUIMath.CalcTexCordsFromPixelRect(new Rect(0,445,571,143)), new Vector2(StartGameXpos,StartGameYpos), 0.5f, LeanTweenType.easeOutSine));
		
		//CancelGameBtn props..
		CancelSize = GUIMath.SmallestOfInchAndPercent(new Vector2(3000.0f, 1000.0f), new Vector2(0.28f, 0.12f));
		CancelXpos = screenWidth / 1.9f;
		CancelYpos = screenHeight/ 1.495f;

		m_buttonsPart2.Add(new LobbyButton(-100,CancelYpos, CancelSize.x, CancelSize.y,	GUIMath.CalcTexCordsFromPixelRect(new Rect(0,158,571,143)), new Vector2(CancelXpos,CancelYpos), 0.5f, LeanTweenType.easeOutSine));

		Vector2 refreshSize = new Vector2(ServersBackBoardSize.x*0.2f,ServersBackBoardSize.x*0.2f);
		Rect refreshTexCords = GUIMath.CalcTexCordsFromPixelRect(new Rect(624,158,158,143));
		Rect refreshPos = new Rect(ServersBackBoardXpos + ServersBackBoardSize.x*0.5f - refreshSize.x*0.5f,ServersBackBoardYpos + ServersBackBoardSize.y,refreshSize.x,refreshSize.y );
		m_refreshButton = new LobbyButton(refreshPos,refreshTexCords);

		Vector2 directConnectSize = new Vector2(ServersBackBoardSize.x*0.18f,m_textFieldSize.y/2);
		Rect directConnectTexCords = GUIMath.CalcTexCordsFromPixelRect(new Rect(577,835,350,190),1024);
		Rect directConnectPos = new Rect(BackBoardXpos + m_textFieldSize.x, Screen.height - directConnectSize.y,directConnectSize.x,directConnectSize.y );
		m_directConnectButton = new LobbyButton(directConnectPos,directConnectTexCords);

		float PADDING = 5f;
		Vector2 muteSize = GUIMath.SmallestOfInchAndPercent(new Vector2(0.5f,0.5f),new Vector2(0.09f,0.09f));
		m_muteButton = new LobbyButton(new Rect(Screen.width - (muteSize.x + PADDING), PADDING, muteSize.x, muteSize.y),GUIMath.CalcTexCordsFromPixelRect(new Rect(294,0,158,158)));
	}

	public static void setName(){
		if(m_tempPlayerName.Equals("")){
			m_tempPlayerName = "Anonymous";
		}
	}


}
