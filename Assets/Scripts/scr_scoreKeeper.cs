using UnityEngine;
using System.Collections;

public class scr_scoreKeeper : MonoBehaviour {
	private static int m_privScore = 0;

	public static int m_score
	{
		set { 
			m_privScore = value;
//			scr_soundManager.play( "event:/IngamePause" );
//			Debug.Log( "Sounds: " + scr_soundManager.m_sounds.Count );
		}
		get { return m_privScore; }
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 10, 100, 25), "Score: " + m_score);
	}

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
