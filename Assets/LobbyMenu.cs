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

		addMenuItem (instance.getMenuItem (MenuManager.START_GAME));
		addMenuItem (instance.getMenuItem (MenuManager.BACK_TO_PREV));
	}
	public override void InitMenuItems()
	{
		AdjustMenuItem (m_menuItems [0], new LTRect (-300.0f, 100.0f, 300.0f, 30.0f), new Vector2 (400.0f, 100.0f));
		AdjustMenuItem (m_menuItems [1], new LTRect (-300.0f, 150.0f, 300.0f, 30.0f), new Vector2 (400.0f, 200.0f));
	}
}
