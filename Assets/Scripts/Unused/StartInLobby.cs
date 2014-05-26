using UnityEngine;
using System.Collections;

public class StartInLobby : MonoBehaviour {

	public string lobbyName = "Sean_Scene";

	public static bool hasStarted = false;

	// Use this for initialization
	void Start () {
		if(hasStarted == false && Application.isEditor){
			hasStarted = true;
			Application.LoadLevel(lobbyName);
		}
	
	}

}
