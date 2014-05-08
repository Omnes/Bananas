using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * A global manager for handling powerups
 * Can update powerups over the network
 */
public class PowerupManager : MonoBehaviour {
	//Design parameters
	private const float INIT_SPAWN_DELAY_MIN = 0.0f;
	private const float INIT_SPAWN_DELAY_MAX = 0.0f;
	private const float SPAWN_INTERVALL_MIN = 5.0f;
	private const float SPAWN_INTERVALL_MAX = 15.0f;

	private const int MAX_POWERUPS = 3;
	private const float SPAWN_RADIUS = 25.0f;
	private const float MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS = 2.5f;
	private const int MAX_REPOSITION_RETRIES = 5;

	//Variables
	private static List<GameObject> m_powerups = new List<GameObject>();
	private static NetworkView network;
	public GameObject m_powerup_prefab;

	private float spawnIntervall;                         
	private float spawnTimer = 0.0f;

	/**
	 * Initialize variables
	 */
	void Awake() {
		network = networkView;
		spawnIntervall = Random.Range (INIT_SPAWN_DELAY_MIN, INIT_SPAWN_DELAY_MAX);
	}

	/**
	 * Tells all players that a powerup was picked up and by who
	 */
	public static void SynchronizePowerupGet(GameObject player)
	{
		if (Network.isServer) {
			int playerID = player.GetComponent<SyncMovement>().getID();
			int powerupType = Random.Range(0, Powerup.COUNT);
//			powerupType = Powerup.EMP;
			if (powerupType == Powerup.TIME_BOMB) {
				network.RPC ("TimeBombGet", RPCMode.All, playerID, TimeBombBuff.GetDuration());
			}
			else if (powerupType == Powerup.BIG_LEAF_BLOWER) {
				network.RPC ("BigLeafBlowerGet", RPCMode.All, playerID);
			}
			else if (powerupType == Powerup.EMP) {
				network.RPC ("EMPGet", RPCMode.All, playerID);
			}
			//TODO: Kolla så att powerupen inte redan finns i spelet
		}
	}

	[RPC]
	public void TimeBombGet(int playerID, int duration)
	{
		SyncMovement syncMovement;
		GameObject player;
		BuffManager buffManager;
		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
			if (playerID == id) {
				syncMovement = SyncMovement.s_syncMovements[id];
				if (syncMovement != null) {
					player = syncMovement.gameObject;
					buffManager = player.GetComponent<BuffManager>();

					buffManager.AddBuff(new TimeBombBuff(player, duration));

					SoundManager.Instance.StartBombMusic();
				}
			}
		}
	}

	[RPC]
	public void BigLeafBlowerGet(int playerID)
	{
		SyncMovement syncMovement;
		GameObject player;
		BuffManager buffManager;
		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
			if (playerID == id) {
				syncMovement = SyncMovement.s_syncMovements[id];
				if (syncMovement != null) {
					player = syncMovement.gameObject;
					buffManager = player.GetComponent<BuffManager>();
					buffManager.AddBuff(new BigLeafBlowerBuff(player));
				}
			}
		}
	}

	[RPC]
	public void EMPGet(int playerID)
	{
		SyncMovement syncMovement;
		GameObject player;
		BuffManager buffManager;
		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
			syncMovement = SyncMovement.s_syncMovements[id];
			if (syncMovement != null) {
				player = syncMovement.gameObject;
				buffManager = player.GetComponent<BuffManager>();
				if (playerID != id) {
					buffManager.AddBuff(new EMPBuff(player));
				}
				else {
					Instantiate(Prefactory.prefab_EMP, player.transform.position, Prefactory.prefab_EMP.transform.rotation);
				}
			}
		}
	}

	/**
	 * Remove a powerup across the network and from the servers powerup list
	 */
	public static void Remove(GameObject powerup) {
		m_powerups.Remove (powerup);
		Network.Destroy (powerup.networkView.viewID);
	}
	
	/**
	 * Spawn powerups at a fixed intervall
	 */
	void Update () {
		if (Network.isServer) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > spawnIntervall) {
				//TODO: Kolla så att det finns <3 powerups i spelet (med buffar!)
				if (m_powerups.Count < MAX_POWERUPS) {
					//Super advanced random position generator 2.0x
					Vector2 position = Vector2.zero;
					bool isToClose = false;
					for (int i = 0; i < MAX_REPOSITION_RETRIES; i++) {
						position = Random.insideUnitCircle * Random.Range(0, SPAWN_RADIUS);

						isToClose = false;
						foreach (GameObject powerup in m_powerups) {
							if (Vector2.Distance(position, new Vector2(powerup.transform.position.x, powerup.transform.position.z)) < MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS) {
								isToClose = true;
								continue;
							}
						}
						if (isToClose == false)
							continue;
					}

					//Spawn powerup
					if (isToClose == false) {
						GameObject new_powerup = Network.Instantiate(m_powerup_prefab, new Vector3(position.x, 0.5f, position.y), Quaternion.identity, 0) as GameObject;
						new_powerup.transform.parent = transform;
						m_powerups.Add(new_powerup);
					}
				}
				spawnTimer = 0;
				spawnIntervall = Random.Range (SPAWN_INTERVALL_MIN, SPAWN_INTERVALL_MAX);
			}
		}
	}
}