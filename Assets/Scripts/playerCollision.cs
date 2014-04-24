using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour {

	public float m_looserColForce;
	public scr_movementLogic m_movementLogic;
	public float m_dizzyTime = 6f;
	public float e = 1.0f;

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			scr_movementLogic othersMovement = other.gameObject.GetComponent<scr_movementLogic>();
			//Get some info about the colliders..
			Vector3 myForwardForce = rigidbody.transform.forward;
			Vector3 othersForwardForce = other.rigidbody.transform.forward;
			//the normal .. 
			Vector3 v = other.transform.position - this.transform.position;


			//Check if the "other player" has higher speed .. 
			if (othersMovement.getRigidVelocity() > m_movementLogic.getRigidVelocity()) //if other speed is higher than mine
			{
//				Debug.Log("Player2: " + othersMovement.getVelocity().ToString("F2"));
//				Debug.Log("You: " + m_movementLogic.getVelocity().ToString("F2"));



				//Angles from "this" rigidbodys force to the normal.. 
				float myAngle = Vector3.Angle(myForwardForce, v);
				//Angles from "other" rigidbodys force to the normal.. 
				float otherAngle = Vector3.Angle(othersForwardForce, v);

				//forces in the two planes for "this" rigidbody before
				Vector3 myTvec = - (Mathf.Sin(myAngle) * myForwardForce);
				Vector3 myNvecBefore = - (Mathf.Cos(myAngle) * myForwardForce);

				//forces in the two planes for "other" rigidbody before
				Vector3 otherTvec = Mathf.Sin(otherAngle) * othersForwardForce;
				Vector3 otherNvecBefore = Mathf.Cos(otherAngle) * othersForwardForce;

				//physical bullshit .. 
				Vector3 myNvecAfter = otherNvecBefore;
				Vector3 myTvecAfter = myTvec;

				Vector3 myResultVec = myNvecAfter + myTvecAfter;

				//---- || ----
				Vector3 otherNvecAfter = myNvecBefore;
				Vector3 otherTvecAfter = otherTvec;

				Vector3 otherResultVec = otherNvecAfter + otherTvecAfter;


				this.m_movementLogic.setTackled();

				rigidbody.AddForce(rigidbody.transform.forward + myResultVec * m_looserColForce , ForceMode.Impulse);

//				m_movementLogic.Invoke("restoreMovement", 3);
				//If so make YOUR character "slidable" for 6 sec..

//				rigidbody.AddExplosionForce(20.0f, other.rigidbody.position, 10.0f);
//				other.rigidbody.AddExplosionForce(7.0f, rigidbody.position, 10.0f);
			}
			else
			{
//				other.rigidbody.AddExplosionForce(20.0f, rigidbody.position, 10.0f);
//				rigidbody.AddExplosionForce(7.0f, other.rigidbody.position, 10.0f);
//
				scr_soundManager.Instance.playOneShot( "event:/Knockout! (1)", other.transform.position );
			}
		}
	}
}
