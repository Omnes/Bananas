using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prefactory : MonoBehaviour {
	public GameObject m_bomb;
	public GameObject m_playerCircle;
	public GameObject m_bombExplosion;
	public GameObject m_EMP;
	public GameObject m_EMPHit;
	public GameObject m_Stun;
	public GameObject m_powerupPickup;

	public Gradient m_rainbowColor;
	public Texture2D m_buttonAtlas;

	public Texture2D m_backGroundsNWinnerFrame;

	//winners
	public Texture2D m_winnerOtherTexture;
	public Texture2D m_winnerTexture;

	public Texture2D m_loadingscreen;
	public GUIStyle m_loadingscreenTextStyle;

	//meshes
	public List<GameObject> m_meshList = new List<GameObject>();

	public static GameObject prefab_bomb;
	public static GameObject prefab_playerCircle;
	public static GameObject prefab_bombExplosion;
	public static GameObject prefab_EMP;
	public static GameObject prefab_EMPHit;
	public static GameObject prefab_Stun;
	public static GameObject prefab_powerupPickup;

	public static Gradient gradient_rainbowColor;

	//winners
	public static Texture2D texture_winnerOther;
	public static Texture2D texture_winner;

	public static Texture2D texture_loadingscreen;
	public static Texture2D texture_buttonAtlas;
	public static GUIStyle style_loadingscreenText;

	public static Texture2D texture_backgrounds;

	//meshes
	public static List<GameObject> prefab_meshList;
	
	void Awake () {
		prefab_bomb = m_bomb;
		prefab_playerCircle = m_playerCircle;
		prefab_bombExplosion = m_bombExplosion;
		prefab_EMP = m_EMP;
		prefab_EMPHit = m_EMPHit;
		prefab_Stun = m_Stun;
		prefab_powerupPickup = m_powerupPickup;

		gradient_rainbowColor = m_rainbowColor;
		texture_buttonAtlas = m_buttonAtlas;
		texture_backgrounds = m_backGroundsNWinnerFrame;

		//winners
		texture_winner = m_winnerTexture;
		texture_winnerOther = m_winnerOtherTexture;
		texture_loadingscreen = m_loadingscreen;
		style_loadingscreenText = m_loadingscreenTextStyle ;

		//meshes
		prefab_meshList = m_meshList;
	}
}
