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

	public enum State{Following,Idle,MovingTowards}
	private State m_state;

	void Awake(){
		m_state = State.Idle;
//		transform.parent = null;

	}

	void FixedUpdate () {
		if(m_state == State.Following){
			//the internal offset is used to always place the camera behind the player

			//made an attempt at a smoother camera, it failed. leaving this here for future generations to laugh and learn from my mistakes
			if(m_DebugSnapCamera == false){
				Vector3 predictedPosition = m_target.position + m_targetRigidbody.velocity * m_predictionMagnitude;
				Vector3 target = (m_predict == true) ?  predictedPosition : m_target.position;

				Vector3 xzmask = new Vector3(1,0,1);
				Vector3 dir = ((target + m_internalOffset)- transform.position);
//				dir.Scale(xzmask); //mask away the y so the camera wont move in y
				if(dir.magnitude > 1){
					dir.Normalize();
				}

				transform.position += dir * m_speed * Time.deltaTime;

			} else if(m_DebugSnapCamera){
				transform.position = m_target.position + m_internalOffset;
			}	
		}
	}

	public void MoveToPosition(Vector3 pos,Quaternion rot,float duration){
		StartCoroutine(LerpTo(pos,rot,duration));
	}

	private IEnumerator LerpTo(Vector3 pos,Quaternion rot,float duration){
		m_state = State.MovingTowards;
		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;
		float startTime = Time.time;
		while(Time.time < startTime + duration){
			float t = (Time.time - startTime)/duration;
			transform.position = Vector3.Lerp(startPosition,pos,t);
			transform.rotation = Quaternion.Slerp(startRotation,rot,t);
			yield return null;
		}
		m_state = State.Idle;
	}

	public void SetTarget(Transform target){
		m_target = target;
		m_state = State.Following;

		m_targetRigidbody = target.rigidbody;
		Vector3 rot = transform.localEulerAngles;
		rot.y = target.localEulerAngles.y;
		transform.rotation = Quaternion.Euler(rot);

		m_internalOffset = m_offset;
		m_internalOffset.x = transform.forward.x * m_offset.x;
		m_internalOffset.z = transform.forward.z * m_offset.z;

		transform.position  = target.position + m_internalOffset;
	}
	
}
