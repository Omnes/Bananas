﻿using UnityEngine;
using System.Collections;

public class TimeBombTargetBuff : Buff {
	private GameObject m_playerCircle = null;

	public TimeBombTargetBuff(GameObject playerRef):base(playerRef)
	{

	}
	
	override public void InitEvent()
	{
		m_playerCircle = Instantiate (Prefactory.prefab_playerCircle) as GameObject;
		m_playerCircle.transform.parent = m_playerRef.transform;
		m_playerCircle.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
		m_playerCircle.renderer.material.SetColor ("_Color", Color.gray);
	}

	public override void RemoveEvent ()
	{
		Destroy (m_playerCircle);
	}
}