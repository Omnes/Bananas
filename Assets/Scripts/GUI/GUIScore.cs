using UnityEngine;
using System.Collections;

public class GUIScore : MonoBehaviour {

	public Transform[] m_scoreObjects;

	private TextMesh[] m_texts;

	public int m_nrOfPlayers = 4;

	// make sure this is executed after the guis have been resized
	void Start () {
		m_nrOfPlayers = SeaNet.Instance.m_connectedPlayers.Count;
		m_texts = new TextMesh[m_scoreObjects.Length];
		for(int i = 0; i < m_scoreObjects.Length; i++){
			Transform scoreObject = m_scoreObjects[i];
			m_texts[i] = scoreObject.GetComponent<TextMesh>();
			float width = scoreObject.localScale.x;
			float heigth = scoreObject.localScale.y;
			float padding = (1f-(width*m_nrOfPlayers))/(float)(m_nrOfPlayers+1);
			float startOffset = padding + width/2;
			float topOffset = heigth/2f + heigth / 10f;
			scoreObject.localPosition = new Vector3(-0.5f + startOffset + (width+padding)*(i),0.5f - topOffset,transform.localPosition.z);
			if(i > m_nrOfPlayers - 1){
				scoreObject.renderer.enabled = false;
			}
		}
		ScoreKeeper.RegistrerGUIScore(this);
	}

	public void updateScore(int playerID,int score){
		m_texts[playerID].text = ""+score;
	}
	
}
