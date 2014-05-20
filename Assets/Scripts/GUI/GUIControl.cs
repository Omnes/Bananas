using UnityEngine;
using System.Collections;

public class GUIControl : MonoBehaviour {

	private Camera m_camera;
//	public TouchInput m_input;
	public static int DEFAULT_DPI = 96; // 96 for computers and 200-300 ish for phones
	public GUIStyle m_muteButtonStyle;
	
	public void initiateGUI(TouchInput input){
//		m_input = input;
		m_camera = transform.parent.camera;
		placeQuad();
//		if(m_input == null){
//			Debug.LogError("m_input is not assigned on the GUICamera->GUI->GUIControl script");
//		}
//		BroadcastMessage("init",m_input);
	}

	void placeQuad(){
		transform.localPosition = calcPosition();
		transform.localScale = calcScale();
	}

	Vector3 calcPosition(){
		return new Vector3(0,0,m_camera.farClipPlane);
	}

	Vector3 calcScale(){
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float size = m_camera.orthographicSize * 2;
		Vector3 scale = new Vector3(size * aspectRatio,size,1);
		return scale;
	}

	public static float GetDPI()
	{
		return Screen.dpi == 0 ? DEFAULT_DPI : Screen.dpi;
	}

	//Mute button
	private const float WIDTH = 50;
	private const float HEIGHT = 50;
	private const float PADDING = 10;
	void OnGUI() {
		if (SoundManager.Instance.m_paused == false) {
			if(GUI.Button(new Rect(Screen.width - (WIDTH + PADDING), PADDING, WIDTH, HEIGHT), Prefactory.texture_muteButton,m_muteButtonStyle))
			{
				SoundManager.Instance.ToggleMute();
			}
		}
		else {
			if(GUI.Button(new Rect(Screen.width - (WIDTH + PADDING), PADDING, WIDTH, HEIGHT), Prefactory.texture_muteButton2,m_muteButtonStyle))
			{
				SoundManager.Instance.ToggleMute();
			}
		}
	}
}
