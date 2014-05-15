using UnityEngine;
using System.Collections;

public class BouncePlayer : MonoBehaviour {

	public float m_bounceForce = 1f;

	public Mode m_bounceMode = Mode.Realistic;

	public ForceMode m_forceMode = ForceMode.Impulse;

	public enum Mode {Realistic,Simple};

	void OnCollisionEnter(Collision other){
		if(other.gameObject.CompareTag("Player")){
			Vector3 otherVelocity = other.rigidbody.velocity;
//			Vector3 otherVelocity = other.gameObject.GetComponent<MovementLogic>().getRigidVelocity();
			
			Vector3 normal = transform.forward.normalized;

			if(m_bounceMode == Mode.Realistic){
				Vector3 reflectedVector = Vector3.Reflect(otherVelocity,normal);
//				Vector3 reflectedVector = otherVelocity - 2 * (Vector3.Dot(otherVelocity,normal)*normal);

				other.rigidbody.AddForce(reflectedVector * m_bounceForce,m_forceMode);
			}else if(m_bounceMode == Mode.Simple){
				other.rigidbody.AddForce(normal * m_bounceForce,m_forceMode);
			}
		}
	}
}
