using UnityEngine;
using System.Collections;

public class StateTransmitter : MonoBehaviour {

	public SyncMovement[] m_playerMovements;
	// Use this for initialization
	void Start () {
		//Det här kanske inte fungerar eftersom vi inte vet ordningen saker kommer ske i (dvs spelarna kanske inte existerar när det här körs)
		GameObject[] players = sortPlayers(GameObject.FindGameObjectsWithTag("Player"));
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
				stream.Serialize(ref sd.m_blowing);
			}
		}else{
			for(int i = 0; i < m_playerMovements.Length; i++){
				Vector3 position = new Vector3();
				Vector3 velocity = new Vector3();
				Quaternion rotation = new Quaternion();
				float blowing = new float();

				stream.Serialize(ref position);
				stream.Serialize(ref velocity);
				stream.Serialize(ref rotation);
				stream.Serialize(ref blowing);
				SyncData sd = new SyncData(position,velocity,rotation,blowing);
				m_playerMovements[i].assignData(sd);
			}
		}
	}

	private GameObject[] sortPlayers(GameObject[] array){
		int length = array.Length;
		for(int i = 0; i < length;i++){
			for(int j = 0; j < length;j++){
				if(array[i].GetComponent<SyncMovement>().getID() < array[j].GetComponent<SyncMovement>().getID()){
					GameObject temp = array[i];
					array[i] = array[j];
					array[j] = temp;
				}
			}
		}
		return array;
	}
}
