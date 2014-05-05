using UnityEngine;
using System.Collections;

public class loadNextSceneGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		Vector2 size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));

		float centerX = Screen.width/2 - (size.x / 2);
		float centerY = Screen.height/6;

		if(GUI.Button(new Rect(centerX, centerY + size.y, size.x, size.y), "Start Game")){
			Application.LoadLevel("Sean_Scene");
		}
	}
}
