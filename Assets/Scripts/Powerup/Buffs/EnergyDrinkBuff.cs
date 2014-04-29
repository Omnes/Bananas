using UnityEngine;
using System.Collections;

public class EnergyDrinkBuff : Buff {

	public EnergyDrinkBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = 3.0f;
//		base.
//		super()
//		Buff (playerRef);
//		base.Buff (playerRef);
//		base(playerRef);
	}

	override public void InitEvent()
	{
//		base.InitEvent ();
//		playerRef.transform.speed *= 2;
		Debug.Log ("EnergyDrink: init");
	}

	override public void periodicEvent()
	{
//		Debug.Log ("EnergyDrink: periodic");
	}

	override public void ExpireEvent()
	{
		Debug.Log ("EnergyDrink: expire");
//		playerRef.transform.speed /= 2;
	}
}
