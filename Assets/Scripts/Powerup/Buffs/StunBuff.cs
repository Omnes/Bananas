using UnityEngine;
using System.Collections;

public class StunBuff : Buff {
	InputHub inputHub;
	LeafBlower m_leafBlower;

	public StunBuff(GameObject playerRef, float duration = 1.0f):base(playerRef)
	{
		m_duration = duration;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		m_leafBlower = m_playerRef.GetComponentInChildren<LeafBlower>();

	}

	override public void InitEvent()
	{
//		Debug.Log ("STUN");
		m_playerRef.rigidbody.velocity = Vector3.zero;
		inputHub.StunMovement();
		inputHub.StunLeafBlower ();
		m_leafBlower.requestDropAll();
	}

	override public void ExpireEvent()
	{
//		Debug.Log ("UNSTUN");
		inputHub.UnStunMovement();
		inputHub.UnStunLeafBlower ();
	}

	public override string ToString ()
	{
		return string.Format ("[StunBuff], alive={0}]", alive);
	}

	public override int GetType ()
	{
		return (int)Buff.Type.STUN;
	}
}
