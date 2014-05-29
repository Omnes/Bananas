using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * A global manager for handling powerups
 * Can update powerups over the network
 */
public class PowerupManager : MonoBehaviour {
	//Design parameters
	private const float INIT_SPAWN_DELAY_MIN = 15.0f;
	private const float INIT_SPAWN_DELAY_MAX = 15.0f;
	private const float SPAWN_INTERVALL_MIN = 15.0f;
	private const float SPAWN_INTERVALL_MAX = 18.0f;

	private const int MAX_POWERUPS = 2;
	private const float SPAWN_RADIUS = 6f;
	private const float MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS = 2.5f;
	private const int MAX_REPOSITION_RETRIES = 5;

	//Variables
	private static List<GameObject> m_powerups = new List<GameObject>();
	private static List<int> m_spawnablePowerups = new List<int>();
	private static NetworkView network;
	public GameObject m_powerup_prefab;

	private float spawnIntervall;                         
	private float spawnTimer = 0.0f;

	//Can spawn timer
	private static float timeBombTimer;
	private static float bigLeafBlowerTimer;
	private static float EMPTimer;
	private static int timeBombSpawnCount;
	private static bool m_canSpawn;

	/**
	 * Initialize variables
	 */
	void Start()
	{
		network = networkView;
		spawnIntervall = Random.Range (INIT_SPAWN_DELAY_MIN, INIT_SPAWN_DELAY_MAX);

		//Add all powerups that should be spawned here
		m_spawnablePowerups.Add(Powerup.TIME_BOMB);
		m_spawnablePowerups.Add(Powerup.BIG_LEAF_BLOWER);
		m_spawnablePowerups.Add(Powerup.EMP);
		timeBombTimer = TimeBombBuff.BOMB_DURATION_MAX;
		bigLeafBlowerTimer = BigLeafBlowerBuff.DURATION;
		EMPTimer = EMPBuff.DURATION;

		timeBombSpawnCount = 0;
		spawnTimer = 0.0f;
		Clear ();

		m_canSpawn = true;
	}

	/**
	 * Tells all players that a powerup was picked up and by who
	 */
	public static void SynchronizePowerupGet(GameObject player)
	{
		if (Network.isServer) {
			//Find available powerups
			List<int> m_availablePowerups = new List<int>(m_spawnablePowerups);
			if (CanSpawnTimeBomb() == false)
				m_availablePowerups.Remove(Powerup.TIME_BOMB);
			if (CanSpawnBigLeafBlower() == false)
				m_availablePowerups.Remove(Powerup.BIG_LEAF_BLOWER);
			if (CanSpawnEMP() == false)
				m_availablePowerups.Remove(Powerup.EMP);

			//Pick a random powerup from available powerups
			int playerID = player.GetComponent<SyncMovement>().getID();
			int rand = Random.Range(0, m_availablePowerups.Count);
			int powerupType = m_availablePowerups[rand];	

			//Send RPC to all players depending on powerup
			if (powerupType == Powerup.TIME_BOMB) {
				network.RPC ("TimeBombGet", RPCMode.All, playerID, TimeBombBuff.GetDuration());
				timeBombTimer = 0;
				timeBombSpawnCount++;
			}
			else if (powerupType == Powerup.BIG_LEAF_BLOWER) {
				network.RPC ("BigLeafBlowerGet", RPCMode.All, playerID);
				bigLeafBlowerTimer = 0;
			}
			else if (powerupType == Powerup.EMP) {
				network.RPC ("EMPGet", RPCMode.All, playerID);
				EMPTimer = 0;
			}
		}
	}

	public static void SynchronizePowerupGet(int playerID,string powerupRPC){
		network.RPC (powerupRPC, RPCMode.All, playerID);
	}

	/**
	 * Add the TimeBomb buff to the colliding player
	 */
	[RPC]
	public void TimeBombGet(int playerID, float duration)
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

	/**
	 * Add the BigLeafBlower buff to the colliding player
	 */
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

	/**
	 * Add the EMP buff to all player except the colliding player
	 */
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
		SoundManager.Instance.playOneShot (SoundManager.EMP);
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
	void Update ()
	{
		if (Network.isServer && m_canSpawn) {
			//Increase timers
			spawnTimer += Time.deltaTime;
			timeBombTimer += Time.deltaTime;
			bigLeafBlowerTimer += Time.deltaTime;
			EMPTimer += Time.deltaTime;

			if (spawnTimer > spawnIntervall) {

				//Räkna antalet powerups som är aktiva
				int nrAvailablePowerups = 0;
				nrAvailablePowerups += CanSpawnTimeBomb() ? 1 : 0;
				nrAvailablePowerups += CanSpawnBigLeafBlower() ? 1 : 0;
				nrAvailablePowerups += CanSpawnEMP() ? 1 : 0;

//				Debug.Log(CanSpawnTimeBomb() + "\t" + CanSpawnBigLeafBlower() + "\t" + CanSpawnEMP() + "\t" + nrAvailablePowerups + "\t" + m_powerups.Count);

				if (m_powerups.Count < MAX_POWERUPS && nrAvailablePowerups > m_powerups.Count) {
					//Randomize spawn position in a circle and with a min distance to nearby powerups
					Vector2 position = Vector2.zero;
					bool isToClose = false;
					for (int i = 0; i < MAX_REPOSITION_RETRIES; i++) {
						position = Random.insideUnitCircle * Random.Range(0, SPAWN_RADIUS);

						isToClose = false;
						foreach (GameObject powerup in m_powerups) {
							if (Vector2.Distance(position, new Vector2(powerup.transform.position.x, powerup.transform.position.z)) < MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS) {
								isToClose = true;
								break;
							}
						}
						if (isToClose == false)
							break;
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

	public static void Clear()
	{
		for (int i = 0; i < m_powerups.Count; i++) {
			Destroy(m_powerups[i]);
		}
		m_powerups.Clear ();
	}
	
	public static void Disable() {
		m_canSpawn = false;
	}

	private static bool CanSpawnTimeBomb() { 
		return (timeBombTimer > Mathf.Max(TimeBombBuff.BOMB_DURATION_MAX, INIT_SPAWN_DELAY_MIN)) &&
				(Winstate.m_timeLeft >= 25) && (Winstate.m_timeLeft <= 60) &&
				(timeBombSpawnCount == 0);
	}

	private static bool CanSpawnBigLeafBlower() { 
		return bigLeafBlowerTimer > Mathf.Max(BigLeafBlowerBuff.DURATION, INIT_SPAWN_DELAY_MIN);
	}

	private static bool CanSpawnEMP() { 
		return EMPTimer > Mathf.Max(EMPBuff.DURATION, INIT_SPAWN_DELAY_MIN);
	}
}