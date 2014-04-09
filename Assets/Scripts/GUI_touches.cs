using UnityEngine;
using System.Collections;

public class GUI_touches : MonoBehaviour {

	private Transform[] m_plupps =  new Transform[5];
	private Renderer[] m_renderers = new Renderer[5];
	private int foundTouches = 0;

	// Use this for initialization
	void Start () {
		foundTouches = transform.childCount;
		for(int i = 0; i < foundTouches; i++){
			m_plupps[i] = transform.GetChild(i);
			m_renderers[i] = m_plupps[i].renderer;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//migth be unefficient
		for(int i = 0; i < foundTouches; i++){
			m_renderers[i].enabled = false;
		}

		Touch[] touches = Input.touches;
		for(int i = 0; i < touches.Length; i++){
			m_renderers[i].enabled = true;
			Vector2 pos = GUIMath.pixelsToPercent(touches[i].position);
			m_plupps[i].localPosition = new Vector3(pos.x,pos.y,m_plupps[i].localPosition.z);
		}
	
	}
}
