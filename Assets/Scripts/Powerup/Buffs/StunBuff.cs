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
		inputHub.m_stunned = true;
	}

//	public override void UpdateEvent ()
//	{
//		movementLogic.enabled = false;
//	}

	override public void ExpireEvent()
	{
		inputHub.m_stunned = false;
	}
}
