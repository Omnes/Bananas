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


	void OnGUI() {
		Vector2 size = GUIMath.SmallestOfInchAndPercent(new Vector2(0.5f,0.5f),new Vector2(0.09f,0.09f));
//		Vector2 size = new Vector2(50,50);
		Rect texCordsMute = GUIMath.CalcTexCordsFromPixelRect(new Rect(294,0,158,158));
		Rect texCordsUnmute = GUIMath.CalcTexCordsFromPixelRect(new Rect(451,0,158,158));
		float PADDING = 0;
		Rect texCord = SoundManager.Instance.m_paused ? texCordsUnmute : texCordsMute;
//		if(MenuBase.CustomButton(new Rect(Screen.width - (size.x + PADDING), PADDING, size.x, size.y), Prefactory.texture_buttonAtlas,texCord)){
//			SoundManager.Instance.ToggleMute();
//			texCord = SoundManager.Instance.m_paused ? texCordsUnmute : texCordsMute;
//		}
	}
}
