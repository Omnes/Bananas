using UnityEngine;
using System.Collections;

public class InitPlayer : MonoBehaviour {

	public GameObject m_playerPrefab;
	public GameObject m_ghostPrefab;
	public Transform[] m_spawnpoints = new Transform[4];

	private PlayerInfo m_playerInfo;
	private GameObject m_player;

	public bool m_debug_is_Server = false;
	public bool m_debug_is_Client = false;
	public int m_debug_id = 0;
	public bool m_debug_local = false;

	// Use this for initialization
	void Start () {
	
		setPlayerInfo(new PlayerInfo("Default",m_debug_id,m_debug_local)); //temp
		Transform spawnpoint = m_spawnpoints[m_playerInfo.id];

		if(Network.isServer || m_debug_is_Server){
			//om vi är server skapa en faktisk spelare
			m_player = Instantiate(m_playerPrefab,spawnpoint.position,spawnpoint.rotation) as GameObject;

			InputHub hub = m_player.GetComponent<InputHub>();
			//är det vår spelare? ta input från den lokala klienten och sätt kameran efter
			if(m_playerInfo.local){
				hub.setInputMetod(GetComponent<InputHub>());
				setCameraTarget(m_player.transform);
				m_player.name = "Player " + m_playerInfo.name + " (Local)";
			}else{
				//ananrs ta remote inputen
				hub.setInputMetod(GetComponent<RemoteInput>());
				m_player.name = "Player " + m_playerInfo.name + " (Remote)";
			}
		}
		else if(Network.isClient || m_debug_is_Client){
			//om vi är en klient skapa en fake spelare
			m_player = Instantiate(m_ghostPrefab,spawnpoint.position,spawnpoint.rotation) as GameObject;

			if(m_playerInfo.local){
				m_player.name = "PlayerGhost " + m_playerInfo.name+ " (Local)";
				//är det vår fake spalare? få kameran att följa
				setCameraTarget(m_player.transform);
			}else{
				m_player.name = "PlayerGhost " + m_playerInfo.name+ " (Remote)";
			}
		}
	}

	void setPlayerInfo(PlayerInfo info){
		m_playerInfo = info;
	}

	void setCameraTarget(Transform target){
		Camera.main.transform.parent.GetComponent<CameraFollow>().SetTarget(m_player.transform);
	}

}
