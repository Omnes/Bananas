using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public float m_rotationSpeed = 45;

	public static int TIME_BOMB 		= GetUniqueID();
	public static int BIG_LEAF_BLOWER 	= GetUniqueID();
	public static int EMP 				= GetUniqueID();
	public static int COUNT				= GetUniqueID();
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}

	private Rigidbody m_rigidbody;

	private bool m_hasBeenPickedUp = false;
	
	void Start() {
		m_rigidbody = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if ( Network.isServer && !m_hasBeenPickedUp) {
			if (col.gameObject.CompareTag ("Player")) {
				m_hasBeenPickedUp = true;

				PowerupManager.SynchronizePowerupGet (col.gameObject);

				GameObject particles = Instantiate(Prefactory.prefab_powerupPickup, transform.position, Prefactory.prefab_powerupPickup.transform.localRotation) as GameObject;
				Destroy(particles, particles.particleSystem.duration + particles.particleSystem.startLifetime);

				SoundManager.Instance.playOneShot (SoundManager.POWERUP_PICKUP);
				PowerupManager.Remove(gameObject);
			}
	    }
	}



	public void FixedUpdate()
	{
		Quaternion prevAngle = m_rigidbody.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + Vector3.up * m_rotationSpeed * Time.deltaTime);
		m_rigidbody.MoveRotation(newAngle);
	}

}
