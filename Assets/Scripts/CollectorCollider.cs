using UnityEngine;
using System.Collections;

public class CollectorCollider : MonoBehaviour {
	private GameObject scoreKeeper;

	void Start() {
//		scoreKeeper = GameObject.Find("global_scripts");
//		Debug.Log ("1 " + scoreKeeper);
	}

	void OnTriggerEnter(Collider col)
	{
//		Debug.Log ("Enter :" + col.gameObject.tag + " :" + col.gameObject);
//		if (col.gameObject.CompareTag ("Leaf")) {
//			Debug.Log ("Leaf enter");
//			GameObject leaf = col.gameObject;
//			score += 1;
//			Destroy( leaf );
//		}

		if (col.gameObject.CompareTag ("Leaf_collector")) {
//			Debug.Log ("Leaf enter");
//			score += 1;
//			scoreKeeper.m_score += 1;
//			Debug.Log ("2 " + scoreKeeper);

			ScoreKeeper.m_score +=1;

//			Debug.Log (scoreKeeper.m_score);
//			Destroy( gameObject );
			gameObject.SetActive(false);
		}
	}

//	void OnTriggerStay(Collider col)
//	{
//		if (col.gameObject.CompareTag ("Leaf")) {
//			Debug.Log ("Leaf Stay");
//			GameObject leaf = col.gameObject;
//			Destroy( leaf );
//		}
//	}
}
