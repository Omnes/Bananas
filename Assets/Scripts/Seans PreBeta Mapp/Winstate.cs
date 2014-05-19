using UnityEngine;
using System.Collections;

public class Winstate : MonoBehaviour {

	private int m_startTime;
	private int m_currentTime;
	public int m_MAXTIME;
	
	public string m_nextScene = "MainMenuScene";
	public string m_nextSceneState = "MainMenu";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void gameStart(){
		if(Network.peerType == NetworkPeerType.Server){
			StartCoroutine("UpdateTime");
			Debug.Log("StartTime "+System.DateTime.Now.TimeOfDay);
		}	
	}

	IEnumerator UpdateTime(){
		yield return new WaitForSeconds(m_MAXTIME);
		Debug.Log("EndTime "+System.DateTime.Now.TimeOfDay);

		//shuts down game
		SeaNet.Instance.savePlayersAndShutDown();
	}
}
