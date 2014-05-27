using UnityEngine;
using System.Collections;

public class LeafLogic : MonoBehaviour {

	public enum State {OnGround,InBlower,Drop,Pickup};
	public State m_state = State.OnGround;

	public int m_id = -1;

	public float m_baseSpeed = 1f;
	public float m_maxSpeed = 15f;
	private float m_speed = 0f;
	public float m_accleleration  = 60f;
	public float m_rotationSpeed = 80f;

	public float m_moveAround = 0.35f;
	public float m_spinSpeedInBlower = 1f;
	public float m_maxAddRandomToMoveAround = 0.5f;
	
	private Vector3 m_endPosition;
	private Transform m_toBeParent;
	private Transform m_originalParent;

	private float m_moveRandom;
	private float m_spinRandom;
	private bool m_spinTheOtherWay = false;
	private bool m_moveIn = true;

	public float m_rotationModifierRange = 2f; 
	private float m_rotationModifier;

	private Transform m_transform;
	private float m_startYPos = 0f;
	private float m_randomStartOffset = 0;

	public float m_maxConstraintRange = 10f;

	public float m_scaleUpDuration = 0.4f;
	
	void Start () {
		m_rotationModifier = Random.Range (-m_rotationModifierRange,m_rotationModifierRange);
		m_transform = transform;
		m_startYPos = m_transform.localPosition.y;
		m_originalParent = m_transform.parent;
		m_moveRandom = Random.Range(0f, m_maxAddRandomToMoveAround) + m_moveAround;
		m_spinSpeedInBlower += Random.Range(0f, m_maxAddRandomToMoveAround);
		m_spinTheOtherWay = Random.Range((int)0, (int)2) == 0 ? true : false;
		m_randomStartOffset = Random.Range(0,2*Mathf.PI);

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
		if(m_state == State.InBlower){
			Vector3 spin = m_endPosition + new Vector3(
				(m_moveRandom) * Mathf.Sin(Time.timeSinceLevelLoad * (m_spinSpeedInBlower)+m_randomStartOffset), 
						transform.localPosition.y, 
				(m_moveRandom) * Mathf.Cos(Time.timeSinceLevelLoad * (m_spinSpeedInBlower)+m_randomStartOffset));

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

			m_transform.localPosition = spin;
		}

		if(m_state == State.Drop){
			if(moveTowards(m_endPosition)){
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

//	bool isWithinConstraints(){
//		return Vector3.Distance(m_transform.position,m_originalParent.position) < m_constraintRange;
//	}

	Vector3 getConstrainedPosition(Vector3 pos){
		Vector3 delta = (pos - m_originalParent.position);
		if(delta.magnitude > m_maxConstraintRange){
			return delta.normalized * m_maxConstraintRange;
		}
		return pos;
	}

	void FixedUpdate(){
		if(m_state == State.InBlower){
			Profiler.BeginSample("Rotate Leifs");
//			Quaternion newAngle = Quaternion.Euler(m_transform.rotation + Vector3.forward * Time.deltaTime * m_rotationSpeed);
//			m_rigidbody.MoveRotation(newAngle);
			m_transform.Rotate(Vector3.up * Time.deltaTime * m_rotationSpeed * m_rotationModifier);
			Profiler.EndSample();
		}
	}

	public void clean(){
		m_transform.parent = m_originalParent;
		m_speed = 0f;
		m_state = State.OnGround;
		collider.enabled = true;
		m_toBeParent = null;
	}

	public void spawn(){
		StartCoroutine(scaleUpLeaf());
	}

	private IEnumerator scaleUpLeaf(){
		Vector3 endSize = transform.localScale;
		float startTime = Time.time;
		transform.localScale = Vector3.zero;

		while(startTime + m_scaleUpDuration > Time.time){
			transform.localScale = endSize*((Time.time - startTime)/m_scaleUpDuration);
			yield return null;
		}
		transform.localScale = endSize;

	}

	public void dropFromWhirlwind(Vector3 pos){
		m_state = State.Drop;
		m_endPosition = getConstrainedPosition(pos);
		m_endPosition.y = m_transform.position.y;
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
