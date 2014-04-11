using UnityEngine;
using System.Collections;

public class scr_movementLogic : MonoBehaviour 
{
	//public float m_speedProportion = 50.0f;
	public float m_rotateProportion = 1.0f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;

	public float m_maxSpeed = 10f;
	public float m_acceleration = 0.1f;

	public float m_powSpeed = 1f;
	public Vector3 drag = new Vector3();

	private float right = 0.0f;
	private float left = 0.0f;
	private float m_speed;
	private Vector3 m_finalVelocity;
	private Vector2 m_inputVec;
	private scr_touchInput m_touchIn = null;

	//Debug.. 
	Rect m_leftArea = new Rect ();
	Rect m_rightArea = new Rect ();

	// Use this for initialization
	void Start () 
	{
		//testVec = new Vector2 (1.0f, 1.0f);
		m_leftArea = new Rect (0,0, 30, 15);
		m_rightArea = new Rect (40,0, 30, 15);
		m_touchIn = GetComponent<scr_touchInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_inputVec = m_touchIn.getCurrentInputVector ();
		right = m_inputVec.y;
		left = m_inputVec.x;

		//friction when no input
		//if (Mathf.Abs(left + right) < m_minimumSpeed) 
		//{
		//	rigidbody.velocity *= m_frictionProportion;
		//}

		m_speed =  Mathf.Pow((right + left),m_powSpeed);///m_speedProportion;

		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= m_rotateProportion;
		transform.Rotate (temp);

		//Projecting the velocity of the RB to the current(new) direction, this prevents the RB from sliding while turning.. 
		Vector3 dir = transform.forward;
		//rigidbody.velocity = Vector3.Project (rigidbody.velocity, dir.normalized);


		//rigidbody.AddForce (dir * m_speed, ForceMode.VelocityChange);

		Vector3 currentVelocity = rigidbody.velocity;
		Vector3 newVelocity = Vector3.Project (currentVelocity, dir.normalized);
		newVelocity += dir*m_acceleration * m_speed*Time.deltaTime;
		if(newVelocity.magnitude > m_maxSpeed){
			newVelocity = newVelocity.normalized*m_maxSpeed;
			//newVelocity = Vector3.zero;
		}

		if(Mathf.Abs(m_speed) < m_minimumSpeed){
			newVelocity *= m_frictionProportion;
		}


		rigidbody.AddForce(newVelocity-currentVelocity,ForceMode.VelocityChange);

	}

}
