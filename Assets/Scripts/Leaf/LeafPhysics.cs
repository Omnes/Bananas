using UnityEngine;
using System.Collections;

public class LeafPhysics : MonoBehaviour {
//	private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
//	public float m_maxVelocity = float.MaxValue;

	[Range(0.0f, 1.0f)]
	public float m_friction = 1.0f;

//	public float m_maxVelocity = 4.0f;
	public float m_rotationSpeed = 30f;

	public float m_minSleep = 0.31f;

	private Rigidbody m_rigidbody;
	private Transform m_transform;

	private bool m_isClient;

	private Vector3 m_ghostPosition = Vector3.zero; 
	public float m_resyncForce = 15f;
	public float m_hardResyncLimit = 5f;

	void Start(){
		m_rigidbody = rigidbody;
		m_transform = transform;
		m_minSleep = Mathf.Sqrt (m_minSleep);
		m_isClient = Network.isClient;
		m_ghostPosition = m_transform.position;
	}
	
	void FixedUpdate () {
//		transform.position += velocity * Time.deltaTime;
//		velocity *= friction;

		Vector3 velocity = m_rigidbody.velocity;
		if(velocity.sqrMagnitude > m_minSleep){
			Profiler.BeginSample("Move Leifs");

			velocity *= m_friction;
			float velocityMagnitude = velocity.magnitude;

			if(m_isClient == true){
				Vector3 serverDeltaVector = m_ghostPosition - transform.position;
				float deltaMagnitude = serverDeltaVector.magnitude;
				if(deltaMagnitude < m_hardResyncLimit){
					float max = velocityMagnitude + deltaMagnitude;
					if(max < 1f){
						max = 1f;
					}
					float share = velocityMagnitude / max;
					
					//normalize if over 1
					Vector3 predictedDir = deltaMagnitude > 1 ? serverDeltaVector.normalized : serverDeltaVector;
					
					velocity = velocity * share + predictedDir * m_resyncForce * (1-share);
				}else{
					m_transform.position = m_ghostPosition;
				}
			}
			m_rigidbody.velocity = velocity;

			Profiler.EndSample();

			Profiler.BeginSample("Rotate Leifs");
			Quaternion prevAngle = m_transform.rotation;
			Quaternion newAngle = Quaternion.Euler(prevAngle.eulerAngles + Vector3.forward * velocityMagnitude * Time.deltaTime * m_rotationSpeed);
			m_rigidbody.MoveRotation(newAngle);
			Profiler.EndSample();
		}else if(velocity.magnitude > 0){
			m_rigidbody.velocity = Vector3.zero;
		}
	}

	public void setGhostLeafPosition(Vector3 pos){
		m_ghostPosition = pos;
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
