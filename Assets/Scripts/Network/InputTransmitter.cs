using UnityEngine;
using System.Collections;

public class InputTransmitter : MonoBehaviour {

	private InputMetod m_input;
	public InputMetod m_inputTarget;
	
	// Use this for initialization
	void Start () {
		GameObject localInput = GameObject.Find("local_input");
		if(localInput == null) Debug.LogError("could not find the object local_input");
		m_input = localInput.GetComponent<InputHub>();
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){

		//bara den som äger networkviewn som kommer köra detta... (det antagandet görs iallafall här)
		if(Network.isClient && stream.isWriting){
			//sending to the server (for some reason they can only send vec3 not vec2)
			Vector3 sendVec = packAsVec3(m_input.getCurrentInputVector(),m_input.getCurrentBlowingPower());
			stream.Serialize(ref sendVec);
			Debug.Log("sent: " + sendVec);
		}else if (Network.isServer && stream.isReading){
			//reciving
			Vector3 recivedVec = new Vector3();
			stream.Serialize (ref recivedVec);
			//Debug.Log("recived: " + recivedVec);
			m_inputTarget.setCurrentInputVector(new Vector2(recivedVec.x,recivedVec.y));
			m_inputTarget.setCurrentBlowingPower(recivedVec.z);
		}
	}


	Vector3 packAsVec3(Vector2 v,float f){
		return new Vector3(v.x,v.y,f);
	}
}
