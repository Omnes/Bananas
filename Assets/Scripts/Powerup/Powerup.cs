using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			OnPowerupGet(col.gameObject);
			Destroy( gameObject );
		}
	}

	public virtual void OnPowerupGet(GameObject obj)
	{
		Debug.Log ("Powerup");
	}

}
