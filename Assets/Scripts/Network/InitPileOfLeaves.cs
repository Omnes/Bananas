using UnityEngine;
using System.Collections;

public class InitPileOfLeaves : MonoBehaviour {
	public GameObject m_prefab;

	private NetworkView network;

	void Start () {
		network = gameObject.AddComponent<NetworkView>();
		network.stateSynchronization = NetworkStateSynchronization.Off;
		network.observed = null;

		if (Network.isServer) {
			Network.Instantiate( m_prefab, Vector3.zero, Quaternion.identity, 0 );
//			GameObject go = Network.Instantiate( m_prefab, Vector3.zero, Quaternion.identity, 0 ) as GameObject;
//			LeafManager lm = go.GetComponent<LeafManager>();
		}
	}

//	[RPC]
//	void SpawnLeafs(int seed){
//		Debug.Log ("Dafuuuuq");
//	}

}
