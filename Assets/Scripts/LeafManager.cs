using UnityEngine;
using System.Collections;

public class LeafManager : MonoBehaviour {
	
	public static LeafManager s_lazyInstance = null;

	private GameObject[] m_leafs;
	private LeafLogic[] m_leafLogics;

	public GameObject m_prefabLeaf;

	[Range(0, 10000)]
	public int m_leafCache = 100;

	[Range(0, 10000)]
	public int m_leafStartCount = 100;
	
	[Range(0f, 100f)]
	public float m_range = 10f;
	
	[Range(-10f, 10f)]
	public float m_startHeight = 0.001f;
	
	[Range(0f, 10f)]
	public float m_totalHeight = 1.0f;

	public int m_minLeafCount = 0;

	private NetworkView network;
	public bool m_spawnInOffline = false;

//	public float m_sqrResyncDistance = Mathf.Sqrt(0.4f);

	/**
	 * Initializes variables
	 */
	void Awake () {
		s_lazyInstance = this;
		network = networkView;

		m_leafs = new GameObject[m_leafCache];
		m_leafLogics = new LeafLogic[m_leafCache];

		float heightIncrease = m_totalHeight / m_leafCache;

		for (int i = 0; i < m_leafs.Length; i++) {
			m_leafs[i] = Instantiate(m_prefabLeaf) as GameObject;
			m_leafs[i].name = "Leaf_" + i;
			m_leafs[i].transform.localPosition = new Vector3(0,m_startHeight + heightIncrease * i,0);
			m_leafs[i].transform.parent = gameObject.transform;
			m_leafs[i].SetActive(false);
			m_leafLogics[i] = m_leafs[i].GetComponent<LeafLogic>();
			m_leafLogics[i].m_id = i;

		}

	}
	
	void Start () {
		if (Network.isServer) {
			network.RPC ("SpawnLeafs", RPCMode.All, Random.Range (int.MinValue, int.MaxValue));
		}
		if (m_spawnInOffline)
		{
			SpawnLeafs (0);
		}
	}

	void Update() {
		if (Network.isServer && ActiveLeafs() <= m_minLeafCount) {
			network.RPC ("SpawnLeafs", RPCMode.All, Random.Range (int.MinValue, int.MaxValue));
		}
	}

	/**
	 * Find and return an unused leaf from the leaf pool
	 * Returns null if no leaf can be used
	 */
	public GameObject SpawnLeaf() {
		for (int i = 0; i < m_leafs.Length; i++) {
			if ( m_leafs[i].activeSelf == false ) {
				m_leafs[i].SetActive(true);
				return m_leafs[i];
			}
		}
		Debug.LogError ("LeafManager.cs: Trying to create more leafs than is available in the pool (Max " + m_leafs.Length + ")");
		return null;
	}

	public GameObject GetLeaf(int index) {
		if (index >= 0 || index < m_leafs.Length) {
			return m_leafs [index];
		}
		return null;
	}

	public int ActiveLeafs() {
		int count = 0;
		for (int i = 0; i < m_leafs.Length; i++) {
			if (m_leafs[i].activeSelf)
				count++;
		}
		return count;
	}

	/**
	 * Spawn leaves randomly on the level based on the defined parameters
	 * The seed is used to make sure that the leaves spawn on the same position for all clients
	 */
	[RPC]
	void SpawnLeafs(int seed){
		Random.seed = seed;
		for (int i = 0; i < m_leafStartCount; i++) {
			GameObject leaf = SpawnLeaf();
			leaf.transform.position = new Vector3(Random.Range(-m_range, m_range), leaf.transform.position.y, Random.Range(-m_range, m_range));
			leaf.transform.Rotate(new Vector3(0,0,Random.Range(0f,359f)));

		}
	}
	
	public void requestLeafDrop(int playerID,int count){
		int seed = Random.Range (0,int.MaxValue);
		network.RPC("RPCLeafDrop",RPCMode.All,playerID,count,seed);
	}


	[RPC]
	public void RPCLeafDrop(int playerID,int count,int seed){
		LeafBlower.s_leafBlowers[playerID].dropLeafs(count,seed);
	}

	public void pickUpLeaf(int playerID,Transform leaf){
		int leafID = leaf.GetComponent<LeafLogic>().m_id;
		network.RPC("RPCPickUpLeaf",RPCMode.All,playerID,leafID);
	}

	[RPC]
	public void RPCPickUpLeaf(int playerID,int leafID){
		LeafBlower.s_leafBlowers[playerID].addLeaf(m_leafs[leafID].transform);
	}
}
