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

	void OnGUI(){
		for (int i = 0; i < m_scores.Length; i++) {
			GUI.Label(new Rect(10, 10 + 20 * i, 200, 20), "Player " + i + "'s score: " + m_scores[i]);
		}

//		GUI.Label(new Rect(10, 10, 100, 25), "Player " + 1 + "'s score: "" + m_scores[0]);
//		GUI.Label(new Rect(10, 10, 100, 25), "Player " + 2 + "'s score: " + m_scores[0]);
//		GUI.Label(new Rect(10, 10, 100, 25), "Player " + 3 + "'s score: " + m_scores[0]);
	}

}
