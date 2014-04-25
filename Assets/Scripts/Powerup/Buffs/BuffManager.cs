using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour {
	private List<Buff> m_buffs = new List<Buff>();

	// Use this for initialization
	void Start () {
	
	}

	public void Add(Buff buff) {
		m_buffs.Add (buff);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Buff buff in m_buffs) {
			buff.Update();
		}
	}
}
