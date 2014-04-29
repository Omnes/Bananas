using UnityEngine;
using System.Collections;

/**
 * Base class for buffs
 */
public class Buff : Object {
	protected GameObject m_playerRef;
	public float m_duration;
	private float m_durationTimer;

	public float m_period;
	private float m_periodTimer;

	private bool m_alive;
	public bool alive{get { return m_alive; }}

	public Buff(GameObject playerRef)
	{
		m_playerRef = playerRef;
		m_alive = true;
		m_duration = 0.0f;
		m_durationTimer = 0.0f;
		m_period = 0.0f;
		m_periodTimer = 0.0f;
	}

	/**
	 * Event called when a buff is first added to the BuffManager
	 */
	virtual public void InitEvent()
	{

	}

	/**
	 * Event called every update
	 */
	virtual public void UpdateEvent()
	{
		
	}

	/**
	 * Event called every period
	 * Note, this event will only be executed if m_period > 0
	 */
	virtual public void PeriodicEvent()
	{
		
	}

	/**
	 * Event called when a buff is removed from the BuffManager
	 */
	virtual public void ExpireEvent()
	{
		
	}

	/**
	 * Check if the buff should be destroyed and execute events
	 */
	public void Update()
	{
		//Check periodic event
		m_periodTimer += Time.deltaTime;
		if (m_period > 0.0f && m_periodTimer > m_period) {
			PeriodicEvent();
			m_periodTimer -= m_period;
		}

		m_durationTimer += Time.deltaTime;
		if (m_durationTimer > m_duration) {
			m_alive = false;
		}
			
		UpdateEvent();
	}

}
