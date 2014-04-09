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
		//kanske är oeffektivt
		//stänger av alla renders
		for(int i = 0; i < foundTouches; i++){
			m_renderers[i].enabled = false;
		}
		//placerar alla pluppar på rätt ställen och enablar de som behövs
		Touch[] touches = Input.touches;
		for(int i = 0; i < Mathf.Min(touches.Length,foundTouches); i++){
			m_renderers[i].enabled = true;
			Vector2 pos = GUIMath.pixelsToPercent(touches[i].position);
			m_plupps[i].localPosition = new Vector3(pos.x,pos.y,m_plupps[i].localPosition.z);
		}
		//debug för editorn
		if(Application.isEditor && Input.GetMouseButton(0)){
			m_renderers[0].enabled = true;
			Vector2 screen_center = new Vector2(Screen.width/2,Screen.height/2);
			Vector2 pos = GUIMath.pixelsToPercent(asVec2(Input.mousePosition) - screen_center);
			m_plupps[0].localPosition = new Vector3(pos.x,pos.y,m_plupps[0].localPosition.z);
		}

	
	}

	Vector2 asVec2(Vector3 v){
		return new Vector2(v.x,v.y);
	}
}
