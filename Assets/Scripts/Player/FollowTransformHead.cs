using UnityEngine;
using System.Collections;

public class FollowTransformHead : MonoBehaviour {

	private Transform m_target;
	private Transform m_transform;
	private Transform m_startParent;
	private Transform m_parentTransform;
	public Vector3 m_offset = Vector3.zero;

//	public void setTarget(Transform parentOfTarget){
	public void Start(){
		m_transform = transform;
		m_parentTransform = gameObject.transform.parent;
		for (int i = 0; i < m_parentTransform.childCount; i++) {
			if (m_parentTransform.GetChild(i).name.Contains("animation")) {
				m_startParent = m_parentTransform.GetChild(i);
			}
		}
		m_target = m_startParent.Find("Full_body_ctrl/hip_ctrl_group/Hip_ctrl/pelvis_joint/spine_1_joint/spine_2_joint/spine_3_joint/spine_4_joint/neck_joint/head_joint");
	}
	
	// Update is called once per frame
	void Update () {
		m_transform.position = m_target.TransformPoint(m_target.localPosition) + m_offset; 
		m_transform.rotation = m_target.rotation;
	}
}
