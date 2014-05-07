using UnityEngine;
using System.Collections;

public class otherTestCol : MonoBehaviour 
{
	public float dizzyTime = 1.0f;

	private MovementLogic m_movementLogic;
	private playerAnimation m_playerAnim;
	private BuffManager m_buffManager;

	/**
	 * Initialize components
	 */
	void Start()
	{
		m_movementLogic = GetComponent<MovementLogic> ();
		m_playerAnim = GetComponent<playerAnimation>();
		m_buffManager = GetComponent<BuffManager> ();
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			MovementLogic othersMovementLogic = other.gameObject.GetComponent<MovementLogic>();
			if(othersMovementLogic.getRigidVelocity() > m_movementLogic.getRigidVelocity())
			{
				//Play tackle animation
				m_playerAnim.tackleAnim(/*dizzyTime*/);

				//Calculate tackle physics
				Vector3 basisVector = other.transform.position - transform.position;
				basisVector.Normalize();

				Vector3 othersVel = othersMovementLogic.getRigidVelVect();
				float x1 = Vector3.Dot(basisVector, othersVel);

				Vector3 othersXvel = basisVector * x1;
				Vector3 othersYvel = othersVel - othersXvel;

				basisVector = -basisVector;
				Vector3 myVel = m_movementLogic.getRigidVelVect();
				float x2 = Vector3.Dot(basisVector, myVel);

				Vector3 myXvel = basisVector * x2;
				Vector3 myYVel = myVel - myXvel;

				Vector3 opponentsResultVel = myXvel + othersYvel;
				Vector3 myResultVel = othersXvel + myYVel;

				othersMovementLogic.setTackled(opponentsResultVel * 0.3f);
				m_movementLogic.setTackled(myResultVel);

				m_movementLogic.Invoke("restoreMovement", dizzyTime);
				othersMovementLogic.Invoke("restoreMovement", dizzyTime);
			}

			//Handle TimeBomb powerup
			if (m_buffManager.HasBuff(typeof(TimeBombBuff))){
				TimeBombBuff timeBombBuff = m_buffManager.GetBuff(typeof(TimeBombBuff)) as TimeBombBuff;
				if (timeBombBuff.CanTransfer()) {
					BuffManager othersBuffManager = other.gameObject.GetComponent<BuffManager> ();
					TimeBombBuff newTimeBombBuff = othersBuffManager.AddBuff(new TimeBombBuff(other.gameObject, timeBombBuff.m_duration)) as TimeBombBuff;
					newTimeBombBuff.TransferUpdate(timeBombBuff.m_durationTimer);
					m_buffManager.RemoveBuff(timeBombBuff);
				}
			}

		}
	}
}
