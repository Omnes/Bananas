using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	private const float SPAWN_INTERVALL = 2.5f;
	private float spawnTimer = 0.0f;

	public GameObject m_powerup;
	private const float SPAWN_RANGE = 10.0f;

	private static NetworkView network;

//	private List<Powerup> m_powerups = new List<Powerup>();

	void Awake() {
		network = networkView;
	}

	public static void SynchronizePowerupGet(int powerupType, GameObject player)
	{
		Debug.Log ("SynchronizePowerupGet");
		if (Network.isServer) {
			Debug.Log ("Sending RPC PowerupGet");
//			network.RPC ("PowerupGet", RPCMode.All, powerupType, player.GetInstanceID());
			network.RPC ("PowerupGet", RPCMode.All, powerupType, player.networkView.viewID);
		}

//		Debug.Log ("PowerupManager.cs: Received powerup(" + powerupType + ")");
//		if (powerupType == Powerup.ENERGY_DRINK) {
//			Debug.Log("PowerupManager.cs: ENERGY DRINK");

//		}
//		else if (powerupType == Powerup.LAZERZ) {
//			Debug.Log("PowerupManager.cs: LAZERZ");
//		}

//		Player
	}

	/**
	 * Tells all players that a powerup was picked up and by who
	 **/
	[RPC]
	public void PowerupGet(int powerupType, int playerID)
	{
		Debug.Log ("Received powerup: " + powerupType + ", " + playerID);
	}
	
	/**
	 * Spawn powerups at a fixed intervall
	 **/
	void Update () {
		if ( Network.isServer ) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > SPAWN_INTERVALL) {
				GameObject child = Network.Instantiate(m_powerup, new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.5f, Random.Range(-SPAWN_RANGE, SPAWN_RANGE)), Quaternion.identity, 0) as GameObject;
				spawnTimer = 0;
			}
		}
	}
}
