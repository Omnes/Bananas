using UnityEngine;
using System.Collections;

public class InitGUICamera : MonoBehaviour {

	public GameObject m_GUICameraPrefab; 

	// Use this for initialization
	void Start () {
		GameObject guiCam = Instantiate(m_GUICameraPrefab,Vector3.up*50f,Quaternion.identity) as GameObject;
		//this have to execute after InitLocalINput
		TouchInput input = GetComponent<InitLocalInput>().getLocalInput().GetComponent<TouchInput>();
		guiCam.GetComponentInChildren<GUIControl>().initiateGUI(input);
	}

}
