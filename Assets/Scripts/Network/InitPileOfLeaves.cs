using UnityEngine;
using System.Collections;

public class InitPileOfLeaves : MonoBehaviour {
	public GameObject m_pileOfLeif;
	public GameObject m_powerupManager;

//	private NetworkView network;

	void Start () {
//		network = gameObject.AddComponent<NetworkView>();
//		network.stateSynchronization = NetworkStateSynchronization.Off;
//		network.observed = null;

		if (Network.isServer) {
			Network.Instantiate( m_pileOfLeif, Vector3.zero, Quaternion.identity, 0 );
			Network.Instantiate( m_powerupManager, Vector3.zero, Quaternion.identity, 0 );
		}
	}

}
