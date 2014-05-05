using UnityEngine;
using System.Collections;

public class Prefactory : MonoBehaviour {
	public GameObject m_bomb;
	public GameObject m_playerCircle;
	public GameObject m_bombExplosion;

	public static GameObject prefab_bomb;
	public static GameObject prefab_playerCircle;
	public static GameObject prefab_bombExplosion;
	
	void Awake () {
		prefab_bomb = m_bomb;
		prefab_playerCircle = m_playerCircle;
		prefab_bombExplosion = m_bombExplosion;
	}
}
