using UnityEngine;
using System.Collections;

public class otherTestCol : MonoBehaviour 
{
	MovementLogic me;
	MovementLogic opponent;

	public playerAnimation m_playerAnim;

	public float dizzyTime = 1.0f;

	void Start()
	{
		m_playerAnim = gameObject.GetComponent<playerAnimation>();
	}
	void OnCollisionEnter(Collision other)
	{
		opponent = other.gameObject.GetComponent<MovementLogic>();
		me = this.gameObject.GetComponent<MovementLogic> ();
		if(other.gameObject.tag == "Player")
		{
			if(opponent.getRigidVelocity() > me.getRigidVelocity())
			{

				//tackleanimation
				m_playerAnim.tackleAnim();

				Vector3 basisVector = other.transform.position - transform.position;
				basisVector.Normalize();

				Vector3 othersVel = opponent.getRigidVelVect();
				float x1 = Vector3.Dot(basisVector, othersVel);

				Vector3 othersXvel = basisVector * x1;
				Vector3 othersYvel = othersVel 	- othersXvel;

				basisVector *= -1.0f;
				Vector3 myVel = me.getRigidVelVect();
				float x2 = Vector3.Dot(basisVector, myVel);

				Vector3 myXvel = basisVector * x2;
				Vector3 myYVel = myVel - myXvel;


				Vector3 opponentsResultVel = myXvel + othersYvel;
				Vector3 myResultVel = othersXvel + myYVel;

				opponent.setTackled(opponentsResultVel * 0.3f);
				me.setTackled(myResultVel);


				me.Invoke("restoreMovement", dizzyTime);
				opponent.Invoke("restoreMovement", dizzyTime);

				//undo tackleanimation
				m_playerAnim.stopTackleAnim();
			}
		}
	}
}
