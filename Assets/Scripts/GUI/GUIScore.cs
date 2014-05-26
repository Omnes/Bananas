using UnityEngine;
using System.Collections;

public class GUIScore : MonoBehaviour {

	public Transform[] m_scoreObjects;

	private TextMesh[] m_texts;
	
	// make sure this is executed after the guis have been resized
	void Start () {
		m_texts = new TextMesh[m_scoreObjects.Length];

		for(int i = 0; i < m_scoreObjects.Length; i++){
			Transform scoreObject = m_scoreObjects[i];
			m_texts[i] = scoreObject.GetComponent<TextMesh>();
		}


		ScoreKeeper.RegistrerGUIScore(this);
	}

	public void updateScore(int playerID,int score){
		m_texts[playerID].text = ""+score;
	}
	
}
