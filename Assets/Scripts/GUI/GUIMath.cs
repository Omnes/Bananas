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

	public static float InchToPixels(float f){
		return f * GUIControl.GetDPI();
	}

	public static Vector2 PixelsToInch(Vector2 v){
		float w = v.x / GUIControl.GetDPI();
		float h = v.y / GUIControl.GetDPI();
		return new Vector2(w,h);
	}

	public static Vector2 InchToPercent(Vector2 v){
		return pixelsToPercent(InchToPixels(v));
	}

	public static Vector2 SmallestOfInchAndPercent(Vector2 inch,Vector2 percent){
		int x = (int)Mathf.Min( GUIMath.InchToPixels(inch.x), Screen.width * percent.x); 
		int y = (int)Mathf.Min( GUIMath.InchToPixels(inch.y), Screen.height * percent.y); 
		return new Vector2(x,y);
	}
	/* <summary>
	 * Convert topleft aligned pixel rects to texture cords unity can use in for example GUI.DrawTextureWithTexCords
	 * </summary>
	 */ 

	public static Rect CalcTexCordsFromPixelRect(Rect area,float TextureSize = 1024f){
		return new Rect(area.x/TextureSize,1f-(area.y+area.height)/TextureSize,area.width/TextureSize,area.height/TextureSize);
	}
//	public static CalcTexCordsFromPixelRect(Rect area,float TextureSizeX,float TextureSizeY){
//		
//	}

}
