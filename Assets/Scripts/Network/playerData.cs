using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public int id;
	//rating
	//apm
	//characterType

	void Start () {
		//fungerar det att sätta dontdestroy på det egna objectet?
		DontDestroyOnLoad(gameObject);
	}

	void Update () {
	
	}
}
