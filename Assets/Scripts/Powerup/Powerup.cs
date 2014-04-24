using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public static int ENERGY_DRINK 	= GetUniqueID();
	public static int LAZERZ 		= GetUniqueID();
	public static int COUNT			= GetUniqueID();
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Player")) {
			OnPowerupGet(col.gameObject);
//			Destroy( gameObject );
		}
	}

	public virtual void OnPowerupGet(GameObject obj)
	{
		Debug.Log ("Powerup");
	}


}
