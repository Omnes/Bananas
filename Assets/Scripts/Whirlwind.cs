using UnityEngine;
using System.Collections;

public class Whirlwind : MonoBehaviour {

	public float m_rotationSpeed = 120f;

	private Rigidbody m_rigidbody;
	private Transform m_transform;
	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Profiler.BeginSample("Rotate Leifs");
//		Vector3 prevAngle = transform.rotation.eulerAngles;
//		Quaternion newAngle = Quaternion.Euler(prevAngle + Vector3.forward * Time.deltaTime * m_rotationSpeed);
//		m_rigidbody.MoveRotation(newAngle);
		m_transform.Rotate(Vector3.up * Time.deltaTime * m_rotationSpeed);
		Profiler.EndSample();
	}


	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Leaf_Collector")){
			Debug.Log ("Whirlwind");
		}
	}
}
