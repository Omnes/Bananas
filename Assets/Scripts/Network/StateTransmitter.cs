using UnityEngine;
using System.Collections;

public class StateTransmitter : MonoBehaviour {

	public SyncMovement[] m_playerMovements;
	// Use this for initialization
	void Start () {
		//Det här kanske inte fungerar eftersom vi inte vet ordningen saker kommer ske i (dvs spelarna kanske inte existerar när det här körs)
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		m_playerMovements = new SyncMovement[players.Length];
		for(int i = 0; i < players.Length;i++){
			m_playerMovements[i] = players[i].GetComponent<SyncMovement>();
		}
		
	}
	
	// Update is called once per frame
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		if(stream.isWriting){
			for(int i = 0; i < m_playerMovements.Length; i++){
				SyncData sd = m_playerMovements[i].getData();
				stream.Serialize(ref sd.m_position);
				stream.Serialize(ref sd.m_velocity);
				stream.Serialize(ref sd.m_rotation);
			}
		}else{
			for(int i = 0; i < m_playerMovements.Length; i++){
				Vector3 position = new Vector3();
				Vector3 velocity = new Vector3();
				Quaternion rotation = new Quaternion();

				stream.Serialize(ref position);
				stream.Serialize(ref velocity);
				stream.Serialize(ref rotation);
				SyncData sd = new SyncData(position,velocity,rotation);
				m_playerMovements[i].assignData(sd);
			}
		}
	}
}
