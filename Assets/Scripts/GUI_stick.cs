using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUI_stick : MonoBehaviour {

	public scr_touchInput m_touchInput;
	public Side m_side = Side.Left;
	public bool m_updateInEditor = false;

	public enum Side {Left,Right};

	// Use this for initialization
	void Start () {
		updateSize();
	}

	void updateSize(){
		//hämtar in storleken de ska ha från scr_touchinput så man bara behöver ändra på ett ställe
		Vector2 size = m_touchInput.getGUIStickSize();
		//skalar om
		transform.localScale = new Vector3(size.x,size.y,transform.localScale.z);
		//placerar dem
		if(m_side == Side.Left){
			transform.localPosition = new Vector3(-(0.5f-size.x/2),0f,0f); //the y is assuming the stick always is in the center (might need change)
		}else{
			transform.localPosition = new Vector3((0.5f-size.x/2),0f,0f); //the y is assuming the stick always is in the center (might need change)
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Application.isEditor && m_touchInput != null && m_updateInEditor){
			updateSize();
		}
	}
}
