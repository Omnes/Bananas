using UnityEngine;
using System.Collections;

public class Winstate : MonoBehaviour {

	//public int m_MAXTIME;

	public float m_startTime;
	public float m_endTime;
	public static float m_timeLeft;

	private bool m_gameRunning = false;

	private GUITimer m_guiTimer{
		get{
			if(guiTimer == null){
				guiTimer = GUITimer.s_lazyInstance;
			}
			return guiTimer;
		}
	}

	private GUITimer guiTimer;

	private bool m_playTimesUpSound = true;
	private bool m_10secLeft = true;
	
//	public void StartGameTimer(){
//		StartGameTimer(m_MAXTIME);
//	}

	public void StartGameTimer(int gameLength){
		m_startTime = Time.time;
		m_endTime = m_startTime + gameLength - 1f;
		m_guiTimer.updateTimer((float)gameLength);
		m_gameRunning = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(m_gameRunning){
			m_timeLeft = m_endTime - Time.time;

			if(m_guiTimer!=null){
				m_guiTimer.updateTimer(m_timeLeft);
			}
			if(Time.time > m_endTime){
				//we can do a check for tie here
				m_gameRunning = false;
				PowerupManager.Disable ();
				if(Network.isServer){
					SeaNet.Instance.savePlayersAndShutDown(ScoreKeeper.GetFirstPlaceID());
				}
			}

			if (m_timeLeft < 6f && m_playTimesUpSound) {
				m_playTimesUpSound = false;
				SoundManager.Instance.playOneShot(SoundManager.TIMES_UP);
			}

			if (m_timeLeft < 11f && m_10secLeft) {
				m_10secLeft = false;
				SoundManager.Instance.StartTenSecondsLeftMusic();
				SoundManager.Instance.playOneShot(SoundManager.TEN_SECONDS_LEFT);
//				StartCoroutine(PlayTenSecondsLeft());
			}
		}
	}

//	IEnumerator PlayTenSecondsLeft() {
//		yield return new WaitForSeconds(1);
//		SoundManager.Instance.StartTenSecondsLeftMusic();
//	}

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
