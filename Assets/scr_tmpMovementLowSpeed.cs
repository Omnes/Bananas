using UnityEngine;
using System.Collections;

public class scr_tmpMovementLowSpeed : MonoBehaviour 
{
	public float m_accelerationProportion;
	public float m_rotateProportion = 1.0f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;
	public float m_maxSpeed = 1.0f;

	private Vector3 direction = new Vector3();
	private bool hasCollided = false;
	private float right = 0.0f;
	private float left = 0.0f;
	private float m_speed;
	private Vector3 m_finalVelocity;
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
		
		//friction when no input
		if (Mathf.Abs(left + right) < m_minimumSpeed) 
		{
			rigidbody.velocity *= m_frictionProportion;
		}
		
		m_speed = (right + left)/m_accelerationProportion;
		
		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= m_rotateProportion;
		transform.Rotate (temp);
		
		
		//Adding a maximun speed to the character..
		rigidbody.velocity = Vector3.ClampMagnitude (rigidbody.velocity, m_maxSpeed);
		
		//Projecting the velocity of the RB to the current(new) direction, this prevents the RB from sliding while turning..
		if (hasCollided == false) 
		{
			direction = transform.forward;
			rigidbody.velocity = Vector3.Project (rigidbody.velocity, direction.normalized);
		}
		
		direction *= m_speed;
		rigidbody.AddForce (direction, ForceMode.VelocityChange);
		
	}
	void OnCollisionEnter()
	{
	}
}
