using UnityEngine;
using System.Collections;

public class ButtonSteering : InputMetod {

	public Texture m_rightButtonTexture;
	public Texture m_leftButtonTexture;
	public Texture m_blowButtonTexture;

	public float m_pushed = -0.5f;
	public float m_buttonSize = 8;
	public float m_buttonPos = 0;

	private Rect m_rightButton = new Rect();
	private Rect m_leftButton = new Rect();
	private Rect m_blowButton = new Rect();

	private float m_blowingPower = 1;
	private Vector2 m_inputVector = new Vector2(1,1);

	void Start () 
	{
		float buttonSize = (float)Screen.width / m_buttonSize;
		
		m_rightButton = new Rect(Screen.width - buttonSize, ((float)Screen.height - buttonSize) * m_buttonPos, buttonSize, buttonSize);
		m_leftButton = new Rect(Screen.width - buttonSize * 2, ((float)Screen.height - buttonSize) * m_buttonPos, buttonSize, buttonSize);
		m_blowButton = new Rect(0, ((float)Screen.height - buttonSize) * m_buttonPos, buttonSize, buttonSize);

	}

	void Update () 
	{
		Vector2 input = new Vector2(1,1);
		m_blowingPower = 0;

		bool pressedRight = false;
		bool pressedLeft = false;

		foreach (Touch t in Input.touches) 
		{
			Vector2 pos = t.position;

			pos.y = Screen.height - pos.y;

			if(m_rightButton.Contains(pos))
			{
				input.y = m_pushed;
				pressedRight = true;
			}
			if(m_leftButton.Contains(pos))
			{
				input.x = m_pushed;
				pressedLeft = true;
			}
			if(m_blowButton.Contains(pos))
			{
				m_blowingPower = 1;
			}
		}

		if(Input.GetKey(KeyCode.A)){
			input.y = m_pushed;
			pressedRight = true;
		}
		if(Input.GetKey(KeyCode.D)){
			input.x = m_pushed;
			pressedLeft = true;
		}


		if(pressedLeft && pressedRight)
		{
			input = new Vector2(1,1);
		}

		if (Input.GetMouseButton (0) &&(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)) 
		{
			Vector2 pos = Input.mousePosition;

			//Debug.Log(pos);


			pos.y = Screen.height - pos.y;

			if(m_rightButton.Contains(pos))
			{
				input.y = m_pushed;
			}
			if(m_leftButton.Contains(pos))
			{
				input.x = m_pushed;
			}
			if(m_blowButton.Contains(pos))
			{
				m_blowingPower = 1;
			}

			//Debug.Log(input);
		}

		m_inputVector = input;
	}

	void OnGUI()
	{
		GUI.DrawTexture(m_rightButton, m_rightButtonTexture);
		GUI.DrawTexture(m_leftButton, m_leftButtonTexture);
		GUI.DrawTexture(m_blowButton, m_blowButtonTexture);
	}


	public override float getCurrentBlowingPower()
	{
		return m_blowingPower;
	}

	public override Vector2 getCurrentInputVector()
	{
		return m_inputVector;
	}

	public override void setCurrentBlowingPower(float f)
	{

	}

	public override void setCurrentInputVector(Vector2 v)
	{

	}
	
}
