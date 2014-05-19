using UnityEngine;
using System.Collections;

public class Winstate : MonoBehaviour {

	public int m_MAXTIME;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void gameStart(){
		if(Network.peerType == NetworkPeerType.Server){
			StartCoroutine("UpdateTime");
		}	
	}

	IEnumerator UpdateTime(){
		yield return new WaitForSeconds(m_MAXTIME);

		int temp = 0;
		for(int i = 0; i < ScoreKeeper.m_scores.Length; i++){
			if(ScoreKeeper.m_scores[i] > temp){
				temp = i;
			}
		}

		//shuts down game
		SeaNet.Instance.savePlayersAndShutDown(temp);
	}
}
