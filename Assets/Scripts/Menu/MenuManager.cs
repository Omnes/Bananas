using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour 
{
	private static MenuManager m_menuManagerInstance;	
	private List<BaseMenuItem> m_allMenus;
	public static string remoteMenu = "NewStartingScreen";
	private MenuBase m_currentMenu;
	private MenuBase m_previousMenu;

	//Sound btn
	public static List<Rect> m_soundBtnRects;

	public static int m_currentSoundBtn;

	public static float m_standardCoolDown;
	public static float m_lobbyCoolDown;

	public const int TO_LOBBY = 0;
	public const int TO_MAIN_MENU = 1;
	public const int BACK_TO_PREV = 2;
	public const int MUTE_SOUND = 3;
	public const int UNMUTE_SOUND = 4;

	public static MenuManager Instance
	{
		//Singleton..
		get
		{
			if(m_menuManagerInstance == null)
			{
				GameObject mm = new GameObject();
				m_menuManagerInstance = mm.AddComponent<MenuManager>();
				mm.name = "MenuManager";
			}
			return m_menuManagerInstance;
		}
	}

	public MenuManager()
	{
		m_allMenus = new List<BaseMenuItem>();

		//Add MenuItems to the list containing all different kind of items that can be added to every menu..
//		addParentMenuItem ("Lobby", LoadSubMenu, "Lobby");
//		addParentMenuItem ("Main Menu", LoadSubMenu, "MainMenu");
//		addParentMenuItem ("Back", BackToPrev, "");
//		addParentMenuItem ("Mute", MuteSound, "");
	}
	// Use this for initialization
	void Start () 
	{
		//If currentobject is not camera, change this line..
		m_currentMenu = (MenuBase) Camera.main.GetComponent(remoteMenu);

		//This line initializes the items on the current menu, such as postion, tweening etc.. 
		m_currentMenu.InitMenuItems ();
	}


	// Update is called once per frame
	void Update () 
	{
		m_currentMenu.DoUpdate();
	}

	void OnGUI()
	{
		//Calls the current menus draw func(DoGUI)..
		m_currentMenu.DoGUI ();
	}

	//Used by every menu to acces the items that are available..
	public BaseMenuItem getMenuItem(int aID)
	{
		return m_allMenus [aID];
	}

	//Adds a action item such as toggle sound etc ..
	private void addActionMenuItem(string aName, BaseMenuItem.OnClickFunc aOnClickFunc, Rect aUnMute, Rect aMute)
	{
		m_allMenus.Add (new ActionMenuItem (aName, aOnClickFunc, aUnMute, aMute));
	}
	//Adds a parent item which "leads" to another menu..
	private void addParentMenuItem(string aName, BaseMenuItem.OnClickFunc aOnClickFunc, string aSceneName)
	{
		m_allMenus.Add (new ParentMenuItem (aName, aOnClickFunc, aSceneName));
	}

	//Loads a submenu, this func is called by almost all parentMenuItems..
	public void LoadSubMenu(/*BaseMenuItem aSender*/)
	{


		//Always store the current menu before switching to be able to go back..
		string nextMenu = "Lobby";
		m_previousMenu = m_currentMenu;
		m_currentMenu = (MenuBase)Camera.main.GetComponent (nextMenu);

		//Needs to set this bool to true so the tweening starts over when chan
		m_currentMenu.FirstTime = true;
		m_currentMenu.InitMenuItems ();
	}

	private void BackToPrev(BaseMenuItem aSender)
	{
		m_currentMenu = m_previousMenu;
		m_currentMenu.FirstTime = true;
		m_currentMenu.InitMenuItems ();
	}
	private void MuteSound(BaseMenuItem aSender)
	{	
		Rect unMuted = new Rect (0.267f, 0.8455f, 0.154f, 0.155f);
		Rect muted = new Rect (0.44f, 0.8455f, 0.1545f, 0.155f);
		aSender.UVRect = SoundManager.Instance.m_paused ? unMuted : muted;
		SoundManager.Instance.ToggleMute ();
	}
	public void initCurrentMenu(string aMenu)
	{
		Debug.Log (aMenu);
		m_currentMenu = (MenuBase)Camera.main.GetComponent (aMenu);
		m_currentMenu.InitMenuItems ();
	}
}
