using UnityEngine;
using System.Collections;

public class scr_movement : MonoBehaviour {

	public float m_speed = 1;
	public float m_rotationSpeed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 current_velocity = rigidbody.velocity;
		float left_power = 0;
		float right_power = 0;

		if (Input.GetKey (KeyCode.Q)) {
			left_power = 1;
		}
		if (Input.GetKey (KeyCode.A)) {
			left_power = -1;
		}
		if (Input.GetKey (KeyCode.E)) {
			right_power = 1;
		}
		if (Input.GetKey (KeyCode.D)) {
			right_power = -1;
		}

		rigidbody.AddForce (Vector3.forward*(left_power + right_power)*m_speed,ForceMode.VelocityChange);
		rigidbody.AddTorque (Vector3.up * (left_power + right_power) * m_rotationSpeed);

	}
}
