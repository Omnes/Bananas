﻿using UnityEngine;
using System.Collections;

public class scr_spawnLeaves : MonoBehaviour {
	public Transform m_parent;
	public GameObject m_prefabLeaf;

	[Range(0, 1000)]
	public int   m_leafCount = 100;

	[Range(0f, 100f)]
	public float m_range = 5f;

	[Range(0f, 10f)]
	public float m_startHeight = 0.001f;

	[Range(0f, 1f)]
	public float m_heightIncrease = 0.001f;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < m_leafCount; x++) {
			GameObject child = Instantiate(m_prefabLeaf, new Vector3(Random.Range(-m_range, m_range), m_startHeight + x * m_heightIncrease, Random.Range(-m_range, m_range)), m_prefabLeaf.transform.rotation/*Quaternion.identity*/) as GameObject;
			child.transform.parent = m_parent;
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
