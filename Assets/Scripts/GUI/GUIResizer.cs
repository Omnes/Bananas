using UnityEngine;
using System.Collections;

public class GUIResizer : MonoBehaviour {

	public Vector2 m_size = Vector2.one;
	public Vector2 m_maxPercentOfScreen = Vector2.one;

	// Use this for initialization
	void Awake () {
		resize();
	}

	public void resize(){
		Vector2 smallest = GUIMath.SmallestOfInchAndPercent(m_size,m_maxPercentOfScreen);
		Vector2 v = GUIMath.pixelsToPercent(smallest);
		transform.localScale = new Vector3(v.x,v.y,1f);
	}
	
}
