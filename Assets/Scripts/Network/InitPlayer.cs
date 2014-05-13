using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitPlayer : MonoBehaviour {

	public GameObject m_playerPrefab;
	public GameObject m_ghostPrefab;

	public GameObject m_stateTransmitterPrefab;

	private GameObject[] m_spawnpoints = new GameObject[4];
	

	private PlayerInfo m_playerInfo;
	private GameObject m_player;

	public bool m_isLocal = false;

	//public bool m_debug_is_Server = false;
	//public bool m_debug_is_Client = false;
	//public int m_debug_id = 0;

	public InputMetod m_localInput;

	private List<PlayerData> m_playerDataList = new List<PlayerData>();

	// Use this for initialization
	void Start () {
		//setPlayerInfo(new PlayerInfo("Default",m_debug_id)); //temp

		//used to find out wich mesh is the correct one
//		m_playerDataList = SeaNet.Instance.m_connectedPlayers;
//		for(int i = 0; i < m_playerDataList.Count; i++){
//			if(m_playerDataList[i].m_id == m_playerInfo.id){
//				//m_playerDataList[i].m_characterMesh;
	//			GameObject tempMesh = (GameObject)Instantiate(Prefactory.prefab_douglas);
	//			tempMesh.transform.parent = gameObject.transform;
//			}
//		}
	}
	
	public void init(){
		gameObject.name = "PlayerController: " + m_playerInfo.name;
		m_spawnpoints = sortSpawnpoints(GameObject.FindGameObjectsWithTag("Spawnpoint"));
		Transform spawnpoint = m_spawnpoints[m_playerInfo.id].transform;
		
		if(Network.isServer){
			//om vi är server skapa en faktisk spelare
			m_player = Instantiate(m_playerPrefab,spawnpoint.position,spawnpoint.rotation) as GameObject;
			m_player.GetComponent<SyncMovement>().setID(m_playerInfo.id,m_isLocal);

			//set correct mesh
			setMesh(m_player);

			//setup the StateTransmitter for this player
			GameObject transmitter = Network.Instantiate(m_stateTransmitterPrefab,Vector3.zero,Quaternion.identity,0) as GameObject;
			transmitter.networkView.RPC("linkToID",RPCMode.All,m_playerInfo.id);

			InputHub hub = m_player.GetComponent<InputHub>();
			//är det vår spelare? ta input från den lokala klienten och sätt kameran efter
			if(m_isLocal){
				hub.setInputMetod(m_localInput);
				setCameraTarget(m_player.transform);
				m_player.name = "Player " + m_playerInfo.name + " (Local)";
			}else{
				//ananrs ta remote inputen
				hub.setInputMetod(GetComponent<RemoteInput>());
				m_player.name = "Player " + m_playerInfo.name + " (Remote)";
			}
		}
		else if(Network.isClient){
			//om vi är en klient skapa en fake spelare
			m_player = Instantiate(m_ghostPrefab,spawnpoint.position,spawnpoint.rotation) as GameObject;
			m_player.GetComponent<SyncMovement>().setID(m_playerInfo.id,m_isLocal);

			//set correct mesh
			setMesh(m_player);

			if(m_isLocal){
				m_player.name = "PlayerGhost " + m_playerInfo.name+ " (Local)";
				//är det vår fake spalare? få kameran att följa
				setCameraTarget(m_player.transform);
			}else{
				m_player.name = "PlayerGhost " + m_playerInfo.name+ " (Remote)";
			}
		}
	}

	public void setLocal(bool local){
		m_isLocal = local;
	}

	public void setLocalInputMetod(InputMetod input){
		m_localInput = input;
	}

	public void setPlayerInfo(PlayerInfo info){
		m_playerInfo = info;
	}

	void setCameraTarget(Transform target){
		Camera.main.GetComponent<CameraFollow>().SetTarget(target);
	}

	[RPC]
	public void RPCInitController(string name,int id){
		setPlayerInfo(new PlayerInfo(name,id));
		init ();
	}

	private GameObject[] sortSpawnpoints(GameObject[] array){
		int length = array.Length;
		for(int i = 0; i < length;i++){
			for(int j = 0; j < length;j++){
				if(array[i].GetComponent<SpawnpointGizmo>().m_id < array[j].GetComponent<SpawnpointGizmo>().m_id){
					GameObject temp = array[i];
					array[i] = array[j];
					array[j] = temp;
				}
			}
		}
		return array;
	}

	public void setMesh(GameObject player){
		//set correct mesh
		GameObject tempMesh = (GameObject)Instantiate(Prefactory.prefab_meshList[m_playerInfo.id], player.transform.position, player.transform.rotation);
		player.GetComponent<upperBodyAnimation>().setAnimator(tempMesh.GetComponent<Animator>());
		tempMesh.transform.parent = player.transform;
	}

}
