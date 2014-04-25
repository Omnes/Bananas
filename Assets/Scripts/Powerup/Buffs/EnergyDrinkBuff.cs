using UnityEngine;
using System.Collections;

public class EnergyDrinkBuff : Buff {

	public EnergyDrinkBuff(GameObject playerRef):base(playerRef)
	{
//		base.
//		super()
//		Buff (playerRef);
//		base.Buff (playerRef);
//		base(playerRef);
	}

	public void InitEvent()
	{
//		base.InitEvent ();
		//playerRef.transform.speed *= 2;
		Debug.Log ("EnergyDrink: init");
	}

	public void periodicEvent()
	{
		Debug.Log ("EnergyDrink: periodic");
	}

	public void ExpireEvent()
	{
		Debug.Log ("EnergyDrink: expire");
		//playerRef.transform.speed /= 2;
	}
}
