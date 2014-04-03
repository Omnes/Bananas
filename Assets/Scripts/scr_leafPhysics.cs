using UnityEngine;
using System.Collections;

public class scr_leafPhysics : MonoBehaviour {
	private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
	public float maxVelocity = float.MaxValue;

	[Range(0.0f, 1.0f)]
	public float friction = 1.0f;
	
	void Update () {
		transform.position += velocity * Time.deltaTime;
		velocity *= friction;
	}

	public void AddForce(Vector3 force)
	{
		force = new Vector3(force.x, 0, force.z);
		velocity += force;
		if (velocity.magnitude > maxVelocity) {
			velocity = velocity.normalized * maxVelocity;
		}
	}
}
