using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIStick : MonoBehaviour {

	public TouchInput m_touchInput;
	public Side m_side = Side.Left;
	public bool m_updateInEditor = false;

	public enum Side {Left,Right};

	void updateSize(){
		//hämtar in storleken de ska ha från scr_touchinput så man bara behöver ändra på ett ställe
		Vector2 size = GUIMath.InchToPercent(m_touchInput.getGUIStickSize());
		Debug.Log (m_touchInput.getGUIStickSize());
		//skalar om
		transform.localScale = new Vector3(size.x,size.y,transform.localScale.z);
		//placerar dem
		if(m_side == Side.Left){
			transform.localPosition = new Vector3(-(0.5f-size.x/2),transform.localPosition.y,transform.localPosition.z); //the y is assuming the stick always is in the center (might need change)
		}else{
			transform.localPosition = new Vector3((0.5f-size.x/2),transform.localPosition.y,transform.localPosition.z); //the y is assuming the stick always is in the center (might need change)
		}
	}

	public void init(TouchInput input){
		m_touchInput = input;
		updateSize();
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.isEditor && m_touchInput != null && m_updateInEditor){
			updateSize();
		}
	}
}
