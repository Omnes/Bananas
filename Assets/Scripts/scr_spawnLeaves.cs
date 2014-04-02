using UnityEngine;
using System.Collections;

public class scr_spawnLeaves : MonoBehaviour {
	public GameObject prefab_leaf;
	// Use this for initialization
	void Start () {
		float range = 5f;
		for (int x = 0; x < 100; x++) {
			Instantiate(prefab_leaf, new Vector3(Random.Range(-range, range), 0.2f + x * 0.01f, Random.Range(-range, range)), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
