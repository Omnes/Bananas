using UnityEngine;
using System.Collections;

public class FootstepSound : MonoBehaviour {
//	private FMOD_StudioEventEmitter m_footstepEmitter;
	private FMOD.Studio.EventInstance m_footstepSound;
	private const float MAX_SPEED = 10;


	// Use this for initialization
	void Start () {
		for (int i = 0; i < SyncMovement.s_syncMovements.Length; i++) {
			if (SyncMovement.s_syncMovements[i] != null)
			{
				if (SyncMovement.s_syncMovements[i].isLocal)
				{
					m_footstepSound = SoundManager.Instance.play(SoundManager.FOOTSTEP);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (m_footstepSound != null)
		{
			float totalSpeed = Mathf.Clamp(rigidbody.velocity.magnitude / MAX_SPEED, 0, 1);
			m_footstepSound.setVolume (totalSpeed);
		}
	}
}
