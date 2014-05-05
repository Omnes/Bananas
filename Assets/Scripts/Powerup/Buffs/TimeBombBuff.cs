using UnityEngine;
using System.Collections;

public class TimeBombBuff : Buff {
	GameObject m_bomb;
	GameObject m_playerCircle;
	GameObject m_explosion;

	private const int BOMB_DURATION_MIN = 4;
	private const int BOMB_DURATION_MAX = 10;
	private const float STUN_DURATION = 1.5f;

	private static Color START_COLOR = new Color(0, 1, 0);
	private static Color END_COLOR = new Color(1, 0, 0);

	public TimeBombBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = Random.Range(BOMB_DURATION_MIN, BOMB_DURATION_MAX);
		m_period = 1.0f;
	}

	override public void InitEvent()
	{
		//Bomb över huvudet
		//Cirkel på marken som visar hur långt det är kvar (på alla players)

		m_bomb = Instantiate (Prefactory.prefab_bomb) as GameObject;
		m_bomb.transform.parent = m_playerRef.transform;
		m_bomb.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);

		m_playerCircle = Instantiate (Prefactory.prefab_playerCircle) as GameObject;
		m_playerCircle.transform.parent = m_playerRef.transform;
		m_playerCircle.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
		UpdateColor ();
	}

//	override public void UpdateEvent()
//	{
//		OnCollide -> Transfer buff (and duration)
//	}

	override public void PeriodicEvent()
	{
		UpdateColor ();
	}

	override public void ExpireEvent()
	{
		//Explode & stun
		Destroy (m_bomb);
		Destroy (m_playerCircle);
		BuffManager buffManager = m_playerRef.GetComponent<BuffManager> ();
		buffManager.Add(new StunBuff(m_playerRef, STUN_DURATION));

		m_explosion = Instantiate (Prefactory.prefab_bombExplosion) as GameObject;
		m_explosion.transform.position = m_playerRef.transform.position;
		Destroy (m_explosion, m_explosion.particleSystem.duration);
	}

	private void UpdateColor() {
		Color newColor = Color.Lerp (START_COLOR, END_COLOR, durationTimer / m_duration);
		m_playerCircle.renderer.material.SetColor ("_Color", newColor);
	}
}
