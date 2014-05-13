using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	public static int[] m_scores = new int[4];

	public static string GetScoreString(){
		string s = "";
		for (int i = 0; i < 4; i++){
			s += "Player " + (i+1) + ": " + m_scores[i] + "\n";   
		}
		return s;
	}

}
