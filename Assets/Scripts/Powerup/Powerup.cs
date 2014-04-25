using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public GameObject prefab_player;

	public static int ENERGY_DRINK 	= GetUniqueID();
	public static int LAZERZ 		= GetUniqueID();
	public static int COUNT			= GetUniqueID();
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}

	void OnTriggerEnter(Collider col)
	{
		if ( Network.isServer ) {
			if (col.gameObject.CompareTag ("Player")) {
				OnPowerupGet(col.gameObject);
//				Network.Destroy(gameObject.networkView.viewID);
				Network.Destroy( networkView.viewID );
			}
	    }
	}

	public virtual void OnPowerupGet(GameObject obj)
	{
		Debug.Log ("Powerup");
	}

}
