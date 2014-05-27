using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
	
	private static GameObject m_loadingScreen;
	private Texture2D m_background;
	private string m_text = "Loading..."; 
	private GUIStyle m_textStyle;

	public static void OpenLoadingScreen(string text){
		if(m_loadingScreen == null){
			//we need to disable the GUI that are beeing drawn to prevent it from drawing over the loadingscreen (a hacky fix)
			GameObject menumanager = GameObject.Find("MenuManager"); 
			if(menumanager != null){
				menumanager.SetActive(false); 
			}
			GameObject winstate = GameObject.Find("SeaNet"); 
			if(winstate != null){
				winstate.GetComponent<WinstateAnimation>().enabled = false; 
			}

			//
			m_loadingScreen = new GameObject();
			m_loadingScreen.name = "Loadingscreen";
			m_loadingScreen.AddComponent<LoadingScreen>();
			DontDestroyOnLoad(m_loadingScreen);

		}
	}

	void Start(){
		m_background = Prefactory.texture_loadingscreen;
		m_textStyle = Prefactory.style_loadingscreenText;
	}

	public static void CloseLoadingScreen(){
		GameObject winstate = GameObject.Find("SeaNet"); 
		if(winstate != null){
			winstate.GetComponent<WinstateAnimation>().enabled = true; 
		}
		Destroy(m_loadingScreen);
	}

	public static void SetLoadingText(string text){
		if(m_loadingScreen != null){
			m_loadingScreen.GetComponent<LoadingScreen>().setText(text);
		}else{
			Debug.LogError("There is no loading screen at the moment");
		}
	}

	private void setText(string text){
		m_text = text;
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),m_background);
		GUI.Label(new Rect(0,Screen.height/2,Screen.width,Screen.height/2),m_text,m_textStyle);

	}


}
