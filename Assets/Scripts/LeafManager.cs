using UnityEngine;
using System.Collections;

public class LeafManager : MonoBehaviour {
	private GameObject[] leafs;
	public int m_leafCache = 1000;

//	public Transform m_parent;
//	public Material m_leafMaterial;
	public GameObject m_prefabLeaf;

	private NetworkView network;

//	private static LeafManager instance;
//	public static LeafManager Instance
//	{
//		get
//		{
//			if (instance == null) {
//				GameObject go = new GameObject();
//				instance = go.AddComponent<LeafManager>();
//				go.name = "LeafManager";
//
//				instance.leafs = new GameObject[LEAF_CACHE];
//				for (int i = 0; i < instance.leafs.Length; i++) {
//					instance.leafs[i] = Instantiate(m_prefabLeaf) as GameObject;
//					instance.leafs[i].name = "Leaf_" + i;
//					instance.leafs[i].transform.parent = gameObject;
//					instance.leafs[i].SetActive(false);
//				}
//			}
//			return instance;
//		}
//	}

	// Use this for initialization
	void Start () {
		network = new NetworkView ();

		leafs = new GameObject[m_leafCache];

		for (int i = 0; i < leafs.Length; i++) {
			leafs[i] = Instantiate(m_prefabLeaf) as GameObject;
			leafs[i].name = "Leaf_" + i;
			leafs[i].transform.parent = gameObject.transform;
			leafs[i].SetActive(false);
		}

		if (Network.isServer) {
				network.RPC ("SpawnLeafs", RPCMode.All, Random.Range (-1000, 1000));
		} else {
			SpawnLeafs(42);
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
		for (int i = 0; i < 10; i++) {
			GameObject leaf = SpawnLeaf();
			leaf.transform.position = new Vector3(Random.Range(-5f, 5f), 0.1f, Random.Range(-5f, 5f));
			leaf.transform.Rotate(new Vector3(0,0,Random.Range(0f,359f)));
		}
	}

//	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
//		if (stream.isWriting) {
			//Sending
//			stream.Serialize (Random.Range ());
//		}
//		else {
			//Receiving
//		}
//	}

//	public void KillLeaf(GameObject leaf) {
//		leaf.SetActive(false);
//	}

//	public GameObject[] GetActiveLeafs() {
//		GameObject[] activeLeafs;
//		for (int i = 0; i < leafs.Length; i++) {
//			if ( leafs[i].activeSelf ) {
//
//			}
//		}
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
