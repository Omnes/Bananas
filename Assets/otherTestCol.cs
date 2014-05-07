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
		//m_movementLogic = gameObject.AddComponent<MovementLogic>();
		//m_movementLogic = GetComponent<MovementLogic> ();
		m_playerAnim = GetComponent<playerAnimation>();
		m_buffManager = GetComponent<BuffManager> ();
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			otherTestCol otherPlayerTestCol = other.transform.GetComponent<otherTestCol>();

			//MovementLogic othersMovementLogic = other.gameObject.GetComponent<MovementLogic>();
			//if(othersMovementLogic.getRigidVelocity() > m_movementLogic.getRigidVelocity())
			if(otherPlayerTestCol.getRigidVelocity() > getRigidVelocity())
			{
				//Play tackle animation
				m_playerAnim.tackleAnim(dizzyTime);

				//Calculate tackle physics
				Vector3 basisVector = other.transform.position - transform.position;
				basisVector.Normalize();

				//changed othersMovementLogic.getRigidVelVect() to otherPlayerTestCol.getPreviosVelocity()
				Vector3 othersVel = otherPlayerTestCol.getPreviosVelocity();
				float x1 = Vector3.Dot(basisVector, othersVel);

				Vector3 othersXvel = basisVector * x1;
				Vector3 othersYvel = othersVel - othersXvel;

				basisVector = -basisVector;
				//changed m_movementLogic.getRigidVelVect() to getPreviosVelocity()
				Vector3 myVel = getPreviosVelocity();
				float x2 = Vector3.Dot(basisVector, myVel);

				Vector3 myXvel = basisVector * x2;
				Vector3 myYVel = myVel - myXvel;

				Vector3 opponentsResultVel = myXvel + othersYvel;
				Vector3 myResultVel = othersXvel + myYVel;


//				//detta kan tas bort när allt är klart
//				othersMovementLogic.setTackled(opponentsResultVel * 0.3f);
//				m_movementLogic.setTackled(myResultVel);
//				Invoke("restorePlayerMovement", dizzyTime);
//				otherPlayerTestCol.Invoke("restorePlayerMovement", dizzyTime);

				restorePlayerMovement(other.gameObject);

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

	//moved here from movementlogic
	public Vector3 getPreviosVelocity(){
		return rigidbody.velocity;
	}

	public float getRigidVelocity(){
		return rigidbody.velocity.sqrMagnitude;
	}

	public void restorePlayerMovement(GameObject other){
		//localplayer
		m_buffManager.AddBuff(new StunBuff(gameObject, 0.3f));
		//ghost
		other.GetComponent<BuffManager>().AddBuff(new StunBuff(gameObject, 1.0f));
	}

}
