using UnityEngine;
using System.Collections;

public class Winstate : MonoBehaviour {

	private int m_startTime;
	private int m_currentTime;
	public int m_MAXTIME = 60;

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
			m_startTime = System.DateTime.Now.Second + (System.DateTime.Now.Minute * 60);
		}	
	}

	IEnumerator UpdateTime(){
		do{
			m_currentTime = System.DateTime.Now.Second + (System.DateTime.Now.Minute * 60);
			yield return new WaitForSeconds(0.2f);
		}while(m_startTime + m_MAXTIME > m_currentTime);
		Debug.Log("EndTime "+System.DateTime.Now.TimeOfDay);
		//shuts down game
		SeaNet.Instance.savePlayersAndShutDown();
	}


}
