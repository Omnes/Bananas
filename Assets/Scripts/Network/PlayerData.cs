using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {

	public int m_id;
	public string m_name;
	public bool m_isLocal = false;
	public string m_guid;
	public int m_characterMesh = 0;
	//islocal


	//rating
	//apm
	//characterType

	//kanske göra en egen serilizable calss med alla info
	//public List <> connectedPlayers = new List <prefab_player>();

	public PlayerData(string name, string guid) {
		m_name = name;
		m_guid = guid;
	}

	public bool local{
		get{return m_isLocal;}
		set{m_isLocal = value;}
	}

}


//accessors
//public string name{
//	get{return m_name;}
//	set{m_name = value;}
//}
//public int id{
//	get{return m_id;}
//	set{m_id = value;}
//}

//public bool local{
//	get{return m_local;}
//	set{m_local = value;}
//}