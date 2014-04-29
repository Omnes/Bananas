using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * A global manager for handling powerups
 * Can send/receive powerups over the network
 */
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

	/**
	 * Tells all players that a powerup was picked up and by who
	 */
	public static void SynchronizePowerupGet(int powerupType, GameObject player)
	{
		Debug.Log ("SynchronizePowerupGet");
		if (Network.isServer) {
			Debug.Log ("Sending RPC PowerupGet");
			int playerID = player.GetComponent<SyncMovement>().getID();
			network.RPC ("PowerupGet", RPCMode.All, powerupType, playerID);
		}
	}
	
	[RPC]
	public void PowerupGet(int powerupType, int playerID)
	{
		Debug.Log ("Received powerup: " + powerupType + ", " + playerID);
//		SyncMovement syncMovement = SyncMovement.s_syncMovements [playerID];

		SyncMovement syncMovement;
		GameObject player;
		BuffManager buffManager;
		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
			if (playerID == id) {
				syncMovement = SyncMovement.s_syncMovements[id];
				if (syncMovement != null) {
					player = syncMovement.gameObject;
					buffManager = player.GetComponent<BuffManager>();

					buffManager.Add(new EnergyDrinkBuff(player));
				}
			}
		}
	}
	
	/**
	 * Spawn powerups at a fixed intervall
	 */
	void Update () {
		if ( Network.isServer ) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > SPAWN_INTERVALL) {
//				GameObject child = Network.Instantiate(m_powerup, new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.5f, Random.Range(-SPAWN_RANGE, SPAWN_RANGE)), Quaternion.identity, 0) as GameObject;
				Network.Instantiate(m_powerup, new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.5f, Random.Range(-SPAWN_RANGE, SPAWN_RANGE)), Quaternion.identity, 0);
				spawnTimer = 0;
			}
		}
	}
}