using UnityEngine;
using System.Collections;

public class LoadingScreenMenu : MenuBase 
{
	public Texture m_loadingScreen;

	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	public override void DoGUI ()
	{
		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_loadingScreen, ScaleMode.StretchToFill);
	}
}
