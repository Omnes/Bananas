using UnityEngine;
using System.Collections;

public class DizzyBuff : Buff {
	private float interpolation;
	private MovementLogic m_movementLogic;

	private float m_minSpeed = 2.5f;

	public DizzyBuff(GameObject playerRef, float duration = 1.0f):base(playerRef)
	{
		m_duration = duration;
		interpolation = 0.0f;
		m_movementLogic = m_playerRef.GetComponent<MovementLogic> ();
	}

	override public void InitEvent()
	{
		m_playerRef.rigidbody.velocity = Vector3.zero;
	}

	public override void UpdateEvent ()
	{
		interpolation += Time.deltaTime / m_duration;
		if (interpolation > 1.0f) {
			interpolation = 1.0f;
		}
		m_movementLogic.m_maxSpeed = m_minSpeed + (m_movementLogic.m_origMaxSpeed - m_minSpeed) * interpolation;
	}

	public override string ToString ()
	{
		return string.Format ("[DizzyBuff], alive={0}]", alive);
	}

	public override int GetBuffType()
	{
		return (int)Buff.Type.DIZZY;
	}
}
