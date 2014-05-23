using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {

	private Transform m_target;
	private Transform m_transform;
	public Vector3 m_offset = new Vector3(0.37f,-0.215f,0.5f);

	public void setTarget(Transform parentOfTarget){
		m_transform = transform;
		m_target = parentOfTarget.Find("Full_body_ctrl/hip_ctrl_group/Hip_ctrl/pelvis_joint/spine_1_joint/spine_2_joint/spine_3_joint/spine_4_joint/R_collar_joint/R_shoulder_joint/R_elbow_joint/R_wrist_ctrl_group/R_wrist_ctrl/LeafBlower");
	}
	
	// Update is called once per frame
	void Update () {
		m_transform.position = m_target.TransformPoint(m_target.localPosition + m_offset); 
		m_transform.rotation = m_target.rotation;
	}
}
