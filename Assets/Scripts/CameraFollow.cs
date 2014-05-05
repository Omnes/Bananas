using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform m_target;
	public Vector3 m_offset = new Vector3();


	public float m_speed = 5f;
	public bool m_predict = true;
	public float m_predictionMagnitude = 0.3f;

	public bool m_DebugSnapCamera = false;
	private Rigidbody m_targetRigidbody;

	private Vector3 m_internalOffset = new Vector3();

	void Start(){
		transform.parent = null;
	}

	void FixedUpdate () {
		if(m_target != null){
			//the internal offset is used to always place the camera behind the player
			m_internalOffset = m_offset;
			m_internalOffset.x = transform.forward.x * m_offset.x;
			m_internalOffset.z = transform.forward.z * m_offset.z;

			//made an attempt at a smoother camera, it failed. leaving this here for future generations to laugh and learn from my mistakes
			if(m_DebugSnapCamera == false){
				Vector3 predictedPosition = m_target.position + m_targetRigidbody.velocity * m_predictionMagnitude;
				Vector3 target = (m_predict == true) ?  predictedPosition : m_target.position;

				Vector3 xzmask = new Vector3(1,0,1);
				Vector3 dir = ((target + m_internalOffset)- transform.position);
				dir.Scale(xzmask); //mask away the y so the camera wont move in y
				if(dir.magnitude > 1){
					dir.Normalize();
				}

			transform.position += dir * m_speed * Time.deltaTime;
			} else if(m_DebugSnapCamera){
				transform.position = m_target.position + m_internalOffset;
			}
			
		}

	}

	public void SetTarget(Transform target){
		m_target = target;
		m_targetRigidbody = target.rigidbody;
		Vector3 rot = transform.localEulerAngles;
		rot.y = target.localEulerAngles.y;
		transform.rotation = Quaternion.Euler(rot);

	}
	
}
