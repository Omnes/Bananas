using UnityEngine;
using System.Collections;

/**
 * Base class for buffs
 */
public class Buff : Object {
	public GameObject m_playerRef;
	public float m_duration;
	public float m_durationTimer;
	public float m_timeCreated;

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
		m_timeCreated = Time.time;
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
	 * Event called when a buff is removed due to duration expire
	 */
	virtual public void ExpireEvent()
	{
		
	}

	/**
	 * Event called when a buff is removed from the BuffManager (using a remove function)
	 */
	virtual public void RemoveEvent()
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
		if (m_duration > 0.0f && m_durationTimer > m_duration) {
			m_alive = false;
		}
			
		UpdateEvent();
	}

	public override string ToString ()
	{
		return string.Format ("[Buff: durationTimer={0}, alive={1}]", m_durationTimer, alive);
	}

}
