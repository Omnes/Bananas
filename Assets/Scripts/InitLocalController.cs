using UnityEngine;
using System.Collections;

public class InitLocalController : MonoBehaviour {

	public GameObject ControllerPrefab;

	// Use this for initialization
	void Start () {
		/*if(Network.TestConnection() == ConnectionTesterStatus.PublicIPNoServerStarted){
			Debug.LogWarning("No connection detected, might be before server is started...");
			return;
		}*/
		GameObject controller = Network.Instantiate(ControllerPrefab,Vector3.zero,Quaternion.identity,0) as GameObject;
		PlayerInfo pi = new PlayerInfo("Robin",0);
		//remote init
		controller.networkView.RPC ("RPCInitController",RPCMode.Others,pi.name,pi.id);

		//local init
		InitPlayer initPlayer = controller.GetComponent<InitPlayer>();

		//min själ gjorde ont när jag skrev det här - byt mycket gärna ut mot en vettig lösning!
		GameObject localInput = GameObject.Find("local_input");
		if(localInput == null) Debug.LogError("could not find the object local_input");
		initPlayer.setLocal(true);
		initPlayer.setLocalInputMetod(localInput.GetComponent<InputHub>()); 

		initPlayer.setPlayerInfo(pi);
		initPlayer.init();
	}

}
