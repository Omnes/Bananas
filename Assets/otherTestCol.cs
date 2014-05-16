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
			otherTestCol opponent = other.transform.GetComponent<otherTestCol>();

			if(other.gameObject.tag == "Player")
			{
				SoundManager.Instance.playOneShot (SoundManager.KNOCKOUT);

				//The centerline between the two "circles" .. 
				Vector3 cLine = other.transform.position - transform.position;
				
				//Info about "my" player..
				Vector3 myForwardVec = transform.forward;
				float myAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, myForwardVec));
				
				//Info about "opponent" player..
				Vector3 oppForwardVec = opponent.transform.forward;
				float oppAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, oppForwardVec));

				if(opponent.getRigidVelocity() > getRigidVelocity() || myAngleToCenter > 45.0f || myAngleToCenter > (oppAngleToCenter - oppAngleMinusValue))
				{
					//Play tackle animation
					m_playerAnim.tackleAnim(dizzyTime);
					cLine.Normalize();

					Vector3 othersVel = opponent.getPreviosVelocity();
					float x1 = Vector3.Dot(cLine, othersVel);

					Vector3 othersXvel = cLine * x1;
					Vector3 othersYvel = othersVel 	- othersXvel;

					cLine *= -1.0f;
					Vector3 myVel = getPreviosVelocity();
					float x2 = Vector3.Dot(cLine, myVel);

					Vector3 myXvel = cLine * x2;
					Vector3 myYVel = myVel - myXvel;
					Vector3 opponentsResultVel = myXvel + othersYvel;
					Vector3 myResultVel = othersXvel + myYVel;

					//Add buffs
					m_buffManager.AddBuff(new StunBuff(gameObject, stunTime));
					m_buffManager.AddBuff(new DizzyBuff(gameObject, dizzyTime));
//					other.gameObject.GetComponent<BuffManager>().AddBuff(new StunBuff(gameObject, stunTime));
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

	//moved here from movementlogic
	public Vector3 getPreviosVelocity(){
		return rigidbody.velocity;
	}

	public float getRigidVelocity(){
		return rigidbody.velocity.sqrMagnitude;
	}

}
