﻿using UnityEngine;
using System.Collections;

public class LeafManager : MonoBehaviour {
	private GameObject[] leafs;
	private Vector2[] m_newPositions;
	private Vector2[] m_oldPositions;
	private float lerpTime;

	public bool useLerp = true;

	public GameObject m_prefabLeaf;

	private LeafPhysics[] m_leafPhysics;

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

	private NetworkView network;
	public bool m_spawnInOffline = false;

	private float m_lastSyncTime = 0f;
	private float m_expectedSyncDelta = 1f/15f;
	private float m_syncDeltaTime = 0f;

//	public float m_sqrResyncDistance = Mathf.Sqrt(0.4f);

	/**
	 * Initializes variables
	 */
	void Awake () {
		network = networkView;

		leafs = new GameObject[m_leafCache];
		m_newPositions = new Vector2[m_leafCache];
		m_oldPositions = new Vector2[m_leafCache];
//		m_leafPhysics = new LeafPhysics[m_leafCache];

		float heightIncrease = m_totalHeight / m_leafCache;

		for (int i = 0; i < leafs.Length; i++) {
			leafs[i] = Instantiate(m_prefabLeaf) as GameObject;
			leafs[i].name = "Leaf_" + i;
			leafs[i].transform.parent = gameObject.transform;
			leafs[i].transform.position = new Vector3(0, m_startHeight + heightIncrease * i, 0);
			leafs[i].SetActive(false);
//			m_leafPhysics[i] = leafs[i].GetComponent<LeafPhysics>();
		}

//		m_lastSyncTime = Time.time;
//		m_expectedSyncDelta = 1f/Network.sendRate;
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

	/**
	 * Find and return an unused leaf from the leaf pool
	 * Returns null if no leaf can be used
	 */
	public GameObject SpawnLeaf() {
		for (int i = 0; i < leafs.Length; i++) {
			if ( leafs[i].activeSelf == false ) {
//				leafs[i].rigidbody.velocity = Vector3.zero;
				leafs[i].SetActive(true);
				return leafs[i];
			}
		}
		Debug.LogError ("LeafManager.cs: Trying to create more leafs than is available in the pool (Max " + leafs.Length + ")");
		return null;
	}

//	public GameObject SpawnLeaf(int index) {
//		if (index >= 0 || index < leafs.Length) {
//			if (leafs[index].activeSelf == false) {
//				leafs[index].rigidbody.velocity = Vector3.zero;
//				leafs[index].SetActive (true);
//				return leafs[index];
//			}
//		}
//		return null;
//	}

	public GameObject GetLeaf(int index) {
		if (index >= 0 || index < leafs.Length) {
			return leafs [index];
		}
		return null;
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

			m_newPositions[i] = new Vector2(leaf.transform.position.x, leaf.transform.position.z);
			m_oldPositions[i] = new Vector2(leaf.transform.position.x, leaf.transform.position.z);
		}
	}

	/**
	 * Send and receive all leaves position
	 */
//	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
//		if (Network.isServer && stream.isWriting) {
//			//Sending
//			Vector3 vec;
//			for (int i = 0; i < leafs.Length; i++) {
////				if ( leafs[i].rigidbody.velocity.sqrMagnitude > 1 ) {	//TODO: Kan bli problem eftersom det skickas mindre än det tas emot!
//					vec = new Vector3(i, leafs[i].transform.position.x, leafs[i].transform.position.z);
//					stream.Serialize( ref vec );
////				}
//			}
//		}
//		else if (Network.isClient && stream.isReading) {
//			Vector3 vec = Vector3.zero;
//			GameObject leaf;
//			int index;
//			lerpTime = 0;
//			for (int i = 0; i < leafs.Length; i++) {
//				stream.Serialize( ref vec );
//				index = (int)vec.x;
//				leaf = leafs[index];
//
//				Vector2 oldPosition = new Vector2(leaf.transform.position.x, leaf.transform.position.z);
//				Vector2 newPos = new Vector2(vec.y, vec.z);
//				m_leafPhysics[index].setGhostLeafPosition(new Vector3(newPos.x,leaf.transform.position.y,newPos.y));
//
//			}
//
//		}
//	}

	/**
	 * Updates the leaves local position so that the match the server position
	 */
//	void Update() {
//		if ( Network.isClient ) {
//			if (useLerp) {
//				lerpTime = (Time.time - m_lastSyncTime) / m_expectedSyncDelta;
////				Debug.Log ("LerpTime: " + lerpTime);
//				for (int i = 0; i < leafs.Length; i++) {
////					if ( leafs[i].rigidbody.velocity.sqrMagnitude > 1 ) {
//						leafs [i].transform.position = new Vector3 (Mathf.Lerp (m_oldPositions [i].x, m_newPositions [i].x, lerpTime),
//								                                   leafs [i].transform.position.y, 
//								                                   Mathf.Lerp (m_oldPositions [i].y, m_newPositions [i].y, lerpTime));
////					}
//				}
//			}
//			else {
//				for (int i = 0; i < leafs.Length; i++) {
//					leafs [i].transform.position = new Vector3 (m_newPositions [i].x, leafs [i].transform.position.y, m_newPositions [i].y);
//				}
//			}
//		}
//	}
}
