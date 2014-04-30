using UnityEngine;
using System.Collections;

public class EnergyDrink : Powerup {

	public override void OnPowerupGet(GameObject obj)
	{
		Debug.Log ("EnergyDrink");
		PowerupManager.SynchronizePowerupGet (Powerup.ENERGY_DRINK, obj);
//		GameObject.fi
//		obj.networkView.viewID
//		PowerupManager.Instance.PowerupGet (obj, EnergyDrink);
	}

}
