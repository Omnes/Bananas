using UnityEngine;
using System.Collections;

public class LeafLogic : MonoBehaviour {

	public enum State {OnGround,InBlower,Drop,Pickup};
	public State m_state = State.OnGround;

	public int m_id = -1;

	public float m_baseSpeed = 1f;
	public float m_maxSpeed = 15f;
	private float m_speed = 0f;
	public float m_accleleration  = 1f;
	//public float m_rotationSpeed = 80f;

	public float m_moveAround = 0.35f;
	public float m_spinSpeedInBlower = 1f;
	public float m_maxAddRandomToMoveAround = 0.5f;

	private bool m_move = false;
	private Vector3 m_endPosition;
	private Transform m_toBeParent;

	private float m_moveRandom;
	private float m_spinRandom;
	private bool m_spinTheOtherWay = false;
	private bool m_moveIn = true;

	public float m_rotationModifierRange = 2f;
	private float m_rotationModifier;

	// Use this for initialization
	void Start () {
		m_rotationModifier = Random.Range (-m_rotationModifierRange,m_rotationModifierRange);

		m_moveRandom = Random.Range(0f, m_maxAddRandomToMoveAround) + m_moveAround;
		m_spinSpeedInBlower += Random.Range(0f, m_maxAddRandomToMoveAround);
		m_spinTheOtherWay = Random.Range((int)0, (int)2) == 0 ? true : false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_state == State.Pickup){
			Vector3 endPos = m_toBeParent.TransformPoint(m_endPosition);
			endPos.y = transform.position.y;
			if(moveTowards(endPos)){
				transform.parent = m_toBeParent;
				m_state = State.InBlower;
				m_speed = m_baseSpeed;
			}
		}

		if(m_state == State.InBlower){
			Vector3 spin = m_endPosition + new Vector3(
						(m_moveRandom) * Mathf.Sin(Time.timeSinceLevelLoad * (m_spinSpeedInBlower)), 
						transform.localPosition.y, 
						(m_moveRandom) * Mathf.Cos(Time.timeSinceLevelLoad * (m_spinSpeedInBlower)));

			if(m_spinTheOtherWay)
				spin.x *= -1;

			if(m_moveIn)
				m_moveRandom -= 0.02f;
			else
				m_moveRandom += 0.02f;

			if(m_moveRandom < m_moveAround)
				m_moveIn = false;
			else if(m_moveRandom > m_moveAround + m_maxAddRandomToMoveAround)
				m_moveIn = true;

			transform.localPosition = spin;
		}

		if(m_state == State.Drop){
			if(moveTowards(m_endPosition)){
				transform.parent = null;
				m_speed = m_baseSpeed;
				m_state = State.OnGround;
				collider.enabled = true;
			}
		}

	}
	//moves the transform towards the target position, returns true when it has reached the destination
	bool moveTowards(Vector3 target){
		Vector3 dir = target - transform.position;
		Vector3 moveVector = dir.sqrMagnitude > 1f ? dir.normalized : dir;
		if(m_speed < m_maxSpeed){
			m_speed += m_accleleration*Time.deltaTime;
		}
		if(dir.magnitude > m_speed*Time.deltaTime){
			moveVector = moveVector.normalized * m_speed;
			transform.localPosition += moveVector * Time.deltaTime;
			return false;
		}else{
			transform.position = target;
			return true;
		}
	}

//	void FixedUpdate(){
//		if(m_inBlower){
//			Profiler.BeginSample("Rotate Leifs");
////			Quaternion newAngle = Quaternion.Euler(transform.rotation + Vector3.forward * Time.deltaTime * m_rotationSpeed);
////			m_rigidbody.MoveRotation(newAngle);
//			transform.Rotate(Vector3.forward * Time.deltaTime * m_rotationSpeed * m_rotationModifier);
//			Profiler.EndSample();
//		}
//	}

	public void dropFromWhirlwind(Vector3 pos){
		m_state = State.Drop;
		m_endPosition = pos;
		m_toBeParent = null;
		transform.parent = null;

	} 

	public void addToWhirlwind(Vector3 pos,Transform parent){
		m_state = State.Pickup;
		m_endPosition = pos;
		m_toBeParent = parent;
		collider.enabled = false;
	} 
}
