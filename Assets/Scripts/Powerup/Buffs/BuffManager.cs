using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * A manager for handling buffs on specific objects
 */
public class BuffManager : MonoBehaviour {
	public static BuffManager[] m_buffManagers = new BuffManager[4];
	private List<Buff> m_buffs = new List<Buff>();

	/**
	 * Add a buff
	 * The buff will automatically be updated & destroyed
	 */
	public Buff AddBuff(Buff buff) {
		m_buffs.Add (buff);
		buff.InitEvent ();
		return buff;
	}
	
//	public Buff AddBuff(Buff buff) {
//		if (HasBuff(buff.GetType()) == false) {
//			m_buffs.Add (buff);
//			buff.InitEvent ();
//			return buff;
//		}
//		return null;
//	}

	/**
	 * Removes a buff (without executing its ExpireEvent)
	 */
	public bool RemoveBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetType() == buffType && m_buffs[i].alive) {
				m_buffs[i].RemoveEvent();
				m_buffs.RemoveAt (i);
				Destroy(m_buffs[i]);
				return true;
			}
		}
		return false;
	}
//	public bool RemoveBuff(Buff buff) {
//		if (m_buffs.Contains(buff)) {
//			buff.RemoveEvent ();
//			m_buffs.Remove (buff);
//			Destroy(buff);
//			return true;
//		}
//		return false;
//	}

	/**
	 * Removes the first buff of the specified type (without executing its ExpireEvent)
	 */
//	public bool RemoveBuff(Type buffType) {
//		foreach (Buff buff in m_buffs) {
//			if (buff.GetType() == buffType && buff.alive) {
//				buff.RemoveEvent ();
//				m_buffs.Remove(buff);
//				Destroy(buff);
//				return true;
//			}
//		}
//		return false;
//	}

	/**
	 * Find a buff of a specific type (if it exists) and return it
	 */
	public Buff GetBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetType() == buffType && m_buffs[i].alive) {
				return m_buffs[i];
			}
		}
		return null;
	}

//	public Buff GetBuff(Type buffType) {
//		foreach (Buff buff in m_buffs) {
//			if (buff.GetType() == buffType && buff.alive) {
//				return buff;
//			}
//		}
//		return null;
//	}

	/**
	 * Find a buff of a specific type (if it exists) and return it
	 */
//	public Buff GetBuff(Type buffType) {
//		for (int i = 0; i < m_buffs.Count; i++) {
//			Buff buff = m_buffs[i];
//			if (buff.GetType() == buffType) {
//				return buff;
//			}
//		}
//		return null;
//	}

	/**
	 * Returns true if the manager contains a buff of the specified type
	 */
	public bool HasBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetType() == buffType && m_buffs[i].alive) {
				return true;
			}
		}
		return false;
	}
//	public bool HasBuff(Type buffType) {
//		foreach (Buff buff in m_buffs) {
//			if (buff.GetType() == buffType && buff.alive) {
//				return true;
//			}
//		}
//		return false;
//	}

	/**
	 * Updates and removes buffs
	 */
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			for (int i = 0; i < 2; i++) {
				AddBuff( new StunBuff(gameObject, 1.0f) );
			}
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			for (int i = 0; i < 5; i++) {
				AddBuff( new DizzyBuff(gameObject, 1.0f) );
			}
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			for (int i = 0; i < 2; i++) {
				AddBuff( new TimeBombBuff(gameObject, 1.0f) );
			}
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			for (int i = 0; i < 2; i++) {
				AddBuff( new EMPBuff(gameObject) );
			}
		}

		//########################################
		List<Buff> m_deadBuff = new List<Buff>();

		for (int i = 0; i < m_buffs.Count; i++) {
			m_buffs[i].Update();
		}

		int buffLength = m_buffs.Count;
		for (int i = 0; i < buffLength; i++) {
			if (m_buffs[i].alive == false) {
				m_buffs[i].ExpireEvent();
				m_deadBuff.Add(m_buffs[i]);
			}
		}

		int deadBuffLength = m_deadBuff.Count;
		for (int i = 0; i < deadBuffLength; i++) {
			for (int k = 0; k < m_buffs.Count; k++) {
				if ( m_buffs[k].uid == m_deadBuff[i].uid) {
					m_buffs.RemoveAt(k);
					k--;
				}
			}
		}
	}
}
