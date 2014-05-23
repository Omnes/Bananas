using UnityEngine;
using System.Collections;

public class otherTestCol : MonoBehaviour 
{
	private CollisionTransmitter m_collisionTransmitter;
	private playerAnimation m_playerAnim;
	private BuffManager m_buffManager;
	private MovementLogic m_myMovLogic;
	private MovementLogic m_otherMovLogic;

	public float m_oppAngleMinusValue = 10.0f;
	public float m_stunTime = 0.3f;
	public float m_dizzyTime = 2.0f;
	public float m_tackledTime = 0.5f;
	private float m_cooldownTimer = 0.0f;
	private const float COOLDOWN = 0.5f;

	public float m_speedThreshold = 3.0f;

	/**
	 * Initialize components
	 */
	void Start()
	{
		m_playerAnim = GetComponent<playerAnimation>();
		m_buffManager = GetComponent<BuffManager> ();
	}

	void Update()
	{
		m_cooldownTimer += Time.deltaTime;
	}

	void OnCollisionEnter(Collision other)
	{
		if (m_cooldownTimer > COOLDOWN)
		{
			if(other.gameObject.CompareTag("Player"))
			{
				m_cooldownTimer = 0;
				SoundManager.Instance.play(SoundManager.KNOCKOUT);

				if(Network.isServer)
				{
					//Get components
					otherTestCol opponent = other.transform.GetComponent<otherTestCol>();
					m_otherMovLogic = other.gameObject.GetComponent<MovementLogic>();
					m_myMovLogic = gameObject.GetComponent<MovementLogic> ();
			
					//The centerline between the two collision circles
					Vector3 cLine = other.transform.position - transform.position;
					cLine.Normalize();
					
					//Calculate the angle between the players
					float myAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, transform.forward));
					float oppAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, opponent.transform.forward));

					//############################################
					//Play tackle animation
					m_playerAnim.tackleAnim(m_dizzyTime);
					
					//						cLine.Normalize();
					
					//Calculate other players velocity
					Vector3 othersVel = m_otherMovLogic.getRigidVelVect();
					float x1 = Vector3.Dot(cLine, othersVel);
					
					Vector3 othersXvel = cLine * x1;
					Vector3 othersYvel = othersVel 	- othersXvel;
					
					cLine = -cLine;
					
					//Calculate local players velocity
					Vector3 myVel = m_myMovLogic.getRigidVelVect();
					float x2 = Vector3.Dot(cLine, myVel);
					
					Vector3 myXvel = cLine * x2;
					Vector3 myYVel = myVel - myXvel;
					
					Vector3 myResultVel = myYVel + othersXvel;
					Vector3 oppResultVel = myXvel + othersYvel;

					//###
					m_myMovLogic.setTackled(myResultVel);
					StartCoroutine("startTackle", m_tackledTime);
					//############################################

					if (m_otherMovLogic.getRigidVelocity() > (m_myMovLogic.getRigidVelocity()) &&
					    Mathf.Abs(m_otherMovLogic.getRigidVelocity() - m_myMovLogic.getRigidVelocity()) > m_speedThreshold)
					{
						//Add buffs
						m_buffManager.AddBuff(new StunBuff(gameObject, m_stunTime));
						m_buffManager.AddBuff(new DizzyBuff(gameObject, m_dizzyTime));

//						m_collisionTransmitter.PlayerCollision();
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

	IEnumerator startTackle(float tackledTime) {
		yield return new WaitForSeconds(tackledTime);
		m_myMovLogic.restoreMovement();
	}

	public CollisionTransmitter collisionTransmitter {
		set {
			m_collisionTransmitter = value;
			m_collisionTransmitter.m_playerRef = gameObject;
		}
		get {
			return m_collisionTransmitter;
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
}
