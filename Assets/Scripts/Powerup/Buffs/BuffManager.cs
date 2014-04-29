using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * A local manager for hanlding buffs on specific objects
 */
public class BuffManager : MonoBehaviour {
	private List<Buff> m_buffs = new List<Buff>();

	// Use this for initialization
	void Start () {
	
	}

	public Buff Add(Buff buff) {
		m_buffs.Add (buff);
		buff.InitEvent ();
		return buff;
	}

	void Update () {
		//Remove dead buffs
		List<Buff> tempBuffs = new List<Buff> (m_buffs);
		foreach (Buff buff in tempBuffs) {
			if (buff.alive == false) {
				buff.ExpireEvent();
				m_buffs.Remove(buff);
				Destroy(buff);
			}
		}

		//Update buffs
		foreach (Buff buff in m_buffs) {
			buff.Update();
//			buff.PeriodicEvent();
		}
	}
}
