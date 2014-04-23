using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	public MovementLogic m_movementLogic;
	public float m_dizzyTime = 6f;

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
		//Check if the "other player" has higher speed .. 
			if (other.rigidbody.velocity.sqrMagnitude > rigidbody.velocity.sqrMagnitude) //if other speed is higher than mine
			{
				//If so make YOUR character "slidable" for 6 sec..
				m_movementLogic.setTackled();
				m_movementLogic.Invoke ("restoreMovement", m_dizzyTime);

				SoundManager.Instance.playOneShot( "event:/Knockout! (1)", other.transform.position );
			}
		}
	}
}
