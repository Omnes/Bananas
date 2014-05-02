using UnityEngine;
using System.Collections;

public class EnergyDrinkBuff : Buff {
	//Bomb, emp, big leafblower
	public EnergyDrinkBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = 3.0f;
//		m_period = 1.0f;
//		base.
//		super()
//		Buff (playerRef);
//		base.Buff (playerRef);
//		base(playerRef);
	}

	override public void InitEvent()
	{
		Debug.Log ("EnergyDrink: init");
//		m_playerRef.transform.GetChild (0).Rotate (Vector3.forward);
//		m_playerRef.transform.localScale.y = 2.0f;
//		m_playerRef.transform.sca
//		base.InitEvent ();
//		playerRef.transform.speed *= 2;
//		Network.time
	}

	override public void UpdateEvent()
	{
		m_playerRef.transform.GetChild (0).Rotate (Vector3.forward * Time.deltaTime * 60);

//		Debug.Log ("EnergyDrink: periodic");
	}

	override public void PeriodicEvent()
	{
//		Debug.Log ("EnergyDrink: period");
	}

	override public void ExpireEvent()
	{
		Debug.Log ("EnergyDrink: expire");
//		m_playerRef.transform.localScale.y = 1.0f;
//		m_playerRef.transform.lossyScale.Set (1.0f, 1.0f, 1.0f);
//		playerRef.transform.speed /= 2;
	}
}
