using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour 
{
	private static MenuManager m_menuManagerInstance;	
	private List<BaseMenuItem> m_allMenus;
	public static string remoteMenu = "MainMenu";
	private MenuBase m_currentMenu;
	private MenuBase m_previousMenu;

	public const int START_GAME = 0;
	public const int TO_LOBBY = 1;
	public const int TO_MAIN_MENU = 2;
	public const int BACK_TO_PREV = 3;
	public const int TO_CHAR_SELECT = 4;

	public static MenuManager Instance
	{
		get
		{
			if(m_menuManagerInstance == null)
			{
				GameObject mm = new GameObject();
				m_menuManagerInstance = mm.AddComponent<MenuManager>();
				mm.name = "MenuManager";
			}
			Debug.Log(m_menuManagerInstance);
			return m_menuManagerInstance;
		}
	}

	public MenuManager()
	{
		m_allMenus = new List<BaseMenuItem>();


		addParentMenuItem ("StartGame", StartGame, "latest_Daniel");
		addParentMenuItem ("Lobby", LoadSubMenu, "Lobby");
		addParentMenuItem ("Main Menu", LoadSubMenu, "MainMenu");
		addParentMenuItem ("Back", BackToPrev, "");
		addParentMenuItem ("CharSelect", LoadSubMenu, "CharacterSelectionMenu");
	}
	// Use this for initialization
	void Start () 
	{
//		DontDestroyOnLoad (gameObject);
		//if currentobject is not camera, change this line
		m_currentMenu = (MenuBase) Camera.main.GetComponent(remoteMenu);
	}

	// Update is called once per frame
	void Update () 
	{
	}

	void OnGUI()
	{

		m_currentMenu.DoGUI ();
	}

	public BaseMenuItem getMenuItem(int aID)
	{
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
		Application.LoadLevel (aSender.SubMenuName);
	}
	private void LoadSubMenu(BaseMenuItem aSender)
	{
		SoundManager.Instance.StartLobbyMusic ();

		string nextMenu = aSender.SubMenuName;
		m_previousMenu = m_currentMenu;
		m_currentMenu = (MenuBase)Camera.main.GetComponent (nextMenu);
	}

	private void BackToPrev(BaseMenuItem aSender)
	{
		m_currentMenu = m_previousMenu;
	}

//	public static void RemoteSetMenu(string aMenuName)
//	{
//		remoteMenu = aMenuName;
//	}
}
