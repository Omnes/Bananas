using UnityEngine;
using System.Collections;

public class GameLostFocus : MonoBehaviour {
	private bool preMuteStatus = true;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	void OnApplicationFocus(bool focusStatus) {
		if (focusStatus == false) {
			preMuteStatus = SoundManager.Instance.m_paused;
			if (preMuteStatus == false) {
				SoundManager.Instance.ToggleMute ();
			}
		}
		else {
			if (preMuteStatus == false) {
				SoundManager.Instance.ToggleMute ();
			}
		}
	}
}
