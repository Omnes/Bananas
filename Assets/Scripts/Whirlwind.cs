using UnityEngine;
using System.Collections;

public class Whirlwind : MonoBehaviour {

	public float m_rotationSpeed = 120f;
	public float m_slideToSideWhenTurnLength = 0.5f;

	private InputHub m_input;
	private Rigidbody m_rigidbody;
	private Transform m_transform;
	private Transform m_parent;
	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
		m_input = m_transform.parent.GetComponent<InputHub>();
		m_parent = m_transform.parent;
	}

	void Update()
	{
		Vector2 input = m_input.getCurrentInputVector();
		Vector3 pos = m_transform.localPosition;

		if(input.y == 0f && pos.x > - m_slideToSideWhenTurnLength)
		{
			pos.x -= 0.1f;
		}
		else if(input.x == 0f &&  pos.x < m_slideToSideWhenTurnLength)
		{
			pos.x += 0.1f;
		}
		else if(input.x == 1f && input.y == 1f && pos.x != 0f)
		{
			if(pos.x > 0f)
				pos.x -= 0.1f;
			else if(pos.x < 0f)
				pos.x += 0.1f;

			if(pos.x < 0.11f && pos.x > -0.11f)
				pos.x = 0f;
		}

		m_transform.localPosition = pos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Profiler.BeginSample("Rotate Leifs");
//		Vector3 prevAngle = transform.rotation.eulerAngles;
//		Quaternion newAngle = Quaternion.Euler(prevAngle + Vector3.forward * Time.deltaTime * m_rotationSpeed);
//		m_rigidbody.MoveRotation(newAngle);
		m_transform.Rotate(Vector3.up * Time.deltaTime * m_rotationSpeed);
		Profiler.EndSample();
	}


	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Leaf_collector")){
			Debug.Log ("Whirlwind");
		}
	}
}
