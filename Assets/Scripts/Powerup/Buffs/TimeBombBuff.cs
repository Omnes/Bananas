﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeBombBuff : Buff {
	//Design parameters
	public const int BOMB_DURATION_MIN = 10;
	public const int BOMB_DURATION_MAX = 10;
	public const float STUN_DURATION = 1.5f;
	public const float TRANSFER_COOLDOWN = 0.5f;

	public const float SOUND_LENGTH = 1.0f;
	private static Color START_COLOR = new Color(1, 1, 0);
	private static Color END_COLOR = new Color(1, 0, 0);

	//Variables
	private GameObject m_bomb = null;
	private GameObject m_playerCircle = null;
	private GameObject m_explosion = null;

	SyncMovement m_syncMovement;
	private bool m_isLocal;

//	private List<Buff> m_targetBuffs = new List<Buff>();
	
	public TimeBombBuff(GameObject playerRef, float duration):base(playerRef)
	{
		m_duration = duration;
		m_period = 1.0f;
		m_syncMovement = playerRef.GetComponent<SyncMovement> ();
		m_isLocal = m_syncMovement.isLocal;
	}

	/**
	 * Creates a bomb over the player head and a circle at its feets
	 */
	override public void InitEvent()
	{
		m_bomb = Instantiate (Prefactory.prefab_bomb) as GameObject;
		m_bomb.transform.parent = m_playerRef.transform;
		m_bomb.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);

		if (m_isLocal) {
			m_playerCircle = Instantiate (Prefactory.prefab_playerCircle) as GameObject;
			m_playerCircle.transform.parent = m_playerRef.transform;
			m_playerCircle.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
			UpdateColor ();

//			for (int i = 0; i < SyncMovement.s_syncMovements.Length; i++) {
//				if (SyncMovement.s_syncMovements[i] != null)
//				{
//					Debug.Log("SyncMovement: " + SyncMovement.s_syncMovements[i]);
//					if (SyncMovement.s_syncMovements[i].isLocal == false) {
//						Buff b = BuffManager.m_buffManagers[i].AddBuff(new TimeBombTargetBuff(BuffManager.m_buffManagers[i].gameObject));
//						m_targetBuffs.Add(b);
//					}
//				}
//			}
		}
	}

	override public void PeriodicEvent()
	{
		SoundManager.Instance.playOneShot (SoundManager.TIMEBOMB_TICK);
		if (m_playerCircle != null) {
			UpdateColor ();
		}
	}

	/**
	 * Create an explosion and stun the player 
	 */
	override public void ExpireEvent()
	{
		RemoveObjects ();
		BuffManager buffManager = m_playerRef.GetComponent<BuffManager> ();
		buffManager.AddBuff(new StunBuff(m_playerRef, STUN_DURATION));

		m_explosion = Instantiate (Prefactory.prefab_bombExplosion,m_playerRef.transform.position,Quaternion.identity) as GameObject;
		Destroy (m_explosion, ExplosionUVAnimator.DURATION);

		SoundManager.Instance.StartIngameMusic ();
		SoundManager.Instance.playOneShot (SoundManager.TIMEBOMB_EXPLOSION);
	}

	public override void RemoveEvent ()
	{
		RemoveObjects ();
	}

	private void RemoveObjects() {
		if (m_bomb != null) {
			Destroy (m_bomb);
		} else {
			Debug.Log("SOMETHING WENT TERRIBLY WRONG 1");
		}
		if (m_playerCircle != null) {
			Destroy (m_playerCircle);
		} else {
			Debug.Log("SOMETHING WENT TERRIBLY WRONG 2");
		}

		//Ta inte bort saker i expire!
//		for (int i = 0; i < SyncMovement.s_syncMovements.Length; i++) {
//			if (BuffManager.m_buffManagers[i] != null) {
//				if (BuffManager.m_buffManagers[i].HasBuff(typeof(TimeBombTargetBuff))) {
//					BuffManager.m_buffManagers[i].GetBuff(typeof(TimeBombTargetBuff)).kill();
//					BuffManager.m_buffManagers[i].RemoveBuff(typeof(TimeBombTargetBuff));
//				}
//			}
//		}
	}

	/**
	 * Should be called after a buff transfer has occured
	 */
	public void TransferUpdate(float durationTimer) {
		m_durationTimer = durationTimer;
		if (m_playerCircle != null) {
			UpdateColor ();
		}
	}

	/**
	 * Updates the color of the circle based on the current time and duration
	 */
	private void UpdateColor() {
		Color newColor = Color.Lerp (START_COLOR, END_COLOR, m_durationTimer / m_duration);
		m_playerCircle.renderer.material.SetColor ("_Color", newColor);
	}

	public static float GetDuration() {
		return Random.Range(BOMB_DURATION_MIN, BOMB_DURATION_MAX) * SOUND_LENGTH;
	}

	/**
	 * Returns true if the buff is allowed to be transfered to another player
	 */
	public bool CanTransfer() {
		return (Time.time - m_timeCreated) > TRANSFER_COOLDOWN;
	}
}
