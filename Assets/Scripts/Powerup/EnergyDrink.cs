using UnityEngine;
using System.Collections;

public class EnergyDrink : Powerup {

	public override void OnPowerupGet(GameObject obj)
	{
//		PowerupManager.Instance.PowerupGet (obj, EnergyDrink);
		Debug.Log ("EnergyDrink");
	}

}
