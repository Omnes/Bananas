﻿using UnityEngine;
using System.Collections;

public class EnergyDrink : Powerup {

	public override void OnPowerupGet(GameObject obj)
	{
		Debug.Log ("EnergyDrink");
		PowerupManager.PowerupGet (Powerup.ENERGY_DRINK, obj);
//		PowerupManager.Instance.PowerupGet (obj, EnergyDrink);
	}

}