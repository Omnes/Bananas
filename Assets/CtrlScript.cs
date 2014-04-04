using UnityEngine;
using System.Collections;

public class CtrlScript : MonoBehaviour 
{
	public float m_speed;
	public float m_friction;

	private Vector2 m_inputVec;
	private scr_touchInput m_touchIn = null;

//	public Vector2 testVec = new Vector2(1.0f, 0.5f);
//	Vector3 targetVec = new Vector3(0.0f, 0.0f, 0.0f);
//	Vector3 finalDirVec = new Vector3(0, 0, 0);
//	Vector3 tmpVec;
	float right = 1.0f; //= m_inputVec.y;
	float left = 0.5f;// = m_inputVec.x;

	// Use this for initialization
	void Start () 
	{
		//testVec = new Vector2 (1.0f, 1.0f);
		m_touchIn = GetComponent<scr_touchInput> (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_inputVec = m_touchIn.getCurrentInputVector ();
	

//		if (Input.GetKey (KeyCode.U) && right < 1.0f) 
//		{
//			right += 0.25f;
//		}
//		if (Input.GetKey (KeyCode.J) && right > -1.0f) 
//		{
//			right -= 0.25f;
//		}
//		if (Input.GetKey (KeyCode.E) && left < 1.0f) 
//		{
//			left += 0.25f;
//		}
//		if (Input.GetKey (KeyCode.D) && left > -1.0f) 
//		{
//			left -= 0.25f;
//		}

		float tmpSpeed =  (right + left)/2;
//		diff = left - right;
//		angleFactor = diff / 0.1f; //always comparing to 0.1(9 degrees)
//		angleToNew = 9 * angleFactor; //getting the angle to the new direction through multiplying 9 with the factor calculated above

		Vector3 temp = Vector3.down * left + Vector3.up * right;

		//Adding rotation..
//		finalDirVec = Vector3.RotateTowards (transform.forward, targetVec, angleToNew, 0.0f);
//		Debug.DrawRay (transform.position, finalDirVec, Color.red);
//		transform.Rotate = Quaternion.AngleAxis(angleToNew, transform.up); 

		transform.Rotate (temp);

		rigidbody.AddForce (transform.forward * m_speed * tmpSpeed * m_friction, ForceMode.VelocityChange);
	}
}
