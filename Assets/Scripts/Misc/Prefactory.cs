using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prefactory : MonoBehaviour {
	public GameObject m_bomb;
	public GameObject m_playerCircle;
	public GameObject m_bombExplosion;
	public GameObject m_EMP;
	public GameObject m_EMPHit;
	public GameObject m_powerupPickup;

	public Texture2D m_muteButton;
	public Texture2D m_muteButton2;

	//winners
	public Texture2D m_winnerOtherTexture;
	public Texture2D m_winnerTexture;

	//meshes
	public List<GameObject> m_meshList = new List<GameObject>();

	public static GameObject prefab_bomb;
	public static GameObject prefab_playerCircle;
	public static GameObject prefab_bombExplosion;
	public static GameObject prefab_EMP;
	public static GameObject prefab_EMPHit;
	public static GameObject prefab_powerupPickup;

	public static Texture2D texture_muteButton;
	public static Texture2D texture_muteButton2;

	//winners
	public static Texture2D texture_winnerOther;
	public static Texture2D texture_winner;
	

	//meshes
	public static List<GameObject> prefab_meshList;
	
	void Awake () {
		prefab_bomb = m_bomb;
		prefab_playerCircle = m_playerCircle;
		prefab_bombExplosion = m_bombExplosion;
		prefab_EMP = m_EMP;
		prefab_EMPHit = m_EMPHit;
		prefab_powerupPickup = m_powerupPickup;

		texture_muteButton = m_muteButton;
		texture_muteButton2 = m_muteButton2;

		//winners
		texture_winner = m_winnerTexture;
		texture_winnerOther = m_winnerOtherTexture;

		//meshes
		prefab_meshList = m_meshList;
	}
}
