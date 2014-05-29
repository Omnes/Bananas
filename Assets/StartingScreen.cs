using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartingScreen : MenuBase 
{
	MenuManager instance;

	private float PlayBtnXpos;
	private float PlayBtnYpos;
	private Vector2 PlayBtnSize;

	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		
		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();
//		addMenuItem(instance.getMenuItem (MenuManager.TO_LOBBY));
//		addMenuItem (instance.getMenuItem (MenuManager.MUTE_SOUND));
		
	}

	public override void InitMenuItems()
	{
		SoundManager.Instance.StartMenuMusic ();
		float screenRatio = Camera.main.camera.aspect;
		size = GUIMath.SmallestOfInchAndPercent(new Vector2(300.0f, 150.0f), new Vector2(0.3f, 0.17f));
		centerX = (screenWidth * 0.5f) - (size.x * 0.5f);
		centerY = screenHeight - size.y;
		AdjustMenuItem (m_menuItems [0], new LTRect (Screen.width*0.5f - size.x*0.5f, Screen.height + size.y , size.x, size.y), new Vector2 (centerX, screenHeight-(size.y * 2)), new Rect(0.565f, 0.19f, 0.43f, 0.165f) , LeanTweenType.easeOutSine);

		//SoundBtn
		size = GUIMath.SmallestOfInchAndPercent (new Vector2(0.5f, 0.5f), new Vector2(0.05f, 0.05f * screenRatio));
		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenHeight/5;
		AdjustMenuItem (m_menuItems [1], new LTRect (screenWidth-size.x, 0.0f, size.x, size.y), new Vector2 (screenWidth-size.x, 0.0f), 
		                new Rect (0.267f, 0.8455f, 0.154f, 0.155f), LeanTweenType.easeOutElastic);

	}
}
