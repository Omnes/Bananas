using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeper : MonoBehaviour {
	private static int[] m_scores = new int[4];
//	private static string[] m_playerNames = new string[4];
	private static GUIScore m_scoreBoard;

	public static bool m_hasFoundLeafCollectors = false;
	private static ParticleSystem[] m_scoreParticleSystem;

	public static void AddScore(int player,int score){
		m_scores[player] += score;
		updateScoreBoard(player, m_scores[player]);

		UpdateParticleSystems (player);

		if (SeaNet.Instance.getLocalPlayer() == player) {
			SoundManager.Instance.playOneShot(SoundManager.SCORE);
		}
	}

	private static void updateScoreBoard(int playerID,int score){
		if(m_scoreBoard != null){
			m_scoreBoard.updateScore(playerID,score);
			m_scoreBoard.setLeader(GetFirstPlaceID());
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
		int totalScore = 0;
		for(int i = 0; i < m_scores.Length; i++){
			totalScore += m_scores[i];
			if(m_scores[i] >= m_scores[playerID]){
				playerID = i;
			}
		}
		return totalScore > 0 ? playerID : 0;
	}

	private static void UpdateParticleSystems(int player) {
		if (m_hasFoundLeafCollectors == false) {
			m_hasFoundLeafCollectors = true;
			
			m_scoreParticleSystem = new ParticleSystem[4];
			GameObject[] leafCollectors = GameObject.FindGameObjectsWithTag("Leaf_collector");
			
//			Debug.Log("LeafCollectors: " + leafCollectors.Length);
			for (int i = 0; i < leafCollectors.Length; i++) {
				int collectorID = leafCollectors[i].GetComponent<ID>().m_ID;
				m_scoreParticleSystem[collectorID] = leafCollectors[i].particleSystem;
			}
		}
		
		m_scoreParticleSystem [player].Play ();
	}
}
