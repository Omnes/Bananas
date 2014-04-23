using UnityEngine;
using System.Collections;

public class InputTransmitter : MonoBehaviour {

	public InputMetod m_input;
	public InputMetod m_inputTarget;

	private PeerType m_type; 
	enum PeerType{CLIENT,SERVER,NOTCONNECTED};

	// Use this for initialization
	void Start () {
		if(Network.isClient){
			m_type = PeerType.CLIENT;
		}else if(Network.isServer){
			m_type = PeerType.SERVER;
		}else{
			m_type = PeerType.NOTCONNECTED;
		}
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){

		if(m_type == PeerType.CLIENT && stream.isWriting){
			//sending to the server (for some reason they can only send vec3 not vec2)
			Vector3 sendVec = packAsVec3(m_input.getCurrentInputVector(),m_input.getCurrentBlowingPower());
			stream.Serialize(ref sendVec);
		}else if (m_type == PeerType.SERVER && stream.isReading){
			//reciving
			Vector3 recivedVec = new Vector3();
			stream.Serialize (ref recivedVec);
			m_input.setCurrentInputVector(new Vector2(recivedVec.x,recivedVec.y));
			m_input.setCurrentBlowingPower(recivedVec.z);
		}
	}


	Vector3 packAsVec3(Vector2 v,float f){
		return new Vector3(v.x,v.y,f);
	}
}
