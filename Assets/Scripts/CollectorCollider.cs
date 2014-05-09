using UnityEngine;
using System.Collections;

public class CollectorCollider : MonoBehaviour {
	private int m_ID;

	void Start() {
		m_ID = GetComponent<ID> ().m_ID;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Leaf")) {
			ScoreKeeper.m_scores[m_ID] += 1;
			SoundManager.Instance.playOneShot(SoundManager.SCORE);
			col.gameObject.SetActive(false);
		}
	}
}
