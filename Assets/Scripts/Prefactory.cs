using UnityEngine;
using System.Collections;

public class Prefactory : MonoBehaviour {
	public GameObject m_bomb;
	public GameObject m_playerCircle;
	public GameObject m_bombExplosion;
	public GameObject m_EMP;

	//meshes
	public GameObject m_douglas;
	public GameObject m_leif;
	public GameObject m_jessica;
	public GameObject m_sarah;

	public static GameObject prefab_bomb;
	public static GameObject prefab_playerCircle;
	public static GameObject prefab_bombExplosion;
	public static GameObject prefab_EMP;

	//meshes
	public static GameObject prefab_douglas;
	public static GameObject prefab_leif;
	public static GameObject prefab_jessica;
	public static GameObject prefab_sarah;
	
	void Awake () {
		prefab_bomb = m_bomb;
		prefab_playerCircle = m_playerCircle;
		prefab_bombExplosion = m_bombExplosion;
		prefab_EMP = m_EMP;

		//meshes
		prefab_douglas = m_douglas;
		prefab_leif = m_leif;
		prefab_jessica = m_jessica;
		prefab_sarah = m_sarah;
	}
}
