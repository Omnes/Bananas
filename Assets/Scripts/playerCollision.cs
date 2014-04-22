using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour {

	public scr_movementLogic m_movementLogic;
	public float m_dizzyTime = 6f;

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			//Check if the "other player" has higher speed .. 
			if (other.rigidbody.velocity.magnitude > rigidbody.velocity.magnitude) //if other speed is higher than mine
			{
				//If so make YOUR character "slidable" for 6 sec..
				rigidbody.AddExplosionForce(20.0f, other.rigidbody.position, 10.0f);
				other.rigidbody.AddExplosionForce(7.0f, rigidbody.position, 10.0f);
				this.m_movementLogic.setTackled();
//				m_movementLogic.Invoke ("restoreMovement", m_dizzyTime);
//				Debug.Log("Collided");
			}
			else
			{
				other.rigidbody.AddExplosionForce(20.0f, rigidbody.position, 10.0f);
				rigidbody.AddExplosionForce(7.0f, other.rigidbody.position, 10.0f);
			}
		}
	}
}
