using UnityEngine;
using System.Collections;

public class GUIScore : MonoBehaviour {

	public Transform[] m_scoreObjects;

	private TextMesh[] m_texts;

	public int m_nrOfPlayers = 4;
	public float m_percentageOfScreen = 1f;
	private int m_localPlayer = 0;

	// make sure this is executed after the guis have been resized
	void Start () {
		m_localPlayer = SeaNet.Instance.getLocalPlayer();
		m_nrOfPlayers = SeaNet.Instance.m_connectedPlayers.Count;
		m_texts = new TextMesh[m_scoreObjects.Length];

		float width = m_scoreObjects[0].localScale.x;
		float heigth = m_scoreObjects[0].localScale.y;
		float padding = (1f-(width*4))/(float)(4+1);
		float startOffset = padding + width/2;
		float topOffset = heigth/2f + heigth / 10f;

		//place the localplayers score to the left and the rest to the right
		m_scoreObjects[m_localPlayer].localPosition = new Vector3(-0.5f + startOffset,0.5f - topOffset,transform.localPosition.z);
		m_scoreObjects[m_localPlayer].localScale *= 1.1f;
		int j = 0;
		for(int i = 0; i < m_scoreObjects.Length; i++){
			Transform scoreObject = m_scoreObjects[i];
			m_texts[i] = scoreObject.GetComponent<TextMesh>();
		
			if(i != m_localPlayer){
				j++;
				scoreObject.localPosition = new Vector3(-0.5f + startOffset + (width+padding)*(j),0.5f - topOffset,transform.localPosition.z);
			}
			if(i > m_nrOfPlayers-1){
				scoreObject.gameObject.SetActive(false);
			}
		}


		ScoreKeeper.RegistrerGUIScore(this);
	}

	public void updateScore(int playerID,int score){
		m_texts[playerID].text = ""+score;
	}
	
}
