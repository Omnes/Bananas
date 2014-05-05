using UnityEngine;
using System.Collections;

public class RemoteMovement : MonoBehaviour {

	public Vector3 m_rotationDelta = Vector3.zero;
	private Rigidbody m_rigidbody;
	private Transform m_transform;

//	private bool initiated = false;
	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
//		transform.Rotate(m_rotationDelta*Time.deltaTime);

		Quaternion prevAngle = m_transform.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + m_rotationDelta * Time.deltaTime);
		m_rigidbody.MoveRotation(newAngle);
	}

	public void setSyncRotationSpeed(float rotSpeed){
		m_rotationDelta = new Vector3(0,rotSpeed,0);
	}
}
