using UnityEngine;
using System.Collections;


public class LobbyButton{

	//name
	private string m_name;
	//rect
	private LTRect m_ltRect;
	//pos
	private Vector2 m_position;
	private Vector2 m_targetPosition;
	//type
	private LeanTweenType m_leanTweenType;
	//rate
	public float m_tweenRate = 3.0f;
	//size
	private Vector2 m_size;

	private Rect m_uvRect;
	//btnImageMap

	public Texture2D m_allBtns;
	private Texture2D m_btnTexture;

	private static float s_lastClickTime = 0f;
	private const float CLICKCOOLDOWN = 0.5f;

	public LobbyButton(float top, float left, float x, float y, Rect aUvRect, Vector2 target, float tweenRate, LeanTweenType tweentype)
	{
		m_position = new Vector2(top, left);
		m_size = new Vector2(x,y);

		m_uvRect = aUvRect;
		m_allBtns = Prefactory.texture_buttonAtlas;
//		m_name = name;
		//ltRect
		m_ltRect = new LTRect(top, left, x, y);

		m_targetPosition = target;
		m_leanTweenType = tweentype;
		m_tweenRate = tweenRate;
	}

	public LobbyButton(Rect area, Rect aUvRect)
	{
		m_position = new Vector2(area.x, area.y);
		m_size = new Vector2(area.width,area.height);
		
		m_uvRect = aUvRect;
		m_allBtns = Prefactory.texture_buttonAtlas;
		//		m_name = name;
		//ltRect
		m_ltRect = new LTRect(area);
		
		m_targetPosition = Vector2.zero;
		m_leanTweenType = LeanTweenType.notUsed;
		m_tweenRate = 0f;
	}


	public void move(){
		if(LeanTween.isTweening(m_ltRect) == false){
			LeanTween.move(m_ltRect, m_targetPosition, m_tweenRate).setEase(m_leanTweenType);
		}
	}

	private bool buttonDown = false;
	public bool isClicked(Rect pos = new Rect()){
		if((pos.x == 0f )&&( pos.y == 0f)&& (pos.width == 0f) && (pos.height == 0f)){
			pos = m_ltRect.rect;
		}
//		return	MenuBase.CustomButton (m_ltRect.rect, m_allBtns, m_uvRect);

		GUI.DrawTextureWithTexCoords (pos, m_allBtns, m_uvRect);
		if(Time.time > s_lastClickTime + CLICKCOOLDOWN){
			//android
			if(Input.touchCount > 0 ){
				if(Input.GetTouch(0).phase == TouchPhase.Began && buttonDown == false){
					if(pos.Contains(Input.GetTouch(0).position)){
						buttonDown = true;
					}
				}
				else if(Input.GetTouch(0).phase == TouchPhase.Ended && buttonDown == true){
					buttonDown = false;
					if( pos.Contains(Input.GetTouch(0).position))
					{
						s_lastClickTime = Time.time;
						SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
						return true;
					}
				}
			}
			//PC
			if((Input.GetMouseButtonDown(0)) && buttonDown == false){
				if(pos.Contains(Event.current.mousePosition)){
					buttonDown = true;
				}
			}
			else if((Input.GetMouseButtonUp(0)) && buttonDown == true){
				buttonDown = false;
				if(pos.Contains(Event.current.mousePosition)){
					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}
		}
		return false;

	}

	public void resetButton(){
		m_ltRect = new LTRect(m_position.x, m_position.y, m_size.x, m_size.y);
	}
	public void changeUVrect(Rect aRect)
	{
		m_uvRect = aRect;
	}

}
