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
	public bool RemoveBuff(Buff buff) {
		if (m_buffs.Contains(buff)) {
			buff.RemoveEvent ();
			m_buffs.Remove (buff);
			Destroy(buff);
			return true;
		}
		return false;
	}

	/**
	 * Removes the first buff of the specified type (without executing its ExpireEvent)
	 */
	public bool RemoveBuff(Type buffType) {
		foreach (Buff buff in m_buffs) {
			if (buff.GetType() == buffType && buff.alive) {
				buff.RemoveEvent ();
				m_buffs.Remove(buff);
				Destroy(buff);
				return true;
			}
		}
		return false;
	}

	/**
	 * Find a buff of a specific type (if it exists) and return it
	 */
	public Buff GetBuff(Type buffType) {
		foreach (Buff buff in m_buffs) {
			if (buff.GetType() == buffType && buff.alive) {
				return buff;
			}
		}
		return null;
	}

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
	public bool HasBuff(Type buffType) {
		foreach (Buff buff in m_buffs) {
			if (buff.GetType() == buffType && buff.alive) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Updates and removes buffs
	 */
	void Update () {
		for (int i = 0; i < m_buffs.Count; i++) {
			Buff buff = m_buffs[i];
			if (buff.alive == false) {
				buff.ExpireEvent();
				m_buffs.RemoveAt(i);
//				Destroy(buff);
				DestroyObject(buff);
				i--;
			}
			else {
				buff.Update();
			}
		}

//		List<Buff> m_deadBuffs = new List<Buff>();
//
//		for (int i = 0; i < m_buffs.Count; i++) {
//			if (m_buffs[i].alive == false) {
//				m_deadBuffs.Add(m_buffs[i]);
//			}
//			else {
//				m_buffs[i].Update();
//			}
//		}
//		for (int i = 0; i < m_deadBuffs.Count; i++) {
//			m_deadBuffs[i].ExpireEvent();
//			m_buffs.Remove(m_deadBuffs[i]);
//			Destroy(m_deadBuffs[i]);
//		}
	}
}
