using UnityEngine;
using System.Collections;

//this script should be on The GUI prefab

[ExecuteInEditMode]
public class scr_coverCameraQuad : MonoBehaviour {

	private Camera m_cam;
	private Transform m_quad;
	public bool m_updateInEditor = true;
	public float m_cameraDistance = 0.1f;
	// Use this for initialization
	void Start () {
		m_cam = Camera.main;
		m_quad = transform;
		placeQuad();
	}

	void placeQuad(){
		float pos = m_cam.nearClipPlane + m_cameraDistance;
		m_quad.position = m_cam.transform.position + m_cam.transform.forward*pos;
		m_quad.LookAt(m_cam.transform.position + m_cam.transform.forward*(pos+1f));

		float h = Mathf.Tan(m_cam.fieldOfView*Mathf.Deg2Rad*0.5f)*pos*2f;
				
		m_quad.transform.localScale = new Vector3(h*m_cam.aspect,h,1f);

	}
	

	void Update(){
		if(m_updateInEditor && !Application.isPlaying){
			placeQuad();
		}
	}
}
