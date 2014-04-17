using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	private const float SPAWN_INTERVALL = 0.1f;
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

	public void PowerupGet(GameObject player, Powerup powerup)
	{

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		if (spawnTimer > SPAWN_INTERVALL) {
//			Debug.Log( "Spawn powerup!" );
//			for (int x = 0; x < m_leafCount; x++) {
			GameObject child = Instantiate(powerup, new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), 0.5f, Random.Range(-SPAWN_RANGE, SPAWN_RANGE)), Quaternion.identity) as GameObject;
//				child.transform.parent = m_parent;
//				child.name = "leaf_"+x;
//				child.renderer.sharedMaterial = m_leafMaterial;
//				child.transform.Rotate(new Vector3(0,0,Random.Range(0f,359f))); 
//			}

			spawnTimer = 0;
		}
	}
}
