using UnityEngine;
using System.Collections;

public class DizzyBuff : Buff {
	private float interpolation;

	public DizzyBuff(GameObject playerRef, float duration = 1.0f):base(playerRef)
	{
		m_duration = duration;
		interpolation = 0.0f;
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
		m_playerRef.rigidbody.velocity *= interpolation;
	}
}
