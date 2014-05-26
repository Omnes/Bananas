using UnityEngine;
using System.Collections;


public class LobbyButton {

	//name
	public string m_name;
	//rect
	public LTRect m_ltRect;
	//pos
	public Vector2 m_position;
	private Vector2 m_targetPosition;
	//type
	private LeanTweenType m_leanTweenType;
	//rate
	public float m_tweenRate = 3.0f;
	//size
	public Vector2 m_size;


	public LobbyButton(float top, float left, float x, float y, string name, Vector2 target, float tweenRate, LeanTweenType tweentype)
	{
		m_position = new Vector2(top, left);
		m_size = new Vector2(x,y);
		m_name = name;
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

	public bool isClicked(){
		return GUI.Button(m_ltRect.rect, m_name);
	}

	public void resetButton(){
		m_ltRect = new LTRect(m_position.x, m_position.y, m_size.x, m_size.y);
	}

}
