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

	/**
	 * Removes a buff (without executing its ExpireEvent)
	 */
	public bool RemoveBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetBuffType() == buffType && m_buffs[i].alive) {
				m_buffs[i].Remove();
				return true;
			}
		}
		return false;
	}

	/**
	 * Find a buff of a specific type (if it exists) and return it
	 */
	public Buff GetBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetBuffType() == buffType && m_buffs[i].alive) {
				return m_buffs[i];
			}
		}
		return null;
	}

	/**
	 * Returns true if the manager contains a buff of the specified type
	 */
	public bool HasBuff(int buffType) {
		for (int i = 0; i < m_buffs.Count; i++) {
			if (m_buffs[i].GetBuffType() == buffType && m_buffs[i].alive) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Updates and removes buffs
	 */
	void Update () {
//		if (Input.GetKeyDown(KeyCode.Q)) {
//			for (int i = 0; i < 2; i++) {
//				AddBuff( new StunBuff(gameObject, 1.0f) );
//			}
//		}
//		if (Input.GetKeyDown(KeyCode.W)) {
//			for (int i = 0; i < 5; i++) {
//				AddBuff( new DizzyBuff(gameObject, 1.0f) );
//			}
//		}
//		if (Input.GetKeyDown(KeyCode.E)) {
//			AddBuff( new TimeBombBuff(gameObject, 12.0f) );
//		}
//		if (Input.GetKeyDown(KeyCode.R)) {
//			for (int i = 0; i < 2; i++) {
//				AddBuff( new EMPBuff(gameObject) );
//			}
//		}

		//########################################
		List<Buff> m_deadBuff = new List<Buff>();

		for (int i = 0; i < m_buffs.Count; i++) {
			m_buffs[i].Update();
		}

		int buffLength = m_buffs.Count;
		for (int i = 0; i < buffLength; i++) {
			if (m_buffs[i].alive == false) {
				if (m_buffs[i].remove == false) {
					m_buffs[i].ExpireEvent();
				}
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
