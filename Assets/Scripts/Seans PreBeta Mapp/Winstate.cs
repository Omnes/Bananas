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

		//shuts down game
		SeaNet.Instance.savePlayersAndShutDown(ScoreKeeper.GetFirstPlaceID());
	}
}
