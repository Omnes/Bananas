using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	private static int m_privScore = 0;

	public static int m_score
	{
		set { 
			m_privScore = value;
			SoundManager.Instance.playOneShot( "event:/leafgoal" );
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
