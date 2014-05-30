using UnityEngine;
using System.Collections;

public class LobbyButton
{
	private LTRect m_button;
	private Rect m_defButtonRect;

	private Vector2 m_targetPos;

	public float m_tweenTime;
	private LeanTweenType m_tweenType;

	private Rect m_uvRect;
	public Texture2D m_allBtns;

	private static float s_lastClickTime = 0f;
	private const float COOLDOWN = 0.125f;

	private Vector2 m_lastPos = new Vector2();
	private bool m_buttonDown = false;

	private bool m_hasTweened = false;

//	private bool m_enable;

	public LobbyButton(float x, float y, float width, float height, Rect aUvRect,
	                   Vector2 target, float tweenRate, LeanTweenType tweentype)
	{
		m_uvRect = aUvRect;
		m_allBtns = Prefactory.texture_buttonAtlas;
		
		m_button = new LTRect(x, y, width, height);
		m_defButtonRect = new Rect (m_button.rect);

		if (target != Vector2.zero) {
			m_targetPos = target;
		} else {
			m_targetPos.x = x;
			m_targetPos.y = y;
		}
		m_tweenTime = tweenRate;
		m_tweenType = tweentype;

//		if (m_tweenType != LeanTweenType.notUsed) {
//			LeanTween.move(m_button, m_targetPos, m_tweenTime).setEase(m_tweenType);
//		}
	}

	public LobbyButton(Rect area, Rect aUvRect) : 
		this(area.x, area.y, area.width, area.height, aUvRect, Vector2.zero, 0f, LeanTweenType.notUsed)
	{

	}

	public void move(){
		if(m_hasTweened == false){
			m_hasTweened = true;
			LeanTween.move(m_button, m_targetPos, m_tweenTime).setEase(m_tweenType);
		}
	}

	public bool isClicked(Rect buttonArea = new Rect()) {
		if(buttonArea.x == 0f && buttonArea.y == 0f && buttonArea.width == 0f && buttonArea.height == 0f) {
			buttonArea = m_button.rect;
		}

		GUI.DrawTextureWithTexCoords (buttonArea, m_allBtns, m_uvRect);
		if(Time.time > s_lastClickTime + COOLDOWN){
			//android
			if(Input.touchCount > 0 ){
				if(m_buttonDown == false){
					if(buttonArea.Contains(Input.GetTouch(0).position)){
						m_buttonDown = true;
						m_lastPos = Input.GetTouch(0).position;
					}
				}
			}else if(m_buttonDown == true){
				m_buttonDown = false;
				if(buttonArea.Contains(m_lastPos)){
					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}

			//PC
			if(Input.GetMouseButtonDown(0)){
				if(buttonArea.Contains(Event.current.mousePosition)){
					Debug.Log("Down");
					m_buttonDown = true;
					scaleButton(0.75f);
				}
			}
			if(Input.GetMouseButtonUp(0)){
				if(buttonArea.Contains(Event.current.mousePosition)){
					Debug.Log("Up");
					scaleButton(1.0f);
					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}

//			if (m_buttonDown) {
//				scaleButton(0.75f);
//			}
//			else {
//				scaleButton(1.0f);
//			}
		}
		return false;
	}

	private void scaleButton(float scale) {
		m_button.width = m_defButtonRect.width * scale;
		m_button.height = m_defButtonRect.height * scale;
		m_button.x = m_targetPos.x + (m_defButtonRect.width - m_button.width) / 2;
		m_button.y = m_targetPos.y + (m_defButtonRect.height - m_button.height) / 2;
//		m_button.x = m_defButtonRect.x + (m_defButtonRect.width - m_button.width) / 2;
//		m_button.y = m_defButtonRect.y + (m_defButtonRect.height - m_button.height) / 2;
	}

	public void resetButton(){
		m_button = new LTRect(m_defButtonRect);
		m_hasTweened = false;
	}

	public void changeUVrect(Rect aRect)
	{
		m_uvRect = aRect;
	}

}




/*
using UnityEngine;
using System.Collections;

public class LobbyButton
{
	private LTRect m_button;
	private Rect m_defButtonRect;

	private Vector2 m_targetPos;

	public float m_tweenTime;
	private LeanTweenType m_tweenType;

	private Rect m_uvRect;
	public Texture2D m_allBtns;

	private static float s_lastClickTime = 0f;
	private const float COOLDOWN = 0.125f;

	private Vector2 m_lastPos = new Vector2();
	private bool m_buttonDown = false;

//	private bool m_enable;

	public LobbyButton(Rect area, Rect aUvRect)
	{
		m_uvRect = aUvRect;
		m_allBtns = Prefactory.texture_buttonAtlas;
		
		m_button = new LTRect(area);
		m_defButtonRect = new Rect (m_button.rect);
	}

	public bool isClicked(Rect buttonArea = new Rect()) {
		if(buttonArea.x == 0f && buttonArea.y == 0f && buttonArea.width == 0f && buttonArea.height == 0f) {
			buttonArea = m_button.rect;
		}

		GUI.DrawTextureWithTexCoords (buttonArea, m_allBtns, m_uvRect);
		if(Time.time > s_lastClickTime + COOLDOWN){
			//android
			if(Input.touchCount > 0 ){
				if(m_buttonDown == false){
					if(buttonArea.Contains(Input.GetTouch(0).position)){
						m_buttonDown = true;
						m_lastPos = Input.GetTouch(0).position;
					}
				}
			}else if(m_buttonDown == true){
				m_buttonDown = false;
				if(buttonArea.Contains(m_lastPos)){
					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}

			//PC
			if(Input.GetMouseButtonDown(0)){
				if(buttonArea.Contains(Event.current.mousePosition)){
					Debug.Log("Down");
					m_buttonDown = true;
					scaleButton(0.75f);
				}
			}
			if(Input.GetMouseButtonUp(0)){
				if(buttonArea.Contains(Event.current.mousePosition)){
					Debug.Log("Up");
					scaleButton(1.0f);
					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}

//			if (m_buttonDown) {
//				scaleButton(0.75f);
//			}
//			else {
//				scaleButton(1.0f);
//			}
		}
		return false;
	}

	private void scaleButton(float scale) {
		m_button.width = m_defButtonRect.width * scale;
		m_button.height = m_defButtonRect.height * scale;
		m_button.x = m_targetPos.x + (m_defButtonRect.width - m_button.width) / 2;
		m_button.y = m_targetPos.y + (m_defButtonRect.height - m_button.height) / 2;
//		m_button.x = m_defButtonRect.x + (m_defButtonRect.width - m_button.width) / 2;
//		m_button.y = m_defButtonRect.y + (m_defButtonRect.height - m_button.height) / 2;
	}

	public void resetButton(){
		m_button = new LTRect(m_defButtonRect);
	}

	public void changeUVrect(Rect aRect)
	{
		m_uvRect = aRect;
	}

}
*/