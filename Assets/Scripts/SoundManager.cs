using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Lägg till destroyTime parameter
//TODO: Play funktion som inte tar bort ljudet
//TODO: Ha koll på hur många av varje ljud som spelas upp så att man kan sätta tex, max 2 ljud får spelas samtidigt
public class SoundManager : MonoBehaviour {
	public const string MUSIC = "event:/Music/MenuEvent";

	public const string FOOTSTEP = "event:/SFX/Fotstegspring";
	public const string KNOCKOUT = "event:/SFX/Knockout! (1)";
	public const string LEAFBLOWER = "event:/SFX/leafblower (ytterst kass)";
	public const string SCORE = "event:/SFX/leafgoal";
	public const string TIMEBOMB_EXPLOSION = "event:/SFX/Timebomb_explosion_v2";
	public const string TIMEBOMB_TICK = "event:/SFX/Timebomb_tick_2";

//	public const string COUNTDOWN = "event:/VO/Countdown";
	public const string COUNTDOWN = "event:/VO/3,2,1 GO ";


	private static FMOD.Studio.EventInstance m_music;

	private static SoundManager instance;
//	public static SoundManager Instance{get{return instance;}}
//	void Awake() {
//		if (instance != null && instance != this) {
//			Destroy(this.gameObject);
//			return;
//		} else {
//			instance = this;
//		}
//		DontDestroyOnLoad(this.gameObject);
//	}
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

//		FMOD.Studio.EventDescription description;
//		sound.getDescription (out description);
//		float minDistance;
//		description.getMinimumDistance (out minDistance);
//		minDistance = 0;
//
//		float maxDistance;
//		description.getMinimumDistance (out maxDistance);
//		maxDistance = 0;
//		bool is3D;
//		description.is3D (out is3D);
//		is3D = false;

//		description.
//		sound.

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
		sound.stop ();
		sound.release ();
		m_sounds.Remove (sound);
	}



	//Mute
//	void Update() {
//		if (Input.GetKey (KeyCode.M))
//			ToggleMute ();
//	}
//
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

	//MUSIC
	public static FMOD.Studio.EventInstance getMusic() {
		return m_music;
	}

//	FMOD.Studio.ParameterInstance menuParam;
	static FMOD.Studio.ParameterInstance lobbyParam;
	static FMOD.Studio.ParameterInstance ingameParam;
	static FMOD.Studio.ParameterInstance bombParam;
	static FMOD.Studio.ParameterInstance winParam;
	public static void initMusic() {
		if (m_music == null) {
			m_music = Instance.play(MUSIC);
			m_music.getParameter("Menu", out lobbyParam);
			m_music.getParameter("Ingame", out ingameParam);
			m_music.getParameter("Bomb", out bombParam);
			m_music.getParameter("Win", out winParam);
		}
	}

	public void StartMenuMusic() {
		lobbyParam.setValue (0);
		ingameParam.setValue (0);
		bombParam.setValue (0);
		winParam.setValue (0);
	}

	public void StartLobbyMusic() {
		lobbyParam.setValue (2);
		ingameParam.setValue (0);
		bombParam.setValue (0);
		winParam.setValue (0);
	}

	public void StartIngameMusic() {
		lobbyParam.setValue (2);
		ingameParam.setValue (1);
		bombParam.setValue (0);
		winParam.setValue (0);
	}

	public void StartBombMusic() {
		lobbyParam.setValue (0);
		ingameParam.setValue (1);
		bombParam.setValue (1);
		winParam.setValue (0);
	}

	public void StartWinuMusic() {
		lobbyParam.setValue (0);
		ingameParam.setValue (0);
		bombParam.setValue (0);
		winParam.setValue (1);
	}

}