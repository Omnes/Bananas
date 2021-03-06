﻿using UnityEngine;
using System.Collections;

/*
 * Sets up the local playerController over the network
 * each playerController keeps track of the corresponding users characters
 * the local controllers differs from the remote ones
 */

public class InitLocalController : MonoBehaviour {

	public GameObject ControllerPrefab;

	// Use this for initialization
	void Start () {
		/*if(Network.TestConnection() == ConnectionTesterStatus.PublicIPNoServerStarted){
			Debug.LogWarning("No connection detected, might be before server is started...");
			return;
		}*/
		GameObject controller = Network.Instantiate(ControllerPrefab,Vector3.zero,Quaternion.identity,0) as GameObject;
		int localPlayerId = SeaNet.Instance.getLocalPlayer();
		PlayerInfo pi = new PlayerInfo(SeaNet.Instance.getPlayerArr()[localPlayerId].m_name,localPlayerId);
		//remote init
		controller.networkView.RPC ("RPCInitController",RPCMode.Others,pi.name,pi.id);

		//local init
		InitPlayer initPlayer = controller.GetComponent<InitPlayer>();

		GameObject localInput = GetComponent<InitLocalInput>().getLocalInput();
		if(localInput == null) Debug.LogError("could not find the object local_input");
		initPlayer.setLocal(true);
		initPlayer.setLocalInputMetod(localInput.GetComponent<InputHub>()); 

		initPlayer.setPlayerInfo(pi);
		initPlayer.init();
	}

}
