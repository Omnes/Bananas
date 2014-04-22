using UnityEngine;
using System.Collections;

public class scr_leafPhysics : MonoBehaviour {
//	private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
//	public float m_maxVelocity = float.MaxValue;

	[Range(0.0f, 1.0f)]
	public float m_friction = 1.0f;

//	public float m_maxVelocity = 4.0f;
	public float m_rotationSpeed = 30f;

	public float m_minSleep = 0.01f;

	private Rigidbody m_rigidbody;
	private Transform m_transform;

	void Start(){
		m_rigidbody = rigidbody;
		m_transform = transform;
	}
	
	void FixedUpdate () {
//		transform.position += velocity * Time.deltaTime;
//		velocity *= friction;

		Vector3 velocity = m_rigidbody.velocity;
		if(velocity.magnitude > m_minSleep){
			m_rigidbody.velocity *= m_friction;

	//		rigidbody.velocity.Scale (  );
	//		if (rigidbody.velocity.magnitude > m_maxVelocity) {
	//			rigidbody.velocity = rigidbody.velocity.normalized * m_maxVelocity;
	//		}
			m_transform.Rotate(Vector3.forward*velocity.magnitude*Time.deltaTime*m_rotationSpeed);
		}else if(velocity.magnitude > 0){
			m_rigidbody.velocity = Vector3.zero;
		}
	}

//	public void AddForce(Vector3 force)
//	{
//		force = new Vector3(force.x, 0, force.z);
//		velocity += force;
//		if (velocity.magnitude > maxVelocity) {
//			velocity = velocity.normalized * maxVelocity;
//		}
//	}
}
