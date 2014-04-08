using UnityEngine;
using System.Collections;

public class scr_spawnLeaves : MonoBehaviour {
	public Transform m_parent;
	public Material m_leafMaterial;
	public GameObject m_prefabLeaf;

	[Range(0, 10000)]
	public int   m_leafCount = 100;

	[Range(0f, 100f)]
	public float m_range = 5f;

	[Range(-10f, 10f)]
	public float m_startHeight = 0.001f;

	[Range(0f, 1f)]
	public float m_heightIncrease = 0.001f;

	void Start () {
		for (int x = 0; x < m_leafCount; x++) {
			GameObject child = Instantiate(m_prefabLeaf, new Vector3(Random.Range(-m_range, m_range), m_startHeight + x * m_heightIncrease, Random.Range(-m_range, m_range)), m_prefabLeaf.transform.rotation/*Quaternion.identity*/) as GameObject;
			child.transform.parent = m_parent;
			child.name = "leaf_"+x;
			child.renderer.sharedMaterial = m_leafMaterial;

		}
	}
}
