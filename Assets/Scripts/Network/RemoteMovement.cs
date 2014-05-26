using UnityEngine;
using System.Collections;

public class RemoteMovement : MonoBehaviour {

	public Vector3 m_rotationDelta = Vector3.zero;
	private Rigidbody m_rigidbody;
	private Transform m_transform;

//	private Quaternion m_ghostRotation;

//	private float m_resyncLimit = 0.05f;
//	public float m_resyncForce = 5f;

	//only temp for debug
	private float share;
	private float max;
	private Vector3 newRotationSpeed;
	private Vector3 predictedDelta;

//	private bool initiated = false;
	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		transform.Rotate(m_rotationDelta*Time.deltaTime);
//		predictedDelta = m_ghostRotation.eulerAngles - transform.rotation.eulerAngles; 
//
//
//		max = m_rotationDelta.magnitude + predictedDelta.magnitude;
//		if(max < m_resyncLimit){
//			max = 1f;
//		}
//		share = m_rotationDelta.magnitude / max;
//
//		
//		newRotationSpeed = m_rotationDelta * share + predictedDelta * m_resyncForce * (1-share);


		Quaternion prevAngle = m_transform.rotation;
		Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + m_rotationDelta * Time.deltaTime);
		m_rigidbody.MoveRotation(newAngle);
	}

	public void setGhostRotation(Quaternion rot,float rotSpeed){
//		m_ghostRotation = rot;
		transform.rotation = rot; //migth need to be masked
		m_rotationDelta = new Vector3(0,rotSpeed,0);
	}

	void OnDrawGizmos(){
		if(Network.isClient){
			Gizmos.color = Color.white;
			float len = 4f;
			Vector3 pos = transform.position + Vector3.up*0.5f;
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(pos,pos + transform.forward*len);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position,0.5f);
		}
	}
}
