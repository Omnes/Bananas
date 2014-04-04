using UnityEngine;
using System.Collections;

public class scr_leafCollector : MonoBehaviour {
	private int score = 0;

	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("Enter :" + col.gameObject.tag + " :" + col.gameObject);
		if (col.gameObject.CompareTag ("Leaf")) {
			Debug.Log ("Leaf enter");
			GameObject leaf = col.gameObject;
			score += 1;
			Destroy( leaf );
		}
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log ("Collision Enter :" + col.gameObject.tag + " :" + col.gameObject);
//		if (col.gameObject.CompareTag ("Leaf")) {
//			Debug.Log ("Leaf enter");
//			GameObject leaf = col.gameObject;
//			score += 1;
//			Destroy( leaf );
//		}
	}
}
