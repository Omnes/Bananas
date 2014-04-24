using UnityEngine;
using System.Collections;

public class scr_movementLogic : MonoBehaviour 
{
	public float m_rotateProportion = 1.0f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;
	
	public float m_maxSpeed = 10f;
	public float m_acceleration = 0.1f;
	public float m_dizzySeconds;
	public float m_powSpeed = 1f;

	[Range(0.0f, 1.0f)]
	public float m_BlowPowerSlowFraction = 1.0f;
	

	private bool m_hasCollided = false;
	private float m_Speed;
	private float right = 0.0f;
	private float left = 0.0f;
	private float m_dizzyFactor = 1.0f;
	private float m_collisionVelocity;
	private Vector3 currentVelocity;

	private float blowPower = 0.0f;
	private Vector2 m_inputVec;
	private InputHub m_touchIn = null;


	private bool m_running;
	private playerAnimation m_animation;


	// Use this for initialization
	void Start () 
	{
//		m_dizzySeconds = Mathf.Clamp (m_dizzySeconds, 1, 10);
		m_animation = GetComponent<playerAnimation>();
		m_touchIn = GetComponent<InputHub> ();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(m_hasCollided)
		{
			//collisionTime = Time.time;
			Debug.Log("Collided");
			m_dizzyFactor = 0.0f;
			m_hasCollided = false;
		}
		//After collision, increase the dizzyFactor untill it has reached one(stearing restored)..
		else
			if(m_dizzyFactor < 1.0f)
			{
				m_dizzyFactor += Time.deltaTime/m_dizzySeconds;
			}

		m_inputVec = m_touchIn.getCurrentInputVector ();
		m_inputVec *= m_dizzyFactor;
		right = m_inputVec.y;
		left = m_inputVec.x;

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


		float inputSpeed =  Mathf.Pow((right + left),m_powSpeed);

		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= m_rotateProportion * Time.deltaTime;
		transform.Rotate (temp);

		Vector3 dir = transform.forward;
		currentVelocity = rigidbody.velocity;
		Vector3 newVelocity = dir;

		//adding som velocity
		newVelocity = Vector3.Project (currentVelocity, dir.normalized);

		//add the new speed
		newVelocity += dir * m_acceleration * inputSpeed * Time.deltaTime;

		//Backward force
//		Vector3 backwardForce = -dir * blowPower * m_BlowPowerForce;
//		newVelocity += backwardForce * Time.deltaTime;
		//clamp speed
		if(newVelocity.magnitude > m_maxSpeed)
		{
			newVelocity = newVelocity.normalized*m_maxSpeed;
		}

		newVelocity = (1 - blowPower) > 0.5?newVelocity:newVelocity * m_BlowPowerSlowFraction;

		//friction if there 
		if(Mathf.Abs(inputSpeed) < m_minimumSpeed && !m_hasCollided)
		{
			newVelocity *= m_frictionProportion;
		}


		//Setting the velocity for collisiondetection..
		m_collisionVelocity = rigidbody.velocity.sqrMagnitude;

		//set the speed to newVelocity
		Vector3 deltaVelocity = newVelocity - currentVelocity;
		rigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
	}

	void OnGUI(){
		GUI.Label(new Rect(Screen.width/2-200,100,200,50),"Dizzyfact " + m_dizzyFactor.ToString("F2"));
	}

	public void restoreMovement()
	{
		m_hasCollided = false;
	}

	public float getRigidVelocity()
	{
		return m_collisionVelocity;
	}

	public void setTackled()
	{
		m_hasCollided = true;
	}


}
