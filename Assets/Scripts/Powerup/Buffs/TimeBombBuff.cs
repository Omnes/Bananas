using UnityEngine;
using System.Collections;

public class TimeBombBuff : Buff {
	GameObject m_prefab_bomb;
	GameObject m_bomb;

	private const int BOMB_DURATION_MIN = 4;
	private const int BOMB_DURATION_MAX = 10;
	private const float STUN_DURATION = 1.5f;

	public TimeBombBuff(GameObject playerRef, GameObject prefab_bomb):base(playerRef)
	{
		m_prefab_bomb = prefab_bomb;
		m_duration = Random.Range(BOMB_DURATION_MIN, BOMB_DURATION_MAX);
//		m_period = 1.0f;
	}

	override public void InitEvent()
	{
		//Bomb över huvudet
		//Cirkel på marken som visar hur långt det är kvar (på alla players)

		m_bomb = Instantiate (m_prefab_bomb) as GameObject;
		m_bomb.transform.parent = m_playerRef.transform;
		m_bomb.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
//		m_bomb.transform.localPosition = Vector3.zero;
//		m_bomb.transform.localPosition.Set (0.0f, 0.0f, 0.0f);
//		m_bomb.transform.position.Set (0, 0, 0);
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
		Destroy (m_bomb);
		BuffManager buffManager = m_playerRef.GetComponent<BuffManager> ();
		buffManager.Add(new StunBuff(m_playerRef, STUN_DURATION));
	}
}
