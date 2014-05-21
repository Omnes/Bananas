using UnityEngine;
using System.Collections;



/**
 * Base class for buffs
 */
public class Buff : UnityEngine.Object {
	public enum Type {
		BIG_LEAF_BLOWER,
		DIZZY,
		EMP,
		STUN,
		TIME_BOMB,
		TIME_BOMB_TARGET
	}

	public GameObject m_playerRef;

	public float m_duration;
	public float m_durationTimer;
	public float m_timeCreated;

	public float m_period;
	private float m_periodTimer;

	private bool m_remove;
	public bool remove{ get { return m_remove; } }
	public void Remove() { m_remove = true; }

	private bool m_alive;
	public bool alive{get { return m_alive; }}
	public void Kill() { m_alive = false; }

	public int uid = -1;
	private static int ID = 0;
	private static int GetUniqueID() {return ID++;}
	
	public Buff(GameObject playerRef)
	{
		m_playerRef = playerRef;
		m_alive = true;
		m_remove = false;
		m_duration = 0.0f;
		m_durationTimer = 0.0f;
		m_period = 0.0f;
		m_periodTimer = 0.0f;
		m_timeCreated = Time.time;

		uid = GetUniqueID ();
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
		if ((m_duration > 0.0f && m_durationTimer > m_duration) || m_remove) {
			if (m_remove && m_alive) {
				RemoveEvent();
			}
			m_alive = false;
		}
			
		UpdateEvent();
	}

	public override string ToString ()
	{
		return string.Format ("[Buff: durationTimer={0}, alive={1}]", m_durationTimer, alive);
	}

	virtual public int GetType() {
		Debug.LogError ("OVERRIDE THIS FUNCTION!");
		return -1;
	}

}
