using UnityEngine;
using System.Collections;

public class StunBuff : Buff {
	InputHub inputHub;
	LeafBlower m_leafBlower;
	playerAnimation m_playerAnim;
	private GameObject m_stun;
	

	public StunBuff(GameObject playerRef, float duration = 1.0f):base(playerRef)
	{
		m_duration = duration;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		m_leafBlower = m_playerRef.GetComponentInChildren<LeafBlower>();
		m_playerAnim = m_playerRef.GetComponent<playerAnimation> ();
	}

	override public void InitEvent()
	{
		//animation, start stun
		m_playerAnim.stunAnim ();

//		Debug.Log ("STUN");
		m_playerRef.rigidbody.velocity = Vector3.zero;
		inputHub.StunMovement();
		inputHub.StunLeafBlower ();
		m_leafBlower.requestDropAll();

		m_stun = Instantiate(Prefactory.prefab_Stun) as GameObject;
		m_stun.transform.position = m_playerRef.transform.position;
		m_stun.transform.parent = m_playerRef.transform;
		m_stun.particleSystem.startLifetime = m_duration;
		Destroy (m_stun, m_duration);
	}

	override public void ExpireEvent()
	{
		//animation stop stun
		m_playerAnim.stopStunAnim ();

//		Debug.Log ("UNSTUN");
		inputHub.UnStunMovement();
		inputHub.UnStunLeafBlower ();
	}

	public override void RemoveEvent ()
	{
		ExpireEvent ();
		Destroy (m_stun);
	}

	public override string ToString ()
	{
		return string.Format ("[StunBuff], alive={0}]", alive);
	}

	public override int GetBuffType()
	{
		return (int)Buff.Type.STUN;
	}
}
