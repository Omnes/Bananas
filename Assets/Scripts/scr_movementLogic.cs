﻿using UnityEngine;
using System.Collections;

public class scr_movementLogic : MonoBehaviour 
{
	public float m_speedProportion = 50.0f;
	public float m_rotateProportion = 1.0f;
	public float m_frictionProportion = 0.95f;
	public float m_minimumSpeed = 0.01f;

	public float m_powSpeed = 1f;


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
		if (Mathf.Abs(left + right) < m_minimumSpeed) 
		{
			rigidbody.velocity *= m_frictionProportion;
		}

		m_speed =  Mathf.Pow((right + left),m_powSpeed)/m_speedProportion;

		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= m_rotateProportion;
		transform.Rotate (temp);


		rigidbody.AddForce (transform.forward * m_speed, ForceMode.VelocityChange);

	}

}