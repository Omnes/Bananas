using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * A manager for handling buffs on specific objects
 */
public class BuffManager : MonoBehaviour {
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
	
//	public Buff AddUniqueBuff(Buff buff) {
//		if (HasBuff(buff.GetType())) {
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
			return m_buffs.Remove (buff);
		}
		return false;
	}

	/**
	 * Removes the first buff of the specified type (without executing its ExpireEvent)
	 */
	public bool RemoveBuff(Type buffType) {
		foreach (Buff buff in m_buffs) {
			if (buff.GetType() == buffType) {
				buff.RemoveEvent ();
				return m_buffs.Remove(buff);
			}
		}
		return false;
	}

	/**
	 * Find a buff of a specific type (if it exists) and return it
	 */
	public Buff GetBuff(Type buffType) {
		Debug.Log ("Buffs length: " + m_buffs.Count);
		foreach (Buff buff in m_buffs) {
			Debug.Log("Currently compared buff: " + buff + " buff is alive == " + buff.alive);
			Debug.Log("Comparing " + buff.GetType() + " and " + buffType);
			if (buff.GetType() == buffType) {
				Debug.Log ("Types are equal! Returning " + buff);
				return buff;
			}
		}
		Debug.Log ("Returning null");
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
			if (buff.GetType() == buffType) {
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
				m_buffs.RemoveAt(i);
				buff.ExpireEvent();
				Destroy(buff);
				i--;
			}
			else {
				buff.Update();
			}
		}
	}
}
