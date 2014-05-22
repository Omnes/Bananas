using UnityEngine;
using System.Collections;

public class InitLocalInput : MonoBehaviour {

	public GameObject m_inputPrefab;
	
	private GameObject m_input;


	void Start () {
		m_input = Instantiate(m_inputPrefab) as GameObject;
		m_input.name = "local_input";
	}

	public GameObject getLocalInput(){
		return m_input;
	}

}
