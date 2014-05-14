using UnityEngine;
using System.Collections;

public class CollectorCollider : MonoBehaviour {
//	private GameObject scoreKeeper;
	public int m_ID;

	void Start() {
		m_ID = GetComponent<ID> ().m_ID;
//		m_ID = 0;
	}

	//TODO: Ändra till RCP
//	void OnTriggerEnter(Collider col)
//	{
//		if (col.gameObject.CompareTag ("Leaf_collector")) {
//			ScoreKeeper.m_scores[m_ID] += 1;
//			col.gameObject.SetActive(false);
//		}
//	}
}
