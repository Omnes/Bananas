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

		//addMenuItem(instance.getMenuItem (MenuManager.BACK_TO_PREV));
		addMenuItem (instance.getMenuItem (MenuManager.START_GAME));

		float top = 100.0f;
		for (int i = 0; i < m_menuItems.Count; i++) 
		{
			m_buttonRects.Add (new LTRect (-300.0f, top, 300.0f, 30.0f));
			m_centers.Add(new Vector2(300.0f, top));
			top += 30.0f;
		}

		Debug.Log ("sub"+m_menuItems.Count.ToString());
		Debug.Log ("rectList" + m_buttonRects.Count.ToString ());
//		addMenuItem(instance.getMenuItem(MenuManager.TO_CHAR_SELECT));
	}
}
