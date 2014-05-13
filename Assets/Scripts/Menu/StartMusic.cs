using UnityEngine;
using System.Collections;

public enum Mode {MainMenu, Lobby, Ingame, Win}

public class StartMusic : MonoBehaviour {
	public Mode mode = new Mode ();

	void Start () {
		SoundManager.initMusic ();

		if (mode == Mode.MainMenu) {
			SoundManager.Instance.StartMenuMusic();
		}
		else if (mode == Mode.Lobby) {
			SoundManager.Instance.StartLobbyMusic();
		}
		else if (mode == Mode.Ingame) {
			SoundManager.Instance.StartIngameMusic();
			SoundManager.Instance.playOneShot(SoundManager.COUNTDOWN);
		}
		else if (mode == Mode.Win) {
			SoundManager.Instance.StartWinuMusic();
		}
	}
}
