using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeper : MonoBehaviour {
	public static int[] m_scores = new int[4];
	private static string[] m_playerNames = new string[4];

	void Start(){
		m_playerNames = SeaNet.Instance.getPlayerNames();
	}

	public static string GetScoreString(){

		string s = "";
		for (int i = 0; i < 4; i++){
			s += m_playerNames[i]+" " + (i+1) + ": " + m_scores[i] + "\n";   
		}
		return s;
	}

}
