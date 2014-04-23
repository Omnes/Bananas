using UnityEngine;
using System.Collections;

public class LeafManager : MonoBehaviour {
	private GameObject[] leafs;
	public GameObject m_prefabLeaf;

	[Range(0, 10000)]
	public int m_leafCache = 100;

	[Range(0, 10000)]
	public int m_leafStartCount = 100;
	
	[Range(0f, 100f)]
	public float m_range = 5f;
	
	[Range(-10f, 10f)]
	public float m_startHeight = 0.001f;
	
	[Range(0f, 10f)]
	public float m_totalHeight = 1.0f;

	private NetworkView network;

	void Awake () {
		network = networkView;
//		network = gameObject.AddComponent<NetworkView>();
//		network.stateSynchronization = NetworkStateSynchronization.Off;
//		network.observed = null;

		leafs = new GameObject[m_leafCache];

		float heightIncrease = m_totalHeight / m_leafCache;

		for (int i = 0; i < leafs.Length; i++) {
			leafs[i] = Instantiate(m_prefabLeaf) as GameObject;
			leafs[i].name = "Leaf_" + i;
			leafs[i].transform.parent = gameObject.transform;
			leafs[i].transform.position = new Vector3(0, m_startHeight + heightIncrease * i, 0);
			leafs[i].SetActive(false);
		}
	}

	void Start () {
		if (Network.isServer) {
			network.RPC ("SpawnLeafs", RPCMode.All, Random.Range (int.MinValue, int.MaxValue));
		}
	}
	
	public GameObject SpawnLeaf() {
		for (int i = 0; i < leafs.Length; i++) {
			if ( leafs[i].activeSelf == false ) {
				leafs[i].rigidbody.velocity = Vector3.zero;
				leafs[i].SetActive(true);
				return leafs[i];
			}
		}
		Debug.LogError ("LeafManager.cs: Trying to create more leafs than is available in the pool (Max " + leafs.Length + ")");
		return null;
	}

	public GameObject SpawnLeaf(int index) {
		if (index >= 0 || index < leafs.Length) {
			if (leafs[index].activeSelf == false) {
				leafs[index].rigidbody.velocity = Vector3.zero;
				leafs[index].SetActive (true);
				return leafs[index];
			}
		}
		return null;
	}

	public GameObject GetLeaf(int index) {
		if (index >= 0 || index < leafs.Length) {
			return leafs [index];
		}
		return null;
	}

	[RPC]
	void SpawnLeafs(int seed){
		Random.seed = seed;
		for (int i = 0; i < m_leafStartCount; i++) {
			GameObject leaf = SpawnLeaf();
			leaf.transform.position = new Vector3(Random.Range(-m_range, m_range), leaf.transform.position.y, Random.Range(-m_range, m_range));
			leaf.transform.Rotate(new Vector3(0,0,Random.Range(0f,359f)));
		}
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
		if (stream.isWriting) {
			//Sending
		}
		else {
			//Receiving
		}
	}

//	public GameObject[] GetActiveLeafs() {
//		GameObject[] activeLeafs;
//		for (int i = 0; i < leafs.Length; i++) {
//			if ( leafs[i].activeSelf ) {
//
//			}
//		}
//	}
}
