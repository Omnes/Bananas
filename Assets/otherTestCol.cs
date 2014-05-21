using UnityEngine;
using System.Collections;

public class otherTestCol : MonoBehaviour 
{
	public float stunTime = 0.3f;
	public float dizzyTime = 2.0f;

	public float oppAngleMinusValue = 10.0f;

	private MovementLogic m_movementLogic;
	private playerAnimation m_playerAnim;
	private BuffManager m_buffManager;

	MovementLogic m_otherMovLogic = null;
	MovementLogic m_myMovLogic = null;

	//public bool m_tackled = false;

	private float m_cooldownTimer = 0.0f;
	private const float COOLDOWN = 0.5f;

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

	void Update() {
		m_cooldownTimer += Time.deltaTime;
	}

	void OnCollisionEnter(Collision other)
	{
		if (m_cooldownTimer > COOLDOWN) {

			if(other.gameObject.CompareTag("Player")){
				if(Network.isServer){
					otherTestCol opponent = other.transform.GetComponent<otherTestCol>();

					m_otherMovLogic = other.gameObject.GetComponent<MovementLogic>();
					m_myMovLogic = gameObject.GetComponent<MovementLogic> ();
				
			
					//The centerline between the two "circles" .. 
					Vector3 cLine = other.transform.position - transform.position;

					//Info about "my" player..
					Vector3 myForwardVec = transform.forward;
					float myAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, myForwardVec));
					
					//Info about "opponent" player..
					Vector3 oppForwardVec = opponent.transform.forward;
					float oppAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, oppForwardVec));


					if(m_otherMovLogic.getRigidVelocity() > m_myMovLogic.getRigidVelocity() || myAngleToCenter > 45.0f || myAngleToCenter > (oppAngleToCenter - oppAngleMinusValue))
					{
						m_cooldownTimer = 0;
						//Play tackle animation
						m_playerAnim.tackleAnim(dizzyTime);

						cLine.Normalize();

						//				MOVEMTNLOGIC
					

						Vector3 othersVel = m_otherMovLogic.getRigidVelVect();
						float x1 = Vector3.Dot(cLine, othersVel);

						Vector3 othersXvel = cLine * x1;
						Vector3 othersYvel = othersVel 	- othersXvel;

						cLine *= -1.0f;

						//			MOVEMENTLOGIC
						Vector3 myVel = m_myMovLogic.getRigidVelVect();
						float x2 = Vector3.Dot(cLine, myVel);

						Vector3 myXvel = cLine * x2;
						Vector3 myYVel = myVel - myXvel;

						Vector3 opponentsResultVel = myXvel + othersYvel;
						Vector3 myResultVel = othersXvel + myYVel;

						m_myMovLogic.setTackled(myResultVel);
						m_otherMovLogic.setTackled(opponentsResultVel);
						StartCoroutine("startTackle", dizzyTime);
						//m_tackled = true;

						//Add buffs
						m_buffManager.AddBuff(new StunBuff(gameObject, stunTime));
						m_buffManager.AddBuff(new DizzyBuff(gameObject, dizzyTime));
		//				other.gameObject.GetComponent<BuffManager>().AddBuff(new StunBuff(gameObject, stunTime));
					}
				}


				//Handle TimeBomb powerup
				if (m_buffManager.HasBuff((int)Buff.Type.TIME_BOMB)){
					TimeBombBuff timeBombBuff = m_buffManager.GetBuff((int)Buff.Type.TIME_BOMB) as TimeBombBuff;
					if (timeBombBuff.CanTransfer()) {
						BuffManager othersBuffManager = other.gameObject.GetComponent<BuffManager> ();
						TimeBombBuff newTimeBombBuff = othersBuffManager.AddBuff(new TimeBombBuff(other.gameObject, timeBombBuff.m_duration)) as TimeBombBuff;
						newTimeBombBuff.TransferUpdate(timeBombBuff.m_durationTimer);
						m_buffManager.RemoveBuff((int)Buff.Type.TIME_BOMB);
					}
				}
			}
		}
	}

	//moved here from movementlogic
//	public Vector3 getPreviosVelocity(){
//		return rigidbody.velocity;
//	}
//
//	public float getRigidVelocity(){
//		return rigidbody.velocity.sqrMagnitude;
//	}

	IEnumerator startTackle(float dizzyTime) {
		yield return new WaitForSeconds(dizzyTime);

		Debug.Log("me "+m_myMovLogic);
		Debug.Log("other "+m_otherMovLogic);

		m_myMovLogic.restoreMovement();
		m_otherMovLogic.restoreMovement();
	//	m_tackled = false;
	}
}
