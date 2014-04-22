using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	private const float SPAWN_INTERVALL = 1.0f;
	private float spawnTimer = 0.0f;

	public GameObject powerup;
	private const float SPAWN_RANGE = 10.0f;

//	private static PowerupManager instance;
//	public static PowerupManager Instance
//	{
//		get
//		{
//			if (instance == null) {
//				GameObject go = new GameObject();
//				instance = go.AddComponent<PowerupManager>();
//				go.name = "PowerupManager";
//			}
//			return instance;
//		}
//	}

	private List<Powerup> m_powerups = new List<Powerup>();

	public static void PowerupGet(int powerupType, GameObject player)
	{
		Debug.Log ("PowerupManager.cs: Received powerup(" + powerupType + ")");
		if (powerupType == Powerup.ENERGY_DRINK) {
//			Debug.Log("PowerupManager.cs: ENERGY DRINK");

		}
		else if (powerupType == Powerup.LAZERZ) {
//			Debug.Log("PowerupManager.cs: LAZERZ");
		}
	}

	// Use this for initialization
//	void Start () {
//
//	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		if (spawnTimer > SPAWN_INTERVALL) {
			GameObject child = Instantiate(powerup, new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.5f, Random.Range(-SPAWN_RANGE, SPAWN_RANGE)), Quaternion.identity) as GameObject;
			spawnTimer = 0;
		}
	}
}
