using UnityEngine;
using System.Collections;

public class GUIMath{
	//hjälpklass för användbara funktioner till GUI

	public static Vector2 pixelsToPercent(Vector2 v){
		float w = v.x/Screen.width;
		float h = v.y/Screen.height;
		return new Vector2(w,h);
	}

	public static Vector2 PercentToPixels(Vector2 v){
		float w = Screen.width / v.x;
		float h = Screen.height / v.y;
		return new Vector2(w,h);
	}
}
