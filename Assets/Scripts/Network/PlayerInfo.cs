using UnityEngine;
using System.Collections;

public class PlayerInfo {

	private string m_name;
	private int m_id;
	private bool m_local;

	public PlayerInfo(string name,int id, bool local){
		m_name = name;
		m_id = id;
		m_local = local;
	}

	//accessors
	public string name{
		get{return m_name;}
		set{m_name = value;}
	}
	public int id{
		get{return m_id;}
		set{m_id = value;}
	}

	public bool local{
		get{return m_local;}
		set{m_local = value;}
	}


}
