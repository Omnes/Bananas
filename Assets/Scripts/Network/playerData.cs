using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : System.Object {

	public int m_id;
	//public string m_ip;
	public string m_name;
	//islocal


	//rating
	//apm
	//characterType

	//kanske göra en egen serilizable calss med alla info
	//public List <> connectedPlayers = new List <prefab_player>();

	public PlayerData(string name, int id) {
		m_name = name;
		m_id = id;
	}

}
