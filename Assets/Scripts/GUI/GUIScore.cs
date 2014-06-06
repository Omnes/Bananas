using UnityEngine;
using System.Collections;

/*
 * The score for the individual players
 * 
 */

public class GUIScore : MonoBehaviour {

	public float m_plingDuration = 0.1f;
	public float m_plingSize = 1.2f;

	public Transform[] m_scoreObjects;
	public Transform[] m_scoreBackgrounds;
	public Transform[] m_playerNames;
	private Transform[] m_leaderGlows;

	private TextMesh[] m_texts;

	// make sure this is executed after the guis have been resized
	void Start () {
		m_texts = new TextMesh[m_scoreObjects.Length];
		m_leaderGlows = new Transform[m_scoreBackgrounds.Length];

		//disable the scorebords for all players
		for (int i = 0; i < m_scoreBackgrounds.Length; i++) {
			m_leaderGlows[i] = m_scoreBackgrounds[i].GetChild(0);
			m_leaderGlows[i].gameObject.SetActive(false);
		}
		for(int i = 0; i < m_scoreObjects.Length; i++){
			Transform scoreObject = m_scoreObjects[i];
			m_texts[i] = scoreObject.GetComponent<TextMesh>();
		}
		//enable the scoreboards for connected players
		int connectedPlayers = SeaNet.Instance.m_connectedPlayers.Count;
		for(int i = 0; i < connectedPlayers; i++){
			Transform nameTransform = m_playerNames[i];
			nameTransform.renderer.enabled = true;
			//display the 3 first letters in each players name 
			string name = SeaNet.Instance.getPlayerNames()[i];
			if(name != null){
				name = name.Substring(0,3);
			}else{
				name = "Ano";
			}
			nameTransform.GetComponent<TextMesh>().text = name;
		}
		//register this instance to the scorekeeper so it can update this
		ScoreKeeper.RegistrerGUIScore(this);
	}

	//enable the "leader glow" on the leading player
	public void setLeader(int playerID){
		for(int i = 0; i < m_leaderGlows.Length; i++){
			m_leaderGlows[i].gameObject.SetActive(false);
		}
		m_leaderGlows[playerID].gameObject.SetActive(true);
	}

	public void updateScore(int playerID,int score){
		m_texts[playerID].text = ""+score;
		pling(playerID);
	}

	//animation for when somone scores  
	public void pling(int scoreId){
		StartCoroutine(rescaleRoutine(scoreId));
	}

	//temporary scales up the scoreboard
	private IEnumerator rescaleRoutine(int id){
		float startTime = Time.time;
		float scale = m_plingSize;
		float scaleDelta = (m_plingSize-1f)/m_plingDuration;

		Vector3 originalSizeBackground = m_scoreBackgrounds[id].localScale;
		Vector3 originalSizeText = m_scoreObjects[id].localScale;
		Vector3 originalSizeName = m_playerNames[id].localScale;

		while(startTime + m_plingDuration > Time.time){
			scale -= scaleDelta*Time.deltaTime;
			m_scoreBackgrounds[id].localScale = originalSizeBackground*scale;
			m_scoreObjects[id].localScale = originalSizeText*scale;
			m_playerNames[id].localScale = originalSizeName*scale;
			yield return null;
		}
		//reset the sizes to not mess with batching
		m_scoreBackgrounds[id].localScale = originalSizeBackground;
		m_scoreObjects[id].localScale = originalSizeText;
		m_playerNames[id].localScale = originalSizeName;

	}


}
