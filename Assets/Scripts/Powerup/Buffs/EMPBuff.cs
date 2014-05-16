using UnityEngine;
using System.Collections;

public class EMPBuff : Buff {
	//Design parameters
	public const float DURATION = 4.0f;

	//Variables
	InputHub inputHub;

	private LeafBlower m_leafBlower;

	public EMPBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = DURATION;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		m_leafBlower = m_playerRef.GetComponentInChildren<LeafBlower>();
//		m_period = 0.25;
	}

	override public void InitEvent()
	{
		m_playerRef.rigidbody.velocity = Vector3.zero;
		inputHub.StunLeafBlower ();
		m_leafBlower.requestDropAll();
	}

//	public override void PeriodicEvent ()
//	{
//		m_period = 0.0;
//		inputHub.StunLeafBlower ();
//		m_playerRef.rigidbody.velocity = Vector3.zero;
//	}

	override public void ExpireEvent()
	{
		inputHub.UnStunLeafBlower ();
	}
}
