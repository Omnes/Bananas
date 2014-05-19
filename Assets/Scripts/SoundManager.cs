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
	private FMOD.Studio.EventInstance m_music;	//TODO: Byt ut till ett track per musik så att man inte kör destroy på ljuden (blir hack då)
//	private FMOD.Studio.EventInstance m_music_menu;
//	private FMOD.Studio.EventInstance m_musicLevel1;
//	private FMOD.Studio.EventInstance m_musicLevel2;
	private FMOD.Studio.ParameterInstance m_musicParam1;
	private FMOD.Studio.ParameterInstance m_musicParam2;

	//TEST
	private FMOD.Studio.MixerStrip m_masterBus;
	private FMOD.Studio.MixerStrip m_musicBus;
	private FMOD.Studio.MixerStrip m_SFXBus;
	private FMOD.Studio.System m_system;
	public bool m_paused = false;

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

				instance.InitChannels();
			}
			return instance;
		}
	}

	private void InitChannels () {
		m_system = FMOD_StudioSystem.instance.System;
		
		FMOD.GUID masterGuid = new FMOD.GUID();
		m_system.lookupID ("bus:/", out masterGuid);
		m_system.getMixerStrip (masterGuid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out m_masterBus);
		
		FMOD.GUID musicGuid = new FMOD.GUID();
		m_system.lookupID ("bus:/Music", out musicGuid);
		m_system.getMixerStrip (musicGuid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out m_musicBus);
		
		FMOD.GUID SFXguid = new FMOD.GUID();
		m_system.lookupID ("bus:/SFX", out SFXguid);
		m_system.getMixerStrip (SFXguid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out m_SFXBus);
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
	
	void Update() {
		if (Input.GetKeyDown (KeyCode.M)) {
			ToggleMute();
		}
	}

//	public void Mute() {
//	}

//	public void Unmute() {
//	}

	public void ToggleMute() {
		m_paused = !m_paused;
		m_masterBus.setPaused (m_paused);
//		m_musicBus.setPaused (m_paused);
//		m_SFXBus.setPaused (m_paused);
	}

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