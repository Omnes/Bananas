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

	private const int MAX_POWERUPS = 3;
	private const float SPAWN_RADIUS = 12f;
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
	private static bool CanSpawnTimeBomb() { return timeBombTimer > TimeBombBuff.BOMB_DURATION_MAX; }
	private static bool CanSpawnBigLeafBlower() { return bigLeafBlowerTimer > BigLeafBlowerBuff.DURATION; }
	private static bool CanSpawnEMP() { return EMPTimer > EMPBuff.DURATION; }

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

		spawnTimer = 0.0f;
		Clear ();
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
		SoundManager.Instance.playOneShot(SoundManager.LEAFBLOWER_WARCRY);
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
	void Update () {
		if (Network.isServer) {
			//Increase timers
			spawnTimer += Time.deltaTime;
			timeBombTimer += Time.deltaTime;
			bigLeafBlowerTimer += Time.deltaTime;
			EMPTimer += Time.deltaTime;

			if (spawnTimer > spawnIntervall) {
				//Räkna antalet powerups som är aktiva
				int powerupCount = m_powerups.Count;
				powerupCount += CanSpawnTimeBomb() ? 0 : 1;
				powerupCount += CanSpawnBigLeafBlower() ? 0 : 1;
				powerupCount += CanSpawnEMP() ? 0 : 1;

				if (powerupCount < MAX_POWERUPS) {
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
		m_powerups.Clear ();
	}
}





//VIKTAT SYSTEM (HALVFÄRDIGT?)
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//
//class Pair {
//	public int m_powerup;
//	public float m_weight;
//	
//	public Pair(int powerup, float weight) {
//		m_powerup = powerup;
//		m_weight = weight;
//	}
//}
//
///**
// * A global manager for handling powerups
// * Can update powerups over the network
// */
//public class PowerupManager : MonoBehaviour {
//	//Design parameters
//	private const float INIT_SPAWN_DELAY_MIN = 0.0f;
//	private const float INIT_SPAWN_DELAY_MAX = 0.0f;
//	//	private const float SPAWN_INTERVALL_MIN = 5.0f;
//	//	private const float SPAWN_INTERVALL_MAX = 15.0f;
//	private const float SPAWN_INTERVALL_MIN = 0.0f;
//	private const float SPAWN_INTERVALL_MAX = 0.0f;
//	
//	private const int MAX_POWERUPS = 3;
//	private const float SPAWN_RADIUS = 12f;
//	private const float MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS = 2.5f;
//	private const int MAX_REPOSITION_RETRIES = 5;
//	
//	private const float TIME_BOMB_WEIGHT = 1;
//	private const float BIG_LEAF_BLOWER_WEIGHT = 1;
//	private const float EMP_WEIGHT = 1;
//	
//	
//	//Variables
//	private static List<GameObject> m_powerups = new List<GameObject>();
//	//	private static List<int> m_spawnablePowerups = new List<int>();
//	private static List<Pair> m_spawnablePowerups = new List<Pair>();
//	private static NetworkView network;
//	public GameObject m_powerup_prefab;
//	
//	private float spawnIntervall;                         
//	private float spawnTimer = 0.0f;
//	
//	//Can spawn timer
//	private static float timeBombTimer;
//	private static float bigLeafBlowerTimer;
//	private static float EMPTimer;
//	private static bool CanSpawnTimeBomb() { return timeBombTimer > TimeBombBuff.BOMB_DURATION_MAX; }
//	private static bool CanSpawnBigLeafBlower() { return bigLeafBlowerTimer > BigLeafBlowerBuff.DURATION; }
//	private static bool CanSpawnEMP() { return EMPTimer > EMPBuff.DURATION; }
//	
//	/**
//	 * Initialize variables
//	 */
//	void Start()
//	{
//		network = networkView;
//		spawnIntervall = Random.Range (INIT_SPAWN_DELAY_MIN, INIT_SPAWN_DELAY_MAX);
//		
//		//Add all powerups that should be spawned here
//		m_spawnablePowerups.Add(new Pair(Powerup.TIME_BOMB, TIME_BOMB_WEIGHT));
//		m_spawnablePowerups.Add(new Pair(Powerup.BIG_LEAF_BLOWER, BIG_LEAF_BLOWER_WEIGHT));
//		m_spawnablePowerups.Add(new Pair(Powerup.EMP, EMP_WEIGHT));
//		timeBombTimer = TimeBombBuff.BOMB_DURATION_MAX;
//		bigLeafBlowerTimer = BigLeafBlowerBuff.DURATION;
//		EMPTimer = EMPBuff.DURATION;
//		
//		spawnTimer = 0.0f;
//		Clear ();
//	}
//	
//	/**
//	 * Tells all players that a powerup was picked up and by who
//	 */
//	public static void SynchronizePowerupGet(GameObject player)
//	{
//		if (Network.isServer) {
//			//Find available powerups 
//			//INTE FINT, FIXA BÄTTRE SÄTT ATT TA BORT PAIRS UR LISTAN.
//			List<Pair> m_availablePowerups = new List<Pair>(m_spawnablePowerups);
//			if (CanSpawnTimeBomb() == false) {
//				foreach (Pair pair in m_availablePowerups) {
//					if (pair.m_powerup == Powerup.TIME_BOMB){
//						m_availablePowerups.Remove(pair); break;
//					}
//				}
//			}
//			if (CanSpawnBigLeafBlower() == false) {
//				foreach (Pair pair in m_availablePowerups) {
//					if (pair.m_powerup == Powerup.BIG_LEAF_BLOWER){
//						m_availablePowerups.Remove(pair); break;
//					}
//				}
//			}
//			if (CanSpawnEMP() == false) {
//				foreach (Pair pair in m_availablePowerups) {
//					if (pair.m_powerup == Powerup.EMP){
//						m_availablePowerups.Remove(pair); break;
//					}
//				}
//			}
//			
//			//Pick a random powerup from available powerups
//			int playerID = player.GetComponent<SyncMovement>().getID();
//			//			int rand = Random.Range(0, m_availablePowerups.Count);
//			
//			float totalWeight = 0;
//			for (int i = 0; i < m_availablePowerups.Count; i++) {
//				totalWeight += m_availablePowerups[i].m_weight;
//			}
//			float rand = Random.Range(0, totalWeight);
//			
//			int powerupType = 0; // = m_availablePowerups[rand];	
//			float curWeight = 0;
//			for (int j = 0; j < m_availablePowerups.Count; j++) {
//				powerupType = m_availablePowerups[j].m_powerup;
//				curWeight += m_availablePowerups[j].m_weight;
//				if (rand < curWeight) {
//					break;
//				}
//			}
//			
//			Debug.Log(powerupType);
//			
//			//Send RPC to all players depending on powerup
//			if (powerupType == Powerup.TIME_BOMB) {
//				network.RPC ("TimeBombGet", RPCMode.All, playerID, TimeBombBuff.GetDuration());
//				timeBombTimer = 0;
//			}
//			else if (powerupType == Powerup.BIG_LEAF_BLOWER) {
//				network.RPC ("BigLeafBlowerGet", RPCMode.All, playerID);
//				bigLeafBlowerTimer = 0;
//			}
//			else if (powerupType == Powerup.EMP) {
//				network.RPC ("EMPGet", RPCMode.All, playerID);
//				EMPTimer = 0;
//			}
//		}
//	}
//	
//	/**
//	 * Add the TimeBomb buff to the colliding player
//	 */
//	[RPC]
//	public void TimeBombGet(int playerID, float duration)
//	{
//		SyncMovement syncMovement;
//		GameObject player;
//		BuffManager buffManager;
//		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
//			if (playerID == id) {
//				syncMovement = SyncMovement.s_syncMovements[id];
//				if (syncMovement != null) {
//					player = syncMovement.gameObject;
//					buffManager = player.GetComponent<BuffManager>();
//					
//					buffManager.AddBuff(new TimeBombBuff(player, duration));
//					
//					SoundManager.Instance.StartBombMusic();
//				}
//			}
//		}
//	}
//	
//	/**
//	 * Add the BigLeafBlower buff to the colliding player
//	 */
//	[RPC]
//	public void BigLeafBlowerGet(int playerID)
//	{
//		SyncMovement syncMovement;
//		GameObject player;
//		BuffManager buffManager;
//		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
//			if (playerID == id) {
//				syncMovement = SyncMovement.s_syncMovements[id];
//				if (syncMovement != null) {
//					player = syncMovement.gameObject;
//					buffManager = player.GetComponent<BuffManager>();
//					buffManager.AddBuff(new BigLeafBlowerBuff(player));
//				}
//			}
//		}
//		SoundManager.Instance.playOneShot(SoundManager.LEAFBLOWER_WARCRY);
//	}
//	
//	/**
//	 * Add the EMP buff to all player except the colliding player
//	 */
//	[RPC]
//	public void EMPGet(int playerID)
//	{
//		SyncMovement syncMovement;
//		GameObject player;
//		BuffManager buffManager;
//		for (int id = 0; id < SyncMovement.s_syncMovements.Length; id++) {
//			syncMovement = SyncMovement.s_syncMovements[id];
//			if (syncMovement != null) {
//				player = syncMovement.gameObject;
//				buffManager = player.GetComponent<BuffManager>();
//				if (playerID != id) {
//					buffManager.AddBuff(new EMPBuff(player));
//				}
//				else {
//					Instantiate(Prefactory.prefab_EMP, player.transform.position, Prefactory.prefab_EMP.transform.rotation);
//				}
//			}
//		}
//		SoundManager.Instance.playOneShot (SoundManager.EMP);
//	}
//	
//	/**
//	 * Remove a powerup across the network and from the servers powerup list
//	 */
//	public static void Remove(GameObject powerup) {
//		m_powerups.Remove (powerup);
//		Network.Destroy (powerup.networkView.viewID);
//	}
//	
//	/**
//	 * Spawn powerups at a fixed intervall
//	 */
//	void Update () {
//		if (Network.isServer) {
//			//Increase timers
//			spawnTimer += Time.deltaTime;
//			timeBombTimer += Time.deltaTime;
//			bigLeafBlowerTimer += Time.deltaTime;
//			EMPTimer += Time.deltaTime;
//			
//			if (spawnTimer > spawnIntervall) {
//				//Räkna antalet powerups som är aktiva
//				int powerupCount = m_powerups.Count;
//				powerupCount += CanSpawnTimeBomb() ? 0 : 1;
//				powerupCount += CanSpawnBigLeafBlower() ? 0 : 1;
//				powerupCount += CanSpawnEMP() ? 0 : 1;
//				
//				if (powerupCount < MAX_POWERUPS) {
//					//Randomize spawn position in a circle and with a min distance to nearby powerups
//					Vector2 position = Vector2.zero;
//					bool isToClose = false;
//					for (int i = 0; i < MAX_REPOSITION_RETRIES; i++) {
//						position = Random.insideUnitCircle * Random.Range(0, SPAWN_RADIUS);
//						
//						isToClose = false;
//						foreach (GameObject powerup in m_powerups) {
//							if (Vector2.Distance(position, new Vector2(powerup.transform.position.x, powerup.transform.position.z)) < MIN_SPAWN_DISTANCE_BETWEEN_POWERUPS) {
//								isToClose = true;
//								break;
//							}
//						}
//						if (isToClose == false)
//							break;
//					}
//					
//					//Spawn powerup
//					if (isToClose == false) {
//						GameObject new_powerup = Network.Instantiate(m_powerup_prefab, new Vector3(position.x, 0.5f, position.y), Quaternion.identity, 0) as GameObject;
//						new_powerup.transform.parent = transform;
//						m_powerups.Add(new_powerup);
//					}
//				}
//				spawnTimer = 0;
//				spawnIntervall = Random.Range (SPAWN_INTERVALL_MIN, SPAWN_INTERVALL_MAX);
//			}
//		}
//	}
//	
//	public static void Clear()
//	{
//		m_powerups.Clear ();
//	}
//}