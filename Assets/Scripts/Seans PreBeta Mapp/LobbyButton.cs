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

	public void move(){
		if(LeanTween.isTweening(m_ltRect) == false){
			LeanTween.move(m_ltRect, m_targetPosition, m_tweenRate).setEase(m_leanTweenType);
		}
	}

	private bool buttonDown = false;
	public bool isClicked(){
//		return	MenuBase.CustomButton (m_ltRect.rect, m_allBtns, m_uvRect);

		GUI.DrawTextureWithTexCoords (m_ltRect.rect, m_allBtns, m_uvRect);
		//Rita ut "i vilket fall" ..
		if(Input.GetMouseButtonDown(0) /*|| Input.GetTouch(0).phase == TouchPhase.Began)*/ && buttonDown == false)
		{
			if(m_ltRect.rect.Contains(Event.current.mousePosition)/* || aPosition.Contains(Input.GetTouch(0).position)*/)
			{
//				Debug.Log("Down  " + aPosition);
				buttonDown = true;
			}
		}
		else if(Input.GetMouseButtonUp(0) /*|| Input.GetTouch(0).phase == TouchPhase.Ended)*/ && buttonDown == true)
		{
			//				Debug.Log("up0  " + btnPos);
			buttonDown = false;
			if(m_ltRect.rect.Contains(Event.current.mousePosition)/* || aPosition.Contains(Input.GetTouch(0).position)*/)
			{
				//					Debug.Log("Up  " + btnPos);
				SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
				return true;
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
