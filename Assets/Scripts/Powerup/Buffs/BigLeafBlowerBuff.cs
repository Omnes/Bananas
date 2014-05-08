using UnityEngine;
using System.Collections;

public class BigLeafBlowerBuff : Buff {
	//Design parameters
	private const float DURATION = 6.0f;
	private const float BLOW_WIDTH = 2.0f;

	//Variables
	private Transform airTrigger;
	private InputHub inputHub;

	/**
	 * Initialize variables
	 */
	public BigLeafBlowerBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = DURATION;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		airTrigger = m_playerRef.transform.FindChild ("air_trigger");
		if (airTrigger == null) {
			Debug.LogError("air_trigger was not found on the player");
		}
	}

	/**
	 * Incease the scale of the air trigger and remove leafblower stuns
	 */
	override public void InitEvent()
	{
		airTrigger.localScale = new Vector3 (2, 1, 1);
		airTrigger.particleSystem.emissionRate *= 2;
		airTrigger.particleSystem.startLifetime *= 1.5f;
		inputHub.ClearLeafBlowerStuns ();
		//SoundManager.playOnceShot("raoarworro");
	}

	/**
	 * Revert the scale of the air trigger to its original scale
	 */
	override public void ExpireEvent()
	{
		airTrigger.localScale = Vector3.one;
		airTrigger.particleSystem.emissionRate /= 2;
		airTrigger.particleSystem.startLifetime /= 1.5f;
	}
}