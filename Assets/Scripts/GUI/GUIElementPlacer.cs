using UnityEngine;
using System.Collections;

public class GUIElementPlacer : MonoBehaviour {


	public Transform[] m_scores;
	public Transform[] m_scoreBackgrounds;
	public float m_scoreArea = 0.9f;
	public float m_localscoreSizeMult = 1.1f;
	public float m_topMargin = 0.02f;
	

	public Transform m_timer;

	private float m_scoreHeigth;

	// Use this for initialization
	void Start () {
		placeScores();
		placeTimer();
	}

	//places the score counters and their backgrounds
	void placeScores(){
		int localPlayer = SeaNet.Instance.getLocalPlayer();
		int nrOfPlayers = SeaNet.Instance.m_connectedPlayers.Count;
		
		m_scores[localPlayer].localScale *= m_localscoreSizeMult;			//scale up the local player's score and place it
		m_scoreBackgrounds[localPlayer].localScale *= m_localscoreSizeMult;
		float padding = calcPaddingX(m_scoreBackgrounds,m_scoreArea,4);				//this padding is used for all objects
		float startOffset = padding + m_scoreBackgrounds[localPlayer].localScale.x/2;
		//place the localplayers score to the left and the rest to the right
		float localTopOffset = m_scoreBackgrounds[localPlayer].localScale.y/2f + m_topMargin;

		Vector3 localScorePos = new Vector3(-0.5f + startOffset,0.5f - localTopOffset,m_scores[localPlayer].localPosition.z);
		m_scores[localPlayer].localPosition = localScorePos;
		m_scoreBackgrounds[localPlayer].localPosition = localScorePos + Vector3.forward * 0.1f;
		
		//place the rest of the scoreobjects
		int j = 0;
		for(int i = 0; i < m_scores.Length; i++){
			Transform scoreObject = m_scores[i];
			Transform scoreBackground = m_scoreBackgrounds[i];
			float width = scoreBackground.localScale.x;
			m_scoreHeigth = scoreBackground.localScale.y;
			float topOffset = m_scoreHeigth/2f + m_topMargin;
			if(i != localPlayer){
				j++;
				Vector3 pos = new Vector3(-0.5f + startOffset + (width+padding)*(j),0.5f - topOffset,m_scores[i].localPosition.z);
				scoreObject.localPosition = pos;
				scoreBackground.localPosition = pos + Vector3.forward * 0.1f;
			}
			if(i > nrOfPlayers-1){
				scoreObject.gameObject.SetActive(false);
				scoreBackground.gameObject.SetActive(false);
			}
		}
	}

	void placeTimer(){
		m_timer.localPosition = new Vector3(0,0.5f-(m_topMargin*2+m_scoreHeigth+m_timer.localScale.y/2),m_timer.localPosition.z);
	}


	float calcPaddingX(Transform[] scores, float area,int playercount){
		float totalsize = 0;
		foreach (var item in scores) {
			totalsize += item.localScale.x;
		}
		return (area - totalsize)/((float)playercount+1f);

	}

}
