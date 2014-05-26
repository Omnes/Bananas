using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartingScreen : MenuBase 
{
	MenuManager instance;

	public Texture2D PlayGame;
	private List<Rect> m_soundBtnRects;


	public float playButtonSizeX = 1.0f;
	public float playButtonSizeY = 1.0f;
//
	public float soundButtonSizeX = 1.0f;
	public float soundButtonSizeY = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		instance = MenuManager.Instance;
		m_menuItems = new List<BaseMenuItem> ();
		addMenuItem(instance.getMenuItem (MenuManager.TO_LOBBY));
		addMenuItem (instance.getMenuItem (MenuManager.MUTE_SOUND));
//		addMenuItem (instance.getMenuItem (MenuManager.UNMUTE_SOUND));

		m_soundBtnRects = new List<Rect> ();
		m_soundBtnRects.Add (new Rect (0.267f, 0.8455f, 0.154f, 0.155f));
		m_soundBtnRects.Add (new Rect (0.422f, 0.8455f, 0.154f, 0.155f));

		m_currentSoundBtn = 0;
	}

	public override void InitMenuItems()
	{
		float screenRatio = Camera.main.camera.aspect;
		size = GUIMath.SmallestOfInchAndPercent(new Vector2(300.0f, 150.0f), new Vector2(0.25f, 0.2f));
		centerX = (screenWidth * 0.5f) - (size.x * 0.5f);
		centerY = screenHeight - size.y;
		AdjustMenuItem (m_menuItems [0], new LTRect (-200, 100.0f, size.x, size.y), new Vector2 (centerX, screenHeight-(size.y * 2)), new Rect(0.565f, 0.19f, 0.43f, 0.165f) , LeanTweenType.easeOutElastic);

		size = GUIMath.SmallestOfInchAndPercent (new Vector2(0.5f, 0.5f), new Vector2(0.05f, 0.05f * screenRatio));
		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenHeight/5;
		AdjustMenuItem (m_menuItems [1], new LTRect (screenWidth-size.x, 0.0f, size.x, size.y), new Vector2 (screenWidth-size.x, 0.0f), m_soundBtnRects[m_currentSoundBtn], LeanTweenType.easeOutElastic);
//		AdjustMenuItem (m_menuItems [2], new LTRect (-200.0f, 100.0f, size.x * 0.6f, size.y ), new Vector2 (centerX + 600.0f, centerY - 250.0f), new Rect(0.422f, 0.8455f, 0.154f, 0.155f) , LeanTweenType.easeOutElastic);

	}
}
