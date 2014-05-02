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
		if ( Network.isServer ) {
			if (col.gameObject.CompareTag ("Player")) {
				OnPowerupGet(col.gameObject);
//				Network.Destroy( networkView.viewID );
				PowerupManager.Remove(gameObject);
			}
	    }
	}

//	void OnDestroy()
//	{
//		Debug.Log ("DESTROY POWERUUUUP");
//	}

	public void Update()
	{
		transform.Rotate (Vector3.up, 45 * Time.deltaTime);
	}

//	public void Destroy ()
//	{
//
//	}

//	public abstract void OnPowerupGet(GameObject obj);
	public virtual void OnPowerupGet(GameObject obj)
	{
//		Debug.Log ("Powerup");
	}

}
