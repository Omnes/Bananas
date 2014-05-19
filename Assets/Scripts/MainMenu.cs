using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class MainMenu : MenuBase
{
	MenuManager instance;
	// Use this for initialization
	void Start()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenWidth/6;

		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();

		//Adding item to "MY"(this) menu .. 
		addMenuItem(instance.getMenuItem(MenuManager.TO_LOBBY));
		addMenuItem(instance.getMenuItem (MenuManager.START_GAME));
	}
	public override void InitMenuItems()
	{
		AdjustMenuItem (m_menuItems [0], new LTRect (-200.0f, 100.0f, size.x, size.y), new Vector2 (centerX, centerY), LeanTweenType.easeOutElastic);
		AdjustMenuItem (m_menuItems [1], new LTRect (-200.0f, 150.0f, size.x, size.y), new Vector2 (centerX, centerY + size.y));

	}
}
