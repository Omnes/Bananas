using UnityEngine;
using System.Collections;

public class EMPBuff : Buff {
	//Design parameters
	public const float DURATION = 4.0f;

	//Variables
	InputHub inputHub;

	private LeafBlower m_leafBlower;
	private GameObject m_empHit;

	public EMPBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = DURATION;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		m_leafBlower = m_playerRef.GetComponentInChildren<LeafBlower>();
//		m_period = 0.25;
	}

	override public void InitEvent()
	{
//		m_playerRef.GetComponent<BuffManager> ().AddBuff (new DizzyBuff (m_playerRef, 2f));
		m_playerRef.rigidbody.velocity = Vector3.zero;

		inputHub.StunLeafBlower ();
		m_leafBlower.requestDropAll();
		m_empHit = Instantiate(Prefactory.prefab_EMPHit, m_playerRef.transform.position, Quaternion.identity) as GameObject;
		m_empHit.transform.parent = m_playerRef.transform;
		Destroy (m_empHit,4f);

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

	public override void RemoveEvent ()
	{
		ExpireEvent ();
		Destroy (m_empHit);
	}

	public override string ToString ()
	{
		return string.Format ("[EMPBuff], alive={0}]", alive);
	}

	public override int GetBuffType()
	{
		return (int)Buff.Type.EMP;
	}
}
