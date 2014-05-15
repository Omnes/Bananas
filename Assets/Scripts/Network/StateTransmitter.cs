using UnityEngine;
using System.Collections;


public class StateTransmitter : MonoBehaviour {

	public SyncMovement m_playerMovement;

	[RPC]
	public void linkToID(int id){
		m_playerMovement = SyncMovement.s_syncMovements[id];
	}

	// Update is called once per frame
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		if(m_playerMovement != null){
			if(stream.isWriting){
				SyncData sd = m_playerMovement.getData();
				stream.Serialize(ref sd.m_position);
				stream.Serialize(ref sd.m_velocity);
				float angle = sd.m_rotation.eulerAngles.y;
				stream.Serialize(ref angle);
				stream.Serialize(ref sd.m_rotationSpeed);
				//stream.Serialize(ref sd.m_rotation);
//				stream.Serialize(ref sd.m_blowing);

			}else{
				Vector3 position = new Vector3();
				Vector3 velocity = new Vector3();
				Quaternion rotation = new Quaternion();
				float blowing = new float();
				float rotationDelta = new float();
				float angle = new float();

				stream.Serialize(ref position);
				stream.Serialize(ref velocity);

				stream.Serialize(ref angle);
				stream.Serialize(ref rotationDelta);
				//stream.Serialize(ref rotation);
//				stream.Serialize(ref blowing);

				rotation = Quaternion.Euler(new Vector3(0,angle,0));
				SyncData sd = new SyncData(position,velocity,rotation,rotationDelta,blowing);
				m_playerMovement.assignData(sd);
			}
		}
	}

}
