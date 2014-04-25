using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {
	protected GameObject m_playerRef;
	public float m_duration;
	private float m_time;

	public Buff(GameObject playerRef)
	{
		m_playerRef = playerRef;
	}

	public void InitEvent()
	{

	}

	public void periodicEvent()
	{
		
	}

	public void ExpireEvent()
	{
		
	}

	//TODO: Kalla på update från BuffManager
	public void Update()
	{
		m_time += Time.deltaTime;
		if (m_time > m_duration) {
			Destroy(this);
		}
	}

}
