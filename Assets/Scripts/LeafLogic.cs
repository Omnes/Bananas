﻿using UnityEngine;
using System.Collections;

public class LeafLogic : MonoBehaviour {

	public enum State {OnGround,InBlower,Drop,Pickup};
	public State m_state = State.OnGround;

	public int m_id = -1;

	public float m_baseSpeed = 1f;
	public float m_maxSpeed = 15f;
	private float m_speed = 0f;
	public float m_accleleration  = 1f;
	public float m_rotationSpeed = 80f;

	private bool m_move = false;
	private Vector3 m_endPosition;
	private Transform m_toBeParent;
	private Transform m_originalParent;


	public float m_rotationModifierRange = 2f; 
	private float m_rotationModifier;

	private Transform m_transform;
	private float m_startYPos = 0f;
	
	void Start () {
		m_rotationModifier = Random.Range (-m_rotationModifierRange,m_rotationModifierRange);
		m_transform = transform;
		m_startYPos = m_transform.localPosition.y;
		m_originalParent = m_transform.parent;
	}

	void Update () {
		//change to inblower when it reaches the destination
		if(m_state == State.Pickup){
			Vector3 endPos = m_toBeParent.TransformPoint(m_endPosition);
			endPos.y = m_startYPos;
			if(moveTowards(endPos)){  //note to other programmers: this is where the movement happens 
				m_transform.parent = m_toBeParent;
				m_state = State.InBlower;
				m_speed = m_baseSpeed;
			}
		}
		//change to onground when it reaches the destination
		if(m_state == State.Drop){
			if(moveTowards(m_endPosition)){
//				m_transform.parent = null;
				m_speed = m_baseSpeed;
				m_state = State.OnGround;
				collider.enabled = true;
			}
		}

	}
	//moves the transform towards the target position, returns true when it has reached the destination
	bool moveTowards(Vector3 target){
		Vector3 dir = target - m_transform.position;
		Vector3 moveVector = dir.sqrMagnitude > 1f ? dir.normalized : dir;
		if(m_speed < m_maxSpeed){
			m_speed += m_accleleration*Time.deltaTime;
		}
		if(dir.magnitude > m_speed*Time.deltaTime){
			moveVector = moveVector.normalized * m_speed;
			m_transform.localPosition += moveVector * Time.deltaTime;
			return false;
		}else{
			m_transform.position = target;
			return true;
		}
	}

//	void FixedUpdate(){
//		if(m_inBlower){
//			Profiler.BeginSample("Rotate Leifs");
////			Quaternion newAngle = Quaternion.Euler(m_transform.rotation + Vector3.forward * Time.deltaTime * m_rotationSpeed);
////			m_rigidbody.MoveRotation(newAngle);
//			m_transform.Rotate(Vector3.forward * Time.deltaTime * m_rotationSpeed * m_rotationModifier);
//			Profiler.EndSample();
//		}
//	}

	public void dropFromWhirlwind(Vector3 pos){
		m_state = State.Drop;
		m_endPosition = pos;
		m_toBeParent = null;
		m_transform.parent = m_originalParent;
	} 

	public void addToWhirlwind(Vector3 pos,Transform parent){
		m_state = State.Pickup;
		m_endPosition = pos;
		m_toBeParent = parent;
		collider.enabled = false;
	} 
}
