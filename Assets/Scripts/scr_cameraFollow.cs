using UnityEngine;
using System.Collections;

public class scr_cameraFollow : MonoBehaviour {
	public Transform m_target;
	public float m_distance = 5f;
	public float m_xdistance= 10f;
	
	void Update () {
		transform.position = new Vector3 (m_target.position.x, m_target.position.y + m_distance, m_target.position.z-m_xdistance);
	}
}
