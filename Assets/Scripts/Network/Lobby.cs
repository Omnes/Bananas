using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {

	//private string m_remoteIP = "127.0.0.1";
	private int m_remotePort = 7777;
	private int m_listenPort = 7777;
	private bool m_useNAT = false;
	//private int yourIP = 0;
	//private int yourPort = 0;
	private int m_maxPlayers = 4;
	public string[] levels = {"test_robin"};


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
			if(GUI.Button(new Rect(110,90,120,30), "Start Server")){
				startServer();
			}
		}
			//started server
		if(Network.peerType == NetworkPeerType.Server){
			GUILayout.Label("Clients: " + Network.connections.Length + "/" + m_maxPlayers);

			if(GUI.Button(new Rect(110, 50, 100, 30), "Start Game")){
				loadLevel();
			}
			
			if(GUI.Button(new Rect(110, 90, 100, 30), "Stop Server")){
				stopServer();
			}
		}else if(Network.peerType == NetworkPeerType.Client){
			//client in serverlobby
		}else{
			//client in lobby
			if(GUI.Button(new Rect(110, 10, 100, 30), "Refresh")){
				MasterServer.RequestHostList("StoryAboutMarvevellousSwaggerLeif");
			}
			
			//
			HostData[] data = MasterServer.PollHostList();
			
			for( int i = 0; i < data.Length; i++){
				if(GUI.Button(new Rect(10, i * 40, 100, 30), data[i].gameName)){
					//connecting to server
					//send networkplayer as patramaeter for cleanup ?
					connectToServer(data[i]);
				}
			}
		}
	}

	//		For starting server on mobile device use 
	//		Network.TestConnnection to check if device can use NAT punchthrough

	//Start sever
	public void startServer(){
		m_useNAT = !Network.HavePublicAddress();
		Network.InitializeServer(m_maxPlayers, m_listenPort, m_useNAT);
		MasterServer.RegisterHost("StoryAboutMarvevellousSwaggerLeif", "Swagger Leif", "This is swagger Leif");
		Debug.Log("lobby.cs : Initializing Server");
	}

	//stop server ? does this function need to remove more stuff ? each player clean up after itself right?
	public void stopServer(){
		disconnect();
		Debug.Log("lobby.cs : Shuting down Server");
	}

	public void connectToServer(HostData e){
		Network.Connect(e);
		Debug.Log("lobby.cs : Connecting Server");
	}

	public void disconnect(){
		Network.Disconnect();
		Debug.Log("lobby.cs : Disconnecting from Server");
		//Network.RemoveRPCs(networkplayer);		get networkplayer
		//
	}

	public void loadLevel(){
		networkView.RPC("loadLevelRPC", RPCMode.All);
	}

	[RPC]
	private void loadLevelRPC(){
		Application.LoadLevel(levels[0]);
	}
}
