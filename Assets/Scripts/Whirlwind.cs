using UnityEngine;
using System.Collections;

public class Whirlwind : MonoBehaviour {

	public float m_rotationSpeed = 120f;

	private Rigidbody m_rigidbody;
	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Profiler.BeginSample("Rotate Leifs");
//		Vector3 prevAngle = transform.rotation.eulerAngles;
//		Quaternion newAngle = Quaternion.Euler(prevAngle + Vector3.forward * Time.deltaTime * m_rotationSpeed);
//		m_rigidbody.MoveRotation(newAngle);
		transform.Rotate(Vector3.up * Time.deltaTime * m_rotationSpeed);
		Profiler.EndSample();
	}
}
