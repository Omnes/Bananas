using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyMenu : MenuBase
{
	MenuManager instance;

	// Use this for initialization
	void Start () 
	{
		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();
		m_menuItems = new List<BaseMenuItem> ();
		m_buttonRects = new List<LTRect> ();
		m_centers = new List<Vector2> ();

		addMenuItem (instance.getMenuItem (MenuManager.START_GAME));
		addMenuItem (instance.getMenuItem (MenuManager.BACK_TO_PREV));

		float top = 100.0f;
		for (int i = 0; i < m_menuItems.Count; i++) 
		{
			m_buttonRects.Add (new LTRect (-300.0f, top, 300.0f, 30.0f));
			m_centers.Add(new Vector2(300.0f, top));
			top += 30.0f;
		}

	}
}
