using UnityEngine;
using System.Collections;

public class ScoreParticleCaller : MonoBehaviour {
	private ID m_id;

	// Use this for initialization
	void Start () {
		m_id = GetComponent<ID> ();
	}

	public static void Play(int ID) {

	}
}
