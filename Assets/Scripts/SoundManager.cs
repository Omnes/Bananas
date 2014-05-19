using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
	public const string MUSIC_MENU = "event:/Music/Music_Menu";
	public const string MUSIC_LEVEL1 = "event:/Music/Music_Level1";
	public const string MUSIC_LEVEL2 = "event:/Music/Music_Level2";

	public const string EMP = "event:/SFX/SFX_EMP";
	public const string TIMEBOMB_EXPLOSION = "event:/SFX/SFX_Explosion";
	public const string FOOTSTEP = "event:/SFX/SFX_Footstep";
	public const string LEAFBLOWER = "event:/SFX/SFX_leafblower";
	public const string POWERUP_PICKUP = "event:/SFX/SFX_PickUp";
	public const string KNOCKOUT = "event:/SFX/SFX_Tackle";
	public const string TIMEBOMB_TICK = "event:/SFX/SFX_Timebomb_tick";

	public const string COUNTDOWN = "event:/VO/VO_Countdown";
	public const string LEAFBLOWER_WARCRY = "event:/VO/SvenskVO/VO_Warcry";

	private string m_currentMusic = "";
	private FMOD.Studio.EventInstance m_music;
	private FMOD.Studio.ParameterInstance m_musicParam1;
	private FMOD.Studio.ParameterInstance m_musicParam2;

	private static SoundManager instance;
	public static bool IsNull() {return instance == null;}
	public static SoundManager Instance
	{
		get
		{
			if (instance == null) {
				GameObject go = new GameObject();
				instance = go.AddComponent<SoundManager>();
				go.name = "Sound Manager";
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	public List<FMOD.Studio.EventInstance> m_sounds = new List<FMOD.Studio.EventInstance>();

	public FMOD.Studio.EventInstance playOneShot(string path)
	{
		//Create sound
		FMOD.Studio.EventInstance sound = FMOD_StudioSystem.instance.GetEvent (path);

		sound.start ();
		m_sounds.Add (sound);
		
		StartCoroutine(waitAndDestroy(sound, getSoundLength(sound)));
		return sound;
	}

	public FMOD.Studio.EventInstance playOneShot(string path, Vector3 position)
	{
		//Create sound
		FMOD.Studio.EventInstance sound = FMOD_StudioSystem.instance.GetEvent (path);

		var attributes = FMOD.Studio.UnityUtil.to3DAttributes (position);
		sound.set3DAttributes (attributes);

		sound.start ();
		m_sounds.Add (sound);

		StartCoroutine(waitAndDestroy(sound, getSoundLength(sound)));
		return sound;
	}

	public FMOD.Studio.EventInstance play (string path)
	{
		//Create sound
		FMOD.Studio.EventInstance sound = FMOD_StudioSystem.instance.GetEvent (path);
		
		sound.start ();
		m_sounds.Add (sound);

		return sound;
	}

	private float getSoundLength(FMOD.Studio.EventInstance sound) 
	{
		FMOD.Studio.EventDescription description;
		sound.getDescription (out description);
		int soundLengthMs;
		description.getLength (out soundLengthMs);
		return (float)soundLengthMs / 1000;
	}

	private IEnumerator waitAndDestroy(FMOD.Studio.EventInstance sound, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		DestroySound (sound);
	}

	public void DestroySound(FMOD.Studio.EventInstance sound)
	{
		if (sound != null) {
			sound.stop ();
			sound.release ();
			m_sounds.Remove (sound);
		}
	}
	
	public static void Mute() {
		Debug.Log ("Mute");
//		FMOD_Listener listener = Camera.main.GetComponent<FMOD_Listener> ();
//		listener.audio.mute = true;
	}

	public static void Unmute() {
		Debug.Log ("Unmute");
//		FMOD_Listener listener = Camera.main.GetComponent<FMOD_Listener> ();
//		listener.audio.mute = false;
	}

	public static void ToggleMute() {
		Debug.Log ("Toggle mute");
//		FMOD_Listener listener = Camera.main.GetComponent<FMOD_Listener> ();
//		listener.audio.mute = !listener.audio.mute;
//		listener.enabled = !listener.enabled;
	}

//	FMOD.Studio.ParameterInstance menuParam;
//	static FMOD.Studio.ParameterInstance lobbyParam;
//	static FMOD.Studio.ParameterInstance ingameParam;
//	static FMOD.Studio.ParameterInstance bombParam;
//	static FMOD.Studio.ParameterInstance winParam;
//	public static void initMusic() {
//		if (m_music == null) {
//			m_music = Instance.play(MUSIC);
//			m_music.getParameter("Menu", out lobbyParam);
//			m_music.getParameter("Ingame", out ingameParam);
//			m_music.getParameter("Bomb", out bombParam);
//			m_music.getParameter("Win", out winParam);
//		}
//	}

	public void StartMenuMusic() {
		if (m_currentMusic != MUSIC_MENU) {
			m_currentMusic = MUSIC_MENU;
			DestroySound (m_music);
			m_music = play (MUSIC_MENU);
			m_music.getParameter("Menu", out m_musicParam1);
		}
		m_musicParam1.setValue (0);
	}

	public void StartLobbyMusic() {
		StartMenuMusic ();
		m_musicParam1.setValue (2);
	}

	public void StartIngameMusic() {

		if (m_currentMusic != MUSIC_LEVEL1) {
			m_currentMusic = MUSIC_LEVEL1;
			DestroySound (m_music);

			int rand = Random.Range(0, 2);
			if (rand == 0) {
				m_music = play (MUSIC_LEVEL1);
			}
			else {
				m_music = play (MUSIC_LEVEL2);
			}
			m_music.getParameter("Win", out m_musicParam1);
			m_music.getParameter("Bomb", out m_musicParam2);
		}
		m_musicParam1.setValue (0);
		m_musicParam2.setValue (0);
	}

	public void StartBombMusic() {
		StartIngameMusic ();
		m_musicParam1.setValue (0);
		m_musicParam2.setValue (1);
	}

	public void StartWinMusic() {
		m_musicParam1.setValue (1);
		m_musicParam2.setValue (0);
	}
}