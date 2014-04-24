using UnityEngine;
using System.Collections;

public class TouchInputNoBacking : InputMetod {

	public bool m_debug_mode = true;
	public Texture m_slider;

	public float m_edge_threshold_scale = 0.3f;
	public float m_blowingEdge = 0.2f;
	public int m_maxSliderHeight = 500;
	
	private Vector2 m_currentInput = new Vector2(0,0);
	private Rect m_leftArea =  new Rect();
	private Rect m_rightArea = new Rect();
	private float m_sliderHeight = 0;

	private float m_blowingPower = 0f;
	private float m_blowingEdgeLeft = 0;
	private float m_blowingEdgeRight = 0;
	private bool m_pressedBlowing = false;
	private bool m_blowing = false;

	
	void Start () {
		m_sliderHeight = (float)Mathf.Min(Screen.height / 2, m_maxSliderHeight);
		float sliderWidth = (float)Screen.width * m_edge_threshold_scale;

		m_blowingEdgeLeft = (float)Screen.width * m_blowingEdge;
		m_blowingEdgeRight = (float)Screen.width - m_blowingEdgeLeft; 

		m_leftArea = new Rect(0, 0, sliderWidth, Screen.height);
		m_rightArea = new Rect(Screen.width - sliderWidth, 0, sliderWidth, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector2 input = new Vector2();

		// pressing walk
		bool pressedLeft = false;
		bool pressedRight = false;

		// pressing blow
		bool pressedBlowLeft = false;
		bool pressedBlowRihgt = false;

		foreach (Touch t in Input.touches) 
		{
			Vector2 pos = t.position;

			float power = pos.y;

			pos.y = Screen.height - pos.y;

			if(m_rightArea.Contains(pos))
			{
				input.y = calculatePower(power);
				pressedRight = true;
			}
			else if(m_leftArea.Contains(pos))
			{
				input.x = calculatePower(power);
				pressedLeft = true;
			}

			if(pos.x < Screen.width / 2 && pos.x > m_blowingEdgeLeft)
			{
				pressedBlowLeft = true;
			}
			else if(pos.x > Screen.width / 2 && pos.x < m_blowingEdgeRight)
			{
				pressedBlowRihgt = true;
			}
		}

		if(pressedBlowLeft && pressedBlowRihgt && !m_pressedBlowing)
		{
			m_pressedBlowing = true;
			m_blowing = !m_blowing;
		}
		else if(!pressedBlowLeft && !pressedBlowRihgt)
		{
			m_pressedBlowing = false;
		}
		
		if (Input.GetMouseButton (0) &&(Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)) 
		{
			Vector2 pos = Input.mousePosition;

			float power = pos.y;

			pos.y = Screen.height - pos.y;

			//Debug.Log(pos);
			
			if(m_rightArea.Contains(pos))
			{
				input.y = calculatePower(power);
				pressedRight = true;
			}
			if(m_leftArea.Contains(pos))
			{
				input.x = calculatePower(power);
				pressedLeft = true;
			}
			
			//Debug.Log(input);
		}

		if(pressedRight && !pressedLeft)
		{
			input.x = -input.y;
		}
		else if(pressedLeft && !pressedRight)
		{
			input.y = -input.x;
		}
		
		m_currentInput = input;
		m_blowingPower = m_blowing ? 1 : 0;

		
	}

	private float calculatePower(float y)
	{
		y = Mathf.Min(y, m_sliderHeight);
		return y / m_sliderHeight;
	}

	
	
	void OnGUI(){
		if (m_debug_mode) {
			//GUI.Label(new Rect(Screen.width/2-50,0,100,50),"("+m_currentInput.x.ToString("F2") + ","+m_currentInput.y.ToString("F2") +")");
			//GUI.Label(new Rect(Screen.width/2-100,50,200,50),"Blowing Power! " + m_blowingPower.ToString("F2"));
			//GUI.Label(new Rect(Screen.width/2-100,100,200,50),"Touches " + Input.touches.Length + " / " + Input.touchCount);
			//GUI.Box(new Rect(0,m_y_offset,m_edgeThreshold,m_vertical_area*2),"");

			GUI.DrawTexture(m_leftArea, m_slider);
			GUI.DrawTexture(m_rightArea, m_slider);
		}
	}
	
	public override Vector2 getCurrentInputVector(){
		return m_currentInput;
		//return m_delayed;
	}
	
	public override float getCurrentBlowingPower(){
		return m_blowingPower;
	}
	public override void setCurrentInputVector(Vector2 v){
		m_currentInput = v;
	}
	
	public override void setCurrentBlowingPower(float f){
		m_blowingPower = f;
	}
	
	//warning this does not consider eventual min/max pixelsizes on objects dont forget to update this aswell
	//public Vector2 getGUIStickSize(){
	//	return new Vector2(m_edge_threshold_scale,m_input_y_scale);
	//}
}
