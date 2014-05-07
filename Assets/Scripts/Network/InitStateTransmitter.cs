using UnityEngine;
using System.Collections;

public class InitStateTransmitter : MonoBehaviour {
	public GameObject m_stateTransmitterPrefab;

	// Use this for initialization
	void Start () {
		if(Network.isServer){
			Network.Instantiate(m_stateTransmitterPrefab,Vector3.zero,Quaternion.identity,0);
		}
	}

}
