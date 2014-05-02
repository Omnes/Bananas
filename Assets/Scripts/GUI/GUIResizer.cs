using UnityEngine;
using System.Collections;

public class GUIResizer : MonoBehaviour {

	public Vector2 m_size = Vector2.one;

	// Use this for initialization
	void Start () {
		resize();
	}

	public void resize(){
		Vector2 v = GUIMath.InchToPercent(m_size);
		transform.localScale = new Vector3(v.x,v.y,1f);
	}
	
}
