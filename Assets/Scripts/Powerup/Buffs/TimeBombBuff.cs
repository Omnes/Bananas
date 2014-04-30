using UnityEngine;
using System.Collections;

public class TimeBombBuff : Buff {
//	GameObject m_bomb;
	private const float BOMB_DURATION = 10.0f;
	private const float STUN_DURATION = 4.0f;

	public TimeBombBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = BOMB_DURATION;
//		m_period = 1.0f;
	}

	override public void InitEvent()
	{
//		m_bomb = Instantiate (m_bomb_prefab);
//		m_bomb.transform.parent = m_playerRef;
	}

//	override public void UpdateEvent()
//	{
//		OnCollide -> Transfer buff (and duration)
//	}

//	override public void PeriodicEvent()
//	{
//
//	}

	override public void ExpireEvent()
	{
		//Explode & stun
//		Destroy (m_bomb);
		BuffManager buffManager = m_playerRef.GetComponent<BuffManager> ();
		buffManager.Add(new StunBuff(m_playerRef, STUN_DURATION));
	}
}
