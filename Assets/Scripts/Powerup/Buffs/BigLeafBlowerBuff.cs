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

	private Color m_preColor;
	private float m_preLifetime;
	private float m_preSpeedModifier;

	private Gradient m_rainbow;
//	private GradientColorKey[] m_rainbowGCK;
//	private GradientAlphaKey[] m_rainbowGAK;

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

		m_rainbow = Prefactory.gradient_rainbowColor;
//		m_rainbow = new Gradient ();
//		m_rainbowGCK = new GradientColorKey[6];
//		m_rainbowGCK [0].color = new Color (255, 0, 0);
//		m_rainbowGCK [0].time = 0.0f;
//		m_rainbowGCK [1].color = new Color (208, 75, 255);
//		m_rainbowGCK [1].time = 0.279f;
//		m_rainbowGCK [2].color = new Color (0, 255, 244);
//		m_rainbowGCK [2].time = 0.474f;
//		m_rainbowGCK [3].color = new Color (10, 255, 22);
//		m_rainbowGCK [3].time = 0.635f;
//		m_rainbowGCK [4].color = new Color (251, 255, 0);
//		m_rainbowGCK [4].time = 0.768f;
//		m_rainbowGCK [5].color = new Color (255, 0, 0);
//		m_rainbowGCK [5].time = 1.0f;
//
//		m_rainbowGAK = new GradientAlphaKey[1];
//		m_rainbowGAK[0].alpha = 0.564f;
//		m_rainbowGAK[0].time = 0.0f;
//		
//		m_rainbow.SetKeys(m_rainbowGCK, m_rainbowGAK);
	}

	/**
	 * Incease the scale of the air trigger and remove leafblower stuns
	 */
	override public void InitEvent()
	{
		airTrigger.localScale = new Vector3 (2, 1, 1);
		blowParticles.localScale = new Vector3 (0.5f, 1, 1);

		m_preColor = blowParticles.particleSystem.startColor;
		m_preLifetime = blowParticles.particleSystem.startLifetime;
		m_preSpeedModifier = leafBlower.m_lowestSpeedModifier;

		inputHub.ClearLeafBlowerStuns ();
		blowParticles.particleSystem.startLifetime += 0.15f;
		leafBlower.m_lowestSpeedModifier = 1.0f;
		SoundManager.Instance.playOneShot(SoundManager.LEAFBLOWER_WARCRY);
	}

	public override void UpdateEvent ()
	{
		//FAAAAABOLOOUUUUUS'ify
		blowParticles.particleSystem.startColor = m_rainbow.Evaluate (Time.time % 1);
	}

	/**
	 * Revert the scale of the air trigger to its original scale
	 */
	override public void ExpireEvent()
	{
		airTrigger.localScale = Vector3.one;
		blowParticles.localScale = Vector3.one;

		blowParticles.particleSystem.startColor = m_preColor;
		blowParticles.particleSystem.startLifetime = m_preLifetime;
		leafBlower.m_lowestSpeedModifier = m_preSpeedModifier;
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