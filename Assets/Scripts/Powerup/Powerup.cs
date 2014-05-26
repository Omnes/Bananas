using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public float m_rotationSpeed = 45;	//Angle per second
	public float m_growSpeed = 0.5f;	//scale 0 to 1 in seconds

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
		transform.localScale = Vector3.zero;
	}

	void OnTriggerEnter(Collider col)
	{
		if ( Network.isServer && !m_hasBeenPickedUp) {
			if (col.gameObject.CompareTag ("Player")) {
				m_hasBeenPickedUp = true;

				PowerupManager.SynchronizePowerupGet (col.gameObject);

				Network.Instantiate(Prefactory.prefab_powerupPickup, transform.position, transform.rotation, 0);
//				GameObject particles = Instantiate(Prefactory.prefab_powerupPickup, transform.position, Prefactory.prefab_powerupPickup.transform.localRotation) as GameObject;
//				Destroy(particles, particles.particleSystem.duration + particles.particleSystem.startLifetime);

				SoundManager.Instance.playOneShot (SoundManager.POWERUP_PICKUP);
				PowerupManager.Remove(gameObject);
			}
	    }
	}

	public void Update()
	{
		Quaternion prevAngle = m_rigidbody.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + Vector3.up * m_rotationSpeed * Time.deltaTime);
		m_rigidbody.MoveRotation(newAngle);

		if (transform.localScale.x < 1.0f) {
			transform.localScale += Vector3.one * Time.deltaTime / m_growSpeed;
		}
	}

}
