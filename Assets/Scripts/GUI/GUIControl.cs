using UnityEngine;
using System.Collections;

public class GUIControl : MonoBehaviour {

	private Camera m_camera;
	public TouchInput m_input;
	public static int DEFAULT_DPI = 96;

	// Use this for initialization
	void Start () {
		m_camera = transform.parent.camera;
		placeQuad();
		BroadcastMessage("init",m_input);
	}

	void placeQuad(){
		transform.localPosition = calcPosition();
		transform.localScale = calcScale();
	}

	Vector3 calcPosition(){
		return new Vector3(0,0,m_camera.farClipPlane);
	}

	Vector3 calcScale(){
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float size = m_camera.orthographicSize * 2;
		Vector3 scale = new Vector3(size * aspectRatio,size,1);
		return scale;
	}

	public static float GetDPI()
	{
		return Screen.dpi == 0 ? DEFAULT_DPI : Screen.dpi;
	}
}
