using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {
	protected GameObject m_playerRef;
	public float m_duration;
	private float m_time;

	private bool m_alive;
	public bool alive{get { return m_alive; }}

	public Buff(GameObject playerRef)
	{
		m_playerRef = playerRef;
		m_alive = true;
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
	virtual public void periodicEvent()
	{
		
	}

	/**
	 * Event called when a buff is removed from the BuffManager
	 */
	virtual public void ExpireEvent()
	{
		
	}

	/**
	 * Check if the buff should be destroyed
	 */
	public void Update()
	{
		m_time += Time.deltaTime;
		if (m_time > m_duration) {
			m_alive = false;
		}
	}

}
