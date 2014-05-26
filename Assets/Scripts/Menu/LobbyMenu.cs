using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyMenu : MenuBase
{
	MenuManager instance;



	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenWidth/6;

		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();

		addMenuItem (instance.getMenuItem (MenuManager.BACK_TO_PREV));
	}
	public override void InitMenuItems()
	{
		AdjustMenuItem (m_menuItems [0], new LTRect (-200.0f, 100.0f, size.x, size.y), new Vector2 (centerX, centerY), new Rect(0.565f, 0.19f, 0.43f, 0.165f), LeanTweenType.easeOutElastic);
	}
}
