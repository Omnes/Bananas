using UnityEngine;
using System.Collections;

public class BigLeafBlowerBuff : Buff {
	//Design parameters
	public const float DURATION = 6.0f;
	public const float BLOW_WIDTH = 2.0f;

	//Variables
	private Transform airTrigger;
	private Transform blowParticles;
	private InputHub inputHub;
	private LeafBlower leafBlower;
	private float preSpeedModifier;

	private Color m_preColor;

	/**
	 * Initialize variables
	 */
	public BigLeafBlowerBuff(GameObject playerRef):base(playerRef)
	{
		m_duration = DURATION;
		inputHub = m_playerRef.GetComponent<InputHub> ();
		blowParticles = m_playerRef.transform.FindChild ("air_trigger/blowParticles");
		airTrigger = blowParticles.parent;
		leafBlower = airTrigger.GetComponent<LeafBlower> ();
	}

	/**
	 * Incease the scale of the air trigger and remove leafblower stuns
	 */
	override public void InitEvent()
	{
		airTrigger.localScale = new Vector3 (2, 1, 1);
		blowParticles.localScale = new Vector3 (0.5f, 1, 1);
		m_preColor = blowParticles.particleSystem.startColor;
		inputHub.ClearLeafBlowerStuns ();
		preSpeedModifier = leafBlower.m_lowestSpeedModifier;
		leafBlower.m_lowestSpeedModifier = 1.0f;
		SoundManager.Instance.playOneShot(SoundManager.LEAFBLOWER_WARCRY);
	}

	public override void UpdateEvent ()
	{
		//FAAAAABOLOOUUUUUS'ify
		blowParticles.particleSystem.startColor = new Color (Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
	}

	/**
	 * Revert the scale of the air trigger to its original scale
	 */
	override public void ExpireEvent()
	{
		airTrigger.localScale = Vector3.one;
		blowParticles.localScale = Vector3.one;
		blowParticles.particleSystem.startColor = m_preColor;
		leafBlower.m_lowestSpeedModifier = preSpeedModifier;
	}

	public override void RemoveEvent ()
	{
		ExpireEvent ();
	}

	public override string ToString ()
	{
		return string.Format ("[BigLeafBlowerBuff], alive={0}]", alive);
	}

	public override int GetBuffType()
	{
		return (int)Buff.Type.BIG_LEAF_BLOWER;
	}
}