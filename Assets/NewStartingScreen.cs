using UnityEngine;
using System.Collections;

public class NewStartingScreen : MenuBase 
{

	LobbyButton PlayBtn;
	private float PlayXpos;
	private float PlayYpos;
	private Vector2 PlaySize;

	//Mute button
	private LobbyButton m_muteButton;

	// Use this for initialization
	void Start () 
	{
		SoundManager.Instance.StartMenuMusic ();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
//		float screenRatio = Camera.main.camera.aspect;

		//Play btn..
		PlaySize = GUIMath.SmallestOfInchAndPercent(new Vector2(300.0f, 150.0f), new Vector2(0.3f, 0.17f));
		PlayXpos = (screenWidth * 0.5f) - (PlaySize.x * 0.5f);
		PlayYpos = screenHeight * 0.5f + PlaySize.y;
		PlayBtn = new LobbyButton(Screen.width*0.5f - PlaySize.x*0.5f, Screen.height + PlaySize.y , PlaySize.x, PlaySize.y, new Rect(0.565f, 0.19f, 0.43f, 0.165f) ,new Vector2 (PlayXpos, PlayYpos)
		               , 1.0f , LeanTweenType.easeOutSine);

		float PADDING = 5f;
		Vector2 muteSize = GUIMath.SmallestOfInchAndPercent(new Vector2(0.5f,0.5f),new Vector2(0.09f,0.09f));
		m_muteButton = new LobbyButton(new Rect(Screen.width - (muteSize.x + PADDING), PADDING, muteSize.x, muteSize.y),GUIMath.CalcTexCordsFromPixelRect(new Rect(294,0,158,158)));
		
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
		Rect texCordsMute = GUIMath.CalcTexCordsFromPixelRect(new Rect(294,0,158,158));
		Rect texCordsUnmute = GUIMath.CalcTexCordsFromPixelRect(new Rect(451,0,158,158));
		
		if(this.m_muteButton.isClicked()){
			SoundManager.Instance.ToggleMute();
			Rect texCord = SoundManager.Instance.m_paused ? texCordsUnmute : texCordsMute;
			m_muteButton.changeUVrect(texCord);
		}

	}
	public override void InitMenuItems ()
	{
		PlayBtn.resetButton();
	}
}
