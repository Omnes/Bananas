using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MainMenu : MonoBehaviour 
{
	public List<BaseMenuItem> m_menuItems;
	MenuManager instance;
	// Use this for initialization
	void Start () 
	{
		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();
		addMenuItem(instance.getMenuItem(MenuManager.SECOND_IN_MAIN));		//Adding item to "MY"(this) menu .. 
		addMenuItem(instance.getMenuItem(MenuManager.THIRD_IN_MAIN));
	}
	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//	}

	void OnGUI()
	{
		float top = 100.0f;
		foreach(BaseMenuItem item in m_menuItems)
		{
			if(GUI.Button(new Rect(100.0f, top, 300.0f, 30.0f), item.Name))
			{
				if(item.OnClick != null)
				{
					item.OnClick(item);
				}
			}
			top +=30.0f;
		}
	}
	void addMenuItem(BaseMenuItem aItem)
	{
		m_menuItems.Add (aItem);
	}
}
