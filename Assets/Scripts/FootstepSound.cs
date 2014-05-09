using UnityEngine;
using System.Collections;

//TODO: Ta bort ControllerCreator, GUICamera, local_inutp

public class FootstepSound : MonoBehaviour {
	private FMOD_StudioEventEmitter footstepEmitter;
	//	FMOD.Studio.ParameterInstance footstepParam;


	// Use this for initialization
	void Start () {
//		footstepEmitter = GetComponent<FMOD_StudioEventEmitter> ();
//		footstepEmitter.StartEvent ();
//		footstepEmitter.evt.setVolume (0);

//		footstepEmitter.evt.getParameter ("Snabbet", out footstepParam);
//		footstepParam.setValue (2.0f);

	}
	
	// Update is called once per frame
	void Update () {
//		float totalSpeed = (Mathf.Abs (right) + Mathf.Abs (left)) / 2;
//		footstepEmitter.evt.setVolume (totalSpeed);
	}
}
