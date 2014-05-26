using UnityEngine;
using System.Collections;

public class GUITimer : MonoBehaviour {

	public static GUITimer s_lazyInstance;

	private TextMesh m_text;
	
	// Use this for initialization
	void Start () {
		m_text = GetComponent<TextMesh>();
		s_lazyInstance = this;
	}

	public void updateTimer(float time){
		m_text.text = getTimeString(time);
	}

	string getTimeString(float time){
		int seconds = (int)(time % 60);
		return (int)(time / 60) + ":" + (seconds < 10 ? "0" + seconds : ""+seconds);
	}
	

}
