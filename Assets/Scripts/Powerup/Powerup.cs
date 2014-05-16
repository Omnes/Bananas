using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public const float ROTATION_SPEED = 45;
	public const float DEATH_TIME = 0.5f;

	public static int TIME_BOMB 		= GetUniqueID();
	public static int BIG_LEAF_BLOWER 	= GetUniqueID();
	public static int EMP 				= GetUniqueID();
	public static int COUNT				= GetUniqueID();
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}

	private Rigidbody m_rigidbody;
	private Transform m_transform;

	private bool m_hasBeenPickedUp = false;
	private GameObject m_pickingObject;
	private float m_killTimer = 0;

	void Start() {
//		m_rigidbody = transform.FindChild ("powerup_questionmark").rigidbody;
		m_rigidbody = GetComponentInChildren<Rigidbody> ();
//		Debug.Log ("Test: " + m_rigidbody);
//		m_rigidbody = rigidbody;
		m_transform = transform;
	}

	void OnTriggerEnter(Collider col)
	{
		if ( Network.isServer ) {
			if (col.gameObject.CompareTag ("Player")) {
				m_hasBeenPickedUp = true;
				m_pickingObject = col.gameObject;
				//Play animation
				SoundManager.Instance.playOneShot (SoundManager.POWERUP_PICKUP);
			}
	    }
	}

	void Update()
	{
		if (m_hasBeenPickedUp) {
			m_killTimer += Time.deltaTime;
			if (m_killTimer > DEATH_TIME) {
				PowerupManager.SynchronizePowerupGet (m_pickingObject);
				PowerupManager.Remove(gameObject);
			}
		}
	}

	public void FixedUpdate()
	{
		Quaternion prevAngle = m_rigidbody.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + Vector3.up * ROTATION_SPEED * Time.deltaTime);
		m_rigidbody.MoveRotation(newAngle);
	}

}
