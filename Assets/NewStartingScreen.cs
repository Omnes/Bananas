using UnityEngine;
using System.Collections;

public class NewStartingScreen : MenuBase 
{

	LobbyButton PlayBtn;
	private float PlayXpos;
	private float PlayYpos;
	private Vector2 PlaySize;

	LobbyButton SoundBtn;
	private float SoundXpos;
	private float SoundYpos;
	private Vector2 SoundSize;

	// Use this for initialization
	void Start () 
	{
		SoundManager.Instance.StartMenuMusic ();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		float screenRatio = Camera.main.camera.aspect;

		//Play btn..
		PlaySize = GUIMath.SmallestOfInchAndPercent(new Vector2(300.0f, 150.0f), new Vector2(0.3f, 0.17f));
		PlayXpos = (screenWidth * 0.5f) - (PlaySize.x * 0.5f);
		PlayYpos = screenHeight * 0.5f + PlaySize.y;
		PlayBtn = new LobbyButton(-200, 100.0f, PlaySize.x, PlaySize.y, new Rect(0.565f, 0.19f, 0.43f, 0.165f) ,new Vector2 (PlayXpos, PlayYpos)
		               , 3.0f , LeanTweenType.easeOutElastic);

		//SoundBtn
		SoundSize = GUIMath.SmallestOfInchAndPercent (new Vector2(0.5f, 0.5f), new Vector2(0.05f, 0.05f * screenRatio));
		SoundXpos = screenWidth/2 - (SoundSize.x / 2);
		SoundYpos = screenHeight/5;
		SoundBtn = new LobbyButton(screenWidth-SoundSize.x, 0.0f, SoundSize.x, SoundSize.y, new Rect (0.267f, 0.8455f, 0.154f, 0.155f), 
		                           new Vector2 (screenWidth-SoundSize.x, 0.0f), 3.0f, LeanTweenType.easeOutElastic);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void DoGUI ()
	{
		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_backGround);
		PlayBtn.move ();
		if(PlayBtn.isClicked ())
		{
			SoundManager.Instance.StartLobbyMusic ();
			MenuManager.Instance.LoadSubMenu();
		}

		SoundBtn.move ();
		if(SoundBtn.isClicked())
		{
			Rect unMuted = new Rect (0.267f, 0.8455f, 0.154f, 0.155f);
			Rect muted = new Rect (0.44f, 0.8455f, 0.1545f, 0.155f);
			SoundBtn.changeUVrect(SoundManager.Instance.m_paused ? unMuted : muted);
			SoundManager.Instance.ToggleMute ();

		}


	}
}
