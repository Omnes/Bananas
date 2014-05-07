using UnityEngine;
using System.Collections;

public class Prefactory : MonoBehaviour {
	public GameObject m_bomb;
	public GameObject m_playerCircle;
	public GameObject m_bombExplosion;
	public GameObject m_EMP;

	public static GameObject prefab_bomb;
	public static GameObject prefab_playerCircle;
	public static GameObject prefab_bombExplosion;
	public static GameObject prefab_EMP;
	
	void Awake () {
		prefab_bomb = m_bomb;
		prefab_playerCircle = m_playerCircle;
		prefab_bombExplosion = m_bombExplosion;
		prefab_EMP = m_EMP;
	}
}
