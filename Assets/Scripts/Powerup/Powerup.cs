using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public static int ENERGY_DRINK 	= GetUniqueID();
	public static int LAZERZ 		= GetUniqueID();
	public static int COUNT			= GetUniqueID();
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}

	private Rigidbody m_rigidbody;
	private Transform m_transform;

	void Start() {
		m_rigidbody = rigidbody;
		m_transform = transform;
	}

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

	public void FixedUpdate()
	{
//		transform.Rotate (Vector3.up, 45 * Time.deltaTime);
		Quaternion prevAngle = m_transform.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + Vector3.up * 45 * Time.deltaTime );
		m_rigidbody.MoveRotation(newAngle);
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
