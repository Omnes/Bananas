using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeper : MonoBehaviour {
	private static int[] m_scores = new int[4];
	private static string[] m_playerNames = new string[4];
	private static GUIScore m_scoreBoard;

	void Start(){
		m_playerNames = SeaNet.Instance.getPlayerNames();
	}

	public static void AddScore(int player,int score){
		m_scores[player] += score;
		updateScoreBoard(player,m_scores[player]);
	}

	private static void updateScoreBoard(int playerID,int score){
		if(m_scoreBoard != null){
			m_scoreBoard.updateScore(playerID,score);
		}
	}

	public static void ResetScore(){
		for(int i = 0; i < 4; i++){
			m_scores[i] = 0;
			updateScoreBoard(i,0);
		}

	}

	public static void RegistrerGUIScore(GUIScore guiscore){
		m_scoreBoard = guiscore;
	}

	public static int GetFirstPlaceID() {
		int playerID = 0;
		for(int i = 0; i < m_scores.Length; i++){
			if(m_scores[i] > m_scores[playerID]){
				playerID = i;
			}
		}
		return playerID;
	}
}
