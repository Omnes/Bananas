using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
//	private static int m_privScore = 0;
//
//	public static int m_score
//	{
//		set { 
//			m_privScore = value;
//			SoundManager.Instance.playOneShot( "event:/leafgoal" );
//		}
//		get { return m_privScore; }
//	}
//	public static int m_score;
	public static int[] m_scores = new int[4];



//	void OnGUI(){
//		for (int i = 0; i < m_scores.Length; i++) {
//			GUI.Label(new Rect(10, 10 + 20 * i, 200, 20), "Player " + i + "'s score: " + m_scores[i]);
//		}
//
//	}

	public static string GetScoreString(){
		string s = "";
		for (int i = 0; i < 4; i++){
			s += "Player " + (i+1) + ": " + m_scores[i] + "\n";   
		}
		return s;
	}

}
