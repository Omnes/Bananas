using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Gör så att det endast finns en singleton/static buff manager (eftersom varje buff ändå har en playerRef)

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
		//NOTE TO SELF: Lägg inte till och ta bort saker i listan som itererar!
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
