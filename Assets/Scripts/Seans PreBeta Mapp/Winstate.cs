using UnityEngine;
using System.Collections;

public class Winstate : MonoBehaviour {

	//public int m_MAXTIME;

	public float m_startTime;
	public float m_endTime;

	private bool m_gameRunning = false;

	private GUITimer m_guiTimer;

	private bool m_playTimesUpSound = true;
	
//	public void StartGameTimer(){
//		StartGameTimer(m_MAXTIME);
//	}

	public void StartGameTimer(int gameLength){
		m_startTime = Time.time;
		m_endTime = m_startTime + gameLength - 1f;
		m_gameRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_guiTimer == null){
			m_guiTimer = GUITimer.s_lazyInstance;
		}

		if(m_gameRunning){
			float timeLeft = m_endTime - Time.time;

			if(m_guiTimer!=null){
				m_guiTimer.updateTimer(timeLeft);
			}
			if(Time.time > m_endTime){
				//we can do a check for tie here
				m_gameRunning = false;
				if(Network.isServer){
					SeaNet.Instance.savePlayersAndShutDown(ScoreKeeper.GetFirstPlaceID());
				}
			}
			if (timeLeft < 5f && m_playTimesUpSound) {
				m_playTimesUpSound = false;
				SoundManager.Instance.playOneShot(SoundManager.TIMES_UP);
			}
		}
	}

//	public void gameStart(){
//		if(Network.peerType == NetworkPeerType.Server){
//			StartCoroutine("UpdateTime");
//		}	
//	}

//	IEnumerator UpdateTime(){
//		yield return new WaitForSeconds(m_MAXTIME);
//
//		//shuts down game
//		SeaNet.Instance.savePlayersAndShutDown(ScoreKeeper.GetFirstPlaceID());
//	}
}
