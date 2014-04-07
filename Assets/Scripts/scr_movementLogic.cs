using UnityEngine;
using System.Collections;

public class scr_movementLogic : MonoBehaviour 
{
	private const float SPEED_PROPORTION = 50.0f;
	private const float ROTATE_PROPORTION = 1.0f;
	private const float FRICTION_PROPORTION = 0.95f;
	private const float MINIMUM_SPEED = 0.01f;


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
		
		if (Mathf.Abs(left + right) < MINIMUM_SPEED) 
		{
			rigidbody.velocity *= FRICTION_PROPORTION;
		}
		m_speed =  -(right + left)/SPEED_PROPORTION;

		//Adding rotation..
		Vector3 temp = Vector3.up * left + Vector3.down * right;
		temp *= ROTATE_PROPORTION;
		transform.Rotate (temp);


		rigidbody.AddForce (transform.forward * m_speed, ForceMode.VelocityChange);

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
