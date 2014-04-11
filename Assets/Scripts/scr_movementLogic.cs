using UnityEngine;
using System.Collections;

public class scr_movementLogic : MonoBehaviour 
{
	public float m_rotateProportion = 1.0f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;
	
	public float m_maxSpeed = 10f;
	public float m_acceleration = 0.1f;

	public float m_powSpeed = 1f;
	
	private bool m_hasCollided = false;

	private float right = 0.0f;
	private float left = 0.0f;
	private Vector2 m_inputVec;
	private scr_touchInput m_touchIn = null;


	// Use this for initialization
	void Start () 
	{
		m_touchIn = GetComponent<scr_touchInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_inputVec = m_touchIn.getCurrentInputVector ();
		right = m_inputVec.y;
		left = m_inputVec.x;


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
		newVelocity += dir*m_acceleration * inputSpeed * Time.deltaTime;
		//clamp speed
		if(newVelocity.magnitude > m_maxSpeed){
			newVelocity = newVelocity.normalized*m_maxSpeed;
		}
		//friction if there 
		if(Mathf.Abs(inputSpeed) < m_minimumSpeed && !m_hasCollided){
			newVelocity *= m_frictionProportion;
		}

		//set the speed to newVelocity
		rigidbody.AddForce(newVelocity-currentVelocity,ForceMode.VelocityChange);

	
	}

	public void restoreMovement()
	{
		m_hasCollided = false;
	}

	public void setTackled(){
		m_hasCollided = true;
	}


}
