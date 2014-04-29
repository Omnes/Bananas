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

	public static Vector2 InchToPixels(Vector2 v){
		float w = v.x * GUIControl.GetDPI();
		float h = v.y * GUIControl.GetDPI();
		return new Vector2(w,h);
	}

	public static Vector2 PixelsToInch(Vector2 v){
		float w = v.x / GUIControl.GetDPI();
		float h = v.y / GUIControl.GetDPI();
		return new Vector2(w,h);
	}

	public static Vector2 InchToPercent(Vector2 v){
		return pixelsToPercent(InchToPixels(v));
	}
	

}
