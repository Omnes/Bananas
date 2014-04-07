using UnityEngine;
using System.Collections;

public class scr_scoreKeeper : MonoBehaviour {
	public static int m_score = 0;

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
