using UnityEngine;
using System.Collections;

//this script should be on The GUI prefab

[ExecuteInEditMode]
public class CoverCameraQuad : MonoBehaviour {

	public Camera m_GUICamera;
	private Transform m_quad;
//	public bool perspective = true;
//	public bool m_updateInEditor = false;
//	public float m_cameraDistance = 0.1f;
	
	// Use this for initialization
	void Start () {
		m_quad = transform;
		placeQuad();
	}

	//placerar en quad framför kameran som täcker hela viewn
	void placeQuad(){
//			float pos = m_GUICamera.nearClipPlane + m_cameraDistance;
//			m_quad.position = m_GUICamera.transform.position + m_GUICamera.transform.forward*pos;
//			m_quad.LookAt(m_GUICamera.transform.position + m_GUICamera.transform.forward*(pos+1f));
//
//			float h = Mathf.Tan(m_GUICamera.fieldOfView*Mathf.Deg2Rad*0.5f)*pos*2f;
//					
//			m_quad.transform.localScale = new Vector3(h*m_GUICamera.aspect,h,1f);

		float size = m_GUICamera.orthographicSize;
		float aspectRation  = m_GUICamera.aspect;
		m_quad.transform.localScale = new Vector3(size*2 * aspectRation ,size*2,1);

	}
	

}
