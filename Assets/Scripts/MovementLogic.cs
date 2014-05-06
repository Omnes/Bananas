using UnityEngine;
using System.Collections;

public class MovementLogic : MonoBehaviour 
{
	public float m_rotateProportion = 60f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;

	public float m_dizzySeconds = 2.0f;

	public float m_maxSpeed = 8f;
	public float m_acceleration = 8f;

	public float m_powSpeed = 1f;
	public float tmpSPeed = 0.1f;
//	public string name = "";

	[Range(0.0f, 1.0f)]
	public float m_BlowPowerSlowFraction = 1.0f; 

	public Vector3 m_currentRotationSpeed = Vector3.zero;

	[Range(0.0f, 1.0f)]
	public float m_BlowPowerSlowWhileTurning = 1.0f;

	private bool m_hasCollided = false;
	private float m_Speed;
	private float right = 0.0f;
	private float left = 0.0f;
	private float m_dizzyFactor = 1.0f;
	private float m_collisionVelocity;
	private Vector3 currentVelocity;


	//Test stuff
	Vector3 testCollisionVect;

	private Vector3 resultOfCollision;
	private float collisionScale;


	private float blowPower = 0.0f;
	private Vector2 m_inputVec;
	private InputHub m_touchIn = null;


	private bool m_running;
	private playerAnimation m_animation;

	private FMOD_StudioEventEmitter footstepEmitter;
//	FMOD.Studio.ParameterInstance footstepParam;


	
	//seans crazy countdown
	public BuffManager m_buffManager;


	// Use this for initialization
	void Start () 
	{
//		m_dizzySeconds = Mathf.Clamp (m_dizzySeconds, 1, 10);
		m_animation = GetComponent<playerAnimation>();
		m_touchIn = GetComponent<InputHub> ();
		footstepEmitter = GetComponent<FMOD_StudioEventEmitter> ();
		footstepEmitter.StartEvent ();
		footstepEmitter.evt.setVolume (0);
//		footstepEmitter.evt.getParameter ("Snabbet", out footstepParam);
//		footstepParam.setValue (2.0f);

		if(m_dizzySeconds  < 0.01f){
			Debug.LogError("m_dizzySeconds can't be 0, this will crash the game on collisions!");
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		//buffamanager. add buff new Buff(STUNTUN);
		if(m_buffManager == null){
			m_buffManager = gameObject.GetComponent<BuffManager>();
			//stun tre sec, countdown before match
			m_buffManager.Add(new StunBuff(gameObject, 3));
		}

		if(m_hasCollided)
		{
			//collisionTime = Time.time;
//			Debug.Log("Collided");
//			Debug.Log("result" + resultOfCollision.ToString("F2"));
//			rigidbody.AddForce(resultOfCollision * 2, ForceMode.VelocityChange);
			m_dizzyFactor = 0.0f;
//			tmpSPeed = 0.0f;
			m_hasCollided = false;
		}
		//After collision, increase the dizzyFactor untill it has reached one(stearing restored)..
		else {
			if(m_dizzyFactor < 1.0f)
			{
				m_dizzyFactor += Time.deltaTime/m_dizzySeconds;
			}
		}


		Profiler.BeginSample("Input");
		m_inputVec = m_touchIn.getCurrentInputVector ();
		m_inputVec *= m_dizzyFactor;
		right = m_inputVec.y;
		left = m_inputVec.x;

//		footstepEmitter.audio.volume = (Mathf.Abs(right) + Mathf.Abs(left)) / 2;
//		Debug.Log ("Emitter: " + footstepEmitter.audio);

		float totalSpeed = (Mathf.Abs (right) + Mathf.Abs (left)) / 2;
		footstepEmitter.evt.setVolume (totalSpeed);
//		totalSpeed *= 10;
//		Debug.Log (totalSpeed);
//		footstepParam.setValue (totalSpeed);


//		totalSpeed *= 10;
//		Debug.Log (totalSpeed);
//		footstepParam.setValue (totalSpeed);

		blowPower = m_touchIn.getCurrentBlowingPower ();
		Profiler.EndSample();

		Profiler.BeginSample("Animation");
		//animationkode och stuff
		if(right+left > 0){
			if(m_running == false){
				m_running = true;
				m_animation.runningAnim();
			}
		}else if(m_running == true){
			m_running = false;
			m_animation.idleAnim();
		}
		Profiler.EndSample();



		//Adding rotation..
		Profiler.BeginSample("Rotation");
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		m_currentRotationSpeed = temp * m_rotateProportion;
		Quaternion prev = transform.rotation;
		Quaternion newRot = Quaternion.Euler(prev.eulerAngles + m_currentRotationSpeed * Time.deltaTime);
		rigidbody.MoveRotation (newRot);
		Profiler.EndSample();

		Vector3 dir = transform.forward;
		currentVelocity = rigidbody.velocity;
		Vector3 newVelocity = dir;

		Profiler.BeginSample("Velocity");
		float inputSpeed =  Mathf.Pow((right + left),m_powSpeed);
		//adding som velocity
		newVelocity = Vector3.Project (currentVelocity, dir.normalized);

		//add the new speed
		newVelocity += dir * m_acceleration * inputSpeed * Time.deltaTime;
		//Debug.Log("DIR "+dir);

		//Backward force
//		Vector3 backwardForce = -dir * blowPower * m_BlowPowerForce;
//		newVelocity += backwardForce * Time.deltaTime;

		//clamp speed
		if(newVelocity.magnitude > m_maxSpeed)
		{
			newVelocity = newVelocity.normalized*m_maxSpeed;
		}

		//turning
		if(Mathf.Abs(right - left) != 0){
			newVelocity = (1 - blowPower) > 0.5?newVelocity:newVelocity * m_BlowPowerSlowWhileTurning;
		}else{	//go straight
			newVelocity = (1 - blowPower) > 0.5?newVelocity:newVelocity * m_BlowPowerSlowFraction;
		}

		//friction if there 
		if(m_dizzyFactor < 1.0f || Mathf.Abs(inputSpeed) < m_minimumSpeed && !m_hasCollided)
		{
			newVelocity *= m_frictionProportion;
		}


		//Setting the velocity for collisiondetection..
		m_collisionVelocity = rigidbody.velocity.sqrMagnitude;
		testCollisionVect = rigidbody.velocity;

		//set the speed to newVelocity
		Vector3 deltaVelocity = newVelocity - currentVelocity;
		deltaVelocity += resultOfCollision;
//		Debug.Log (name + " DeltaVelocity :" + deltaVelocity.ToString ("F2"));
//		Debug.Log (name + " NewVelocity :" + newVelocity.ToString ("F2"));
//		Debug.Log (name + " currentVelocity :" + currentVelocity.ToString ("F2"));
//		Debug.Log (name + " RigidbodysVelocity :" + rigidbody.velocity.ToString ("F2"));
//
//		if(tmpSPeed > 0.0f)
//		{
//			rigidbody.AddForce(0.0f, 0.0f, 0.1f, ForceMode.VelocityChange);
//		}
//		else
		rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
		Profiler.EndSample();
	}

	public float getRotationSpeed(){
		return m_currentRotationSpeed.y;
	}

	public void restoreMovement()
	{
//		m_hasCollided = false;
		resultOfCollision = Vector3.zero;
	}

	public float getRigidVelocity()
	{
		return m_collisionVelocity;
	}

	public Vector3 getRigidVelVect ()
	{
		return testCollisionVect;
	}

	public void setTackled(Vector3 aCollisionForce)
	{
		resultOfCollision = aCollisionForce;
		m_hasCollided = true;
	}

}
