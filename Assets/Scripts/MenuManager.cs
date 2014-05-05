using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager //: MonoBehaviour 
{
	private static MenuManager m_menuManagerInstance;	
	private List<BaseMenuItem> m_allMenus;
	public const int START_GAME = 0;
	public const int SECOND_IN_MAIN = 1;
	public const int THIRD_IN_MAIN = 2;


	public static MenuManager Instance
	{
		get
		{
			if(m_menuManagerInstance == null)
			{
				m_menuManagerInstance = new MenuManager();
			}
			Debug.Log(m_menuManagerInstance);
			return m_menuManagerInstance;
		}
	}

	public MenuManager()
	{
		m_allMenus = new List<BaseMenuItem>();
		addActionMenuItem ("Start Game", StartGame);
		addParentMenuItem ("SecondInMain", LoadSubMenu, "latest_Daniel");
		addActionMenuItem ("ThirdInMain", ThirdButtonClicked);

	}
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public BaseMenuItem getMenuItem(int aID)
	{
		Debug.Log (m_allMenus);
		return m_allMenus [aID];
	}
	
	private void addActionMenuItem(string aName, BaseMenuItem.OnClickFunc aOnClickFunc)
	{
		m_allMenus.Add (new ActionMenuItem (aName, aOnClickFunc));
	}
	private void addParentMenuItem(string aName, BaseMenuItem.OnClickFunc aOnClickFunc, string aSceneName)
	{
		m_allMenus.Add (new ParentMenuItem (aName, aOnClickFunc, aSceneName));
	}

	private void StartGame(BaseMenuItem aSender)
	{

	}
	private void LoadSubMenu(BaseMenuItem aSender)
	{
		Application.LoadLevel (aSender.SubSceneName);
		Debug.Log ("Second clicked");
	}

	private void ThirdButtonClicked(BaseMenuItem aSender)
	{
		Debug.Log ("Third clicked");
	}
}
