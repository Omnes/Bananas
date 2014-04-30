using UnityEngine;
using System.Collections;

public class BigLeafBlowerBuff : Buff {
//	InputHub inputHub;
	Transform airTrigger;

	public BigLeafBlowerBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = 6.0f;
		airTrigger = m_playerRef.transform.GetChild (0);
	}

	override public void InitEvent()
	{
//		airTrigger.transform.localScale.Set (2.0f, 2.0f, 2.0f);
//		airTrigger.transform.lossyScale.Set (2.0f, 2.0f, 2.0f);
	}

//	public override void UpdateEvent ()
//	{
//		movementLogic.enabled = false;
//	}

	override public void ExpireEvent()
	{
//		airTrigger.transform.localScale.Set (1.0f, 1.0f, 1.0f);
//		airTrigger.transform.lossyScale.Set (1.0f, 1.0f, 1.0f);
	}
}