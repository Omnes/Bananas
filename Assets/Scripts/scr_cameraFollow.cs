using UnityEngine;
using System.Collections;

public class scr_cameraFollow : MonoBehaviour {
	public Transform m_target;
	public float m_distance = 5f;
	public Vector3 m_offset = new Vector3();
	
	void Update () {
		transform.position = new Vector3 (m_target.position.x, m_target.position.y + m_distance, m_target.position.z) + m_offset;
	}
}
