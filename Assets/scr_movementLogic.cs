using UnityEngine;
using System.Collections;

public class CtrlScript : MonoBehaviour 
{
	float right = 0.0f;
	float left = 0.0f;
	private float tmpSpeed;
	private const float SPEEDPROPORTION = 50.0f;
	private const float ROTATEPROPORTION = 1.0f;
	private const float FRICTIONPROPORTION = 0.95f;
	private const float MINIMUMSPEED = 0.1f;
	Rect m_leftArea = new Rect ();
	Rect m_rightArea = new Rect ();

	private Vector2 m_inputVec;
	private scr_touchInput m_touchIn = null;

	string myLog;
//	public Vector2 testVec = new Vector2(1.0f, 0.5f);
//	Vector3 targetVec = new Vector3(0.0f, 0.0f, 0.0f);
//	Vector3 finalDirVec = new Vector3(0, 0, 0);
//	Vector3 tmpVec;

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


//		if (Input.GetKeyUp (KeyCode.U) && right <= 1.0f) 
//		{
//			right += 0.25f;
//		}
//		if (Input.GetKeyUp (KeyCode.J) && right >= -1.0f) 
//		{
//			right -= 0.25f;
//		}
//		if (Input.GetKeyUp (KeyCode.E) && left <= 1.0f) 
//		{
//			left += 0.25f;
//		}
//		if (Input.GetKeyUp (KeyCode.D) && left >= -1.0f) 
//		{
//			left -= 0.25f;
//		}

		if (Mathf.Abs(left + right) < 0.1f) 
		{
			tmpSpeed = 0.0f;
			rigidbody.velocity *= tmpSpeed;
			return;
		}
		tmpSpeed =  -(right + left)/SPEEDPROPORTION;
//		diff = left - right;
//		angleFactor = diff / 0.1f; //always comparing to 0.1(9 degrees)
//		angleToNew = 9 * angleFactor; //getting the angle to the new direction through multiplying 9 with the factor calculated above

		Vector3 temp = Vector3.down * left + Vector3.up * right;
		temp *= ROTATEPROPORTION;
		//Adding rotation..
//		finalDirVec = Vector3.RotateTowards (transform.forward, targetVec, angleToNew, 0.0f);
//		Debug.DrawRay (transform.position, finalDirVec, Color.red);
//		transform.Rotate = Quaternion.AngleAxis(angleToNew, transform.up); 
		
		transform.Rotate (temp);

//		rigidbody.AddForce (transform.forward * m_speed * tmpSpeed * m_friction, ForceMode.VelocityChange);
		rigidbody.AddForce (transform.forward * tmpSpeed * FRICTIONPROPORTION, ForceMode.VelocityChange);

	}
	void OnGUI () 
	{
		GUI.Box(m_leftArea,"Left");
		GUI.Box(m_rightArea,"Rigth");
		GUI.Label(new Rect(Screen.width/2-50,0,100,25),"("+left.ToString("F2") + ","+right.ToString("F2") +")");
//		GUI.Label(new Rect(Screen.width/2-100,25,200,25),"Blowing Power! " + m_blowing_power.ToString("F2"));
//		GUI.Box(new Rect(0,m_y_offset,10,m_vertical_area*2),"");
	}
}
