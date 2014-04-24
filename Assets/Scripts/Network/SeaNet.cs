using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeaNet : MonoBehaviour {

	public List<PlayerData> m_connectedPlayers;
	public List<int> m_IDs;


	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void setConnectedPlayers(List<PlayerData> arr){
		Debug.Log("EUHEUEHEUH");
		m_connectedPlayers = arr;
		foreach(PlayerData e in m_connectedPlayers){
			m_IDs.Add(e.m_id);
		}
	}

}
