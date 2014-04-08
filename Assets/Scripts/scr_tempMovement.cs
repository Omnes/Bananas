using UnityEngine;
using System.Collections;

public class scr_tempMovement : MonoBehaviour {


	private scr_touchInput m_input;
	// Use this for initialization
	void Start () {
		m_input = GetComponent<scr_touchInput>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 inputVector = m_input.getCurrentInputVector();
		rigidbody.AddForce(transform.TransformDirection(Vector3.forward)*inputVector.y);
		rigidbody.velocity *= 0.98f;
		transform.Rotate(new Vector3(0,inputVector.x,0));
	}
}
