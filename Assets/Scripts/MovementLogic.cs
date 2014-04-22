using UnityEngine;
using System.Collections;

public class MovementLogic : MonoBehaviour 
{
	public float m_rotateProportion = 60f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;
	
	public float m_maxSpeed = 8f;
	public float m_acceleration = 8f;

	public float m_powSpeed = 1f;

	[Range(0.0f, 1.0f)]
	public float m_BlowPowerSlowFraction = 1.0f;
	
	private bool m_hasCollided = false;

	private float right = 0.0f;
	private float left = 0.0f;
	private float blowPower = 0.0f;
	private Vector2 m_inputVec;
	private InputHub m_touchIn = null;


	private bool m_running;
	private playerAnimation m_animation;

	private FMOD_StudioEventEmitter footstepEmitter;
	FMOD.Studio.ParameterInstance footstepParam;


	// Use this for initialization
	void Start () 
	{
		m_animation = GetComponent<playerAnimation>();
		m_touchIn = GetComponent<InputHub> ();
		footstepEmitter = GetComponent<FMOD_StudioEventEmitter> ();
		footstepEmitter.StartEvent ();
		footstepEmitter.evt.setVolume (0);
		footstepEmitter.evt.getParameter ("Snabbet", out footstepParam);
//		footstepParam.setValue (2.0f);
	}

	// Update is called once per frame
	void Update () 
	{
		m_inputVec = m_touchIn.getCurrentInputVector ();
		right = m_inputVec.y;
		left = m_inputVec.x;

//		footstepEmitter.audio.volume = (Mathf.Abs(right) + Mathf.Abs(left)) / 2;
//		Debug.Log ("Emitter: " + footstepEmitter.audio);

		float totalSpeed = (Mathf.Abs (right) + Mathf.Abs (left)) / 2;
		footstepEmitter.evt.setVolume (totalSpeed);
//		totalSpeed *= 10;
//		Debug.Log (totalSpeed);
//		footstepParam.setValue (totalSpeed);

		blowPower = m_touchIn.getCurrentBlowingPower ();

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


		float inputSpeed =  Mathf.Pow((right + left),m_powSpeed);///m_speedProportion;

		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= m_rotateProportion * Time.deltaTime;
		transform.Rotate (temp);

		Vector3 dir = transform.forward;
		Vector3 currentVelocity = rigidbody.velocity;
		Vector3 newVelocity = dir;

		if(!m_hasCollided){
			//project to remove slide
			newVelocity = Vector3.Project (currentVelocity, dir.normalized);
		}

		//add the new speed
		newVelocity += dir * m_acceleration * inputSpeed * Time.deltaTime;

		//Backward force
//		Vector3 backwardForce = -dir * blowPower * m_BlowPowerForce;
//		newVelocity += backwardForce * Time.deltaTime;

		//clamp speed
		if(newVelocity.magnitude > m_maxSpeed){
			newVelocity = newVelocity.normalized*m_maxSpeed;
		}

		newVelocity = (1 - blowPower) > 0.5?newVelocity:newVelocity * m_BlowPowerSlowFraction;

		//friction if there 
		if(!m_hasCollided){
			newVelocity *= 1-((1-m_frictionProportion) * (1-(Mathf.Max(Mathf.Abs(right),Mathf.Abs(left)))));
		}

		//set the speed to newVelocity
		Vector3 deltaVelocity = newVelocity - currentVelocity;
		rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
	}

	public void restoreMovement()
	{
		m_hasCollided = false;
	}

	public void setTackled(){
		m_hasCollided = true;
	}

}
