using UnityEngine;
using System.Collections;

public class StunBuff : Buff {
	InputHub inputHub;

	public StunBuff(GameObject playerRef, float duration = 1.0f):base(playerRef)
	{
		m_duration = duration;
		inputHub = m_playerRef.GetComponent<InputHub> ();
	}

	override public void InitEvent()
	{
		m_playerRef.rigidbody.velocity = Vector3.zero;
		inputHub.StunMovement();
		inputHub.StunLeafBlower ();
	}

	override public void ExpireEvent()
	{
		inputHub.UnStunMovement();
		inputHub.UnStunLeafBlower ();
	}
}
