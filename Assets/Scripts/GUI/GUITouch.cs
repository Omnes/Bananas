using UnityEngine;
using System.Collections;

public class GUITouch : MonoBehaviour {

	public static GUITouch s_lazyInstance;

	//dont forget to assign this in the editor
	public Transform[] m_quads;
	public float m_fadeTime = 0.5f;

	public enum Side{Left,Rigth}; 
	private float[] m_timers;

	// Use this for initialization
	void Start () {
		s_lazyInstance = this;
		m_timers = new float[2];

		float width = m_quads[0].localScale.x;
		float heigth = m_quads[0].localScale.y;
		m_quads[0].localPosition = new Vector3(-0.5f + width/2,-0.5f + heigth/2, m_quads[0].localPosition.z);
		m_quads[1].localPosition = new Vector3(0.5f - width/2,-0.5f + heigth/2, m_quads[1].localPosition.z);
		m_quads[0].renderer.material.color = new Color(1, 1, 1, 0);
		m_quads[1].renderer.material.color = new Color(1, 1, 1, 0);
	}

	public void Press(Side side){
		if (Winstate.m_gameRunning) {
			m_timers[(int)side] = m_fadeTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateSide((int)Side.Left);
		updateSide((int)Side.Rigth);
	}

	void updateSide(int side){
		m_timers[side] -= Time.deltaTime;
		if(m_timers[side] > 0f){
			m_quads[side].renderer.material.color = new Color(1, 1, 1, m_timers[side] / m_fadeTime);
		}
	}
}
