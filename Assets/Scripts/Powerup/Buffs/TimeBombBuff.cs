using UnityEngine;
using System.Collections;

public class TimeBombBuff : Buff {
	//Design parameters
	private const int BOMB_DURATION_MIN = 6;
	private const int BOMB_DURATION_MAX = 10;
	private const float STUN_DURATION = 1.5f;
	private const float TRANSFER_COOLDOWN = 0.5f;

	private static Color START_COLOR = new Color(1, 1, 0);
	private static Color END_COLOR = new Color(1, 0, 0);

	//Variables
	private GameObject m_bomb = null;
	private GameObject m_playerCircle = null;
	private GameObject m_explosion = null;

	private bool m_isLocal;	//TODO: Hämta isLocal från syncMovement

	public TimeBombBuff(GameObject playerRef, float duration):base(playerRef)
	{
		m_duration = duration;
		m_period = 1.0f;
		m_isLocal = true;
	}

	/**
	 * Creates a bomb over the player head and a circle at its feets
	 */
	override public void InitEvent()
	{
		m_bomb = Instantiate (Prefactory.prefab_bomb) as GameObject;
		m_bomb.transform.parent = m_playerRef.transform;
		m_bomb.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);

		if (m_isLocal) {
			m_playerCircle = Instantiate (Prefactory.prefab_playerCircle) as GameObject;
			m_playerCircle.transform.parent = m_playerRef.transform;
			m_playerCircle.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
			UpdateColor ();

			//TODO: Create circle at the other players
		}
	}

	override public void PeriodicEvent()
	{
		if (m_isLocal) {
			UpdateColor ();
		}
	}

	/**
	 * Create an explosion and stun the player 
	 */
	override public void ExpireEvent()
	{
		RemoveObjects ();
		BuffManager buffManager = m_playerRef.GetComponent<BuffManager> ();
		buffManager.AddBuff(new StunBuff(m_playerRef, STUN_DURATION));

		m_explosion = Instantiate (Prefactory.prefab_bombExplosion) as GameObject;
		m_explosion.transform.position = m_playerRef.transform.position;
		Destroy (m_explosion, m_explosion.particleSystem.duration);

		SoundManager.Instance.StartIngameMusic ();
	}

	public override void RemoveEvent ()
	{
		RemoveObjects ();
	}

	private void RemoveObjects() {
		Destroy (m_bomb);
		Destroy (m_playerCircle);
	}

	/**
	 * Should be called after a buff transfer has occured
	 */
	public void TransferUpdate(float durationTimer) {
		m_durationTimer = durationTimer;
		UpdateColor ();
	}

	/**
	 * Updates the color of the circle based on the current time and duration
	 */
	private void UpdateColor() {
		Color newColor = Color.Lerp (START_COLOR, END_COLOR, m_durationTimer / m_duration);
		m_playerCircle.renderer.material.SetColor ("_Color", newColor);
	}

	public static int GetDuration() {
		return Random.Range(BOMB_DURATION_MIN, BOMB_DURATION_MAX);
	}

	/**
	 * Returns true if the buff is allowed to be transfered to another player
	 */
	public bool CanTransfer() {
		return Time.time - m_timeCreated > TRANSFER_COOLDOWN;
	}
}
