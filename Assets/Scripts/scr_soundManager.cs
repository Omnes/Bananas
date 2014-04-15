using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Lägg till destroyTime parameter
//TODO: Play funktion som inte tar bort ljudet
//TODO: Ha koll på hur många av varje ljud som spelas upp så att man kan sätta tex, max 2 ljud får spelas samtidigt
public class scr_soundManager : MonoBehaviour {
	private static scr_soundManager instance;
	public static scr_soundManager Instance
	{
		get
		{
			if (instance == null) {
				GameObject go = new GameObject();
				instance = go.AddComponent<scr_soundManager>();
				go.name = "Sound Manager";
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
		sound.stop ();
		sound.release ();
		m_sounds.Remove (sound);
	}

}