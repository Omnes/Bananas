using UnityEngine;
using System.Collections;

public class CollisionTransmitter : MonoBehaviour {
	public GameObject m_playerRef;
	public void derp() {
		Debug.Log ("derp");	//derp
	}

	public void PlayerCollision() {
		networkView.RPC ("PlayerCollisionRPC", RPCMode.All);
	}

	[RPC]
	private void PlayerCollisionRPC()
	{
		derp ();
	}

	// Use this for initialization0
//	void Start () {
//
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
