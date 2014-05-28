using UnityEngine;
using System.Collections;

public class GlideAgainstWall : MonoBehaviour {
	private float m_speed = 2.5f;

	void OnCollisionStay(Collision other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			GameObject player = other.gameObject;

			Vector3 curDir = player.transform.forward;
			Vector3 newDir = Vector3.Angle(curDir, transform.right) < Vector3.Angle(curDir, -transform.right) ? transform.right : -transform.right;

			float speedMultiplier = Mathf.Min(Vector3.Angle(curDir, transform.right), Vector3.Angle(curDir, -transform.right)) / 90;
//			speedMultiplier = 1 - speedMultiplier; 
//			Debug.Log("Angle: " + speedMultiplier);

//			newDir += transform.forward;
			Vector3 finalForce = newDir * m_speed * speedMultiplier;
			finalForce += transform.forward + Vector3.one * (1f - speedMultiplier);
			player.rigidbody.AddForce(finalForce, ForceMode.VelocityChange);
		}
	}
}
