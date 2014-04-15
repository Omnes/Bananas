using UnityEngine;
using System.Collections;

public class scr_cameraFollow : MonoBehaviour {
	public Transform m_target;
	public Vector3 m_offset = new Vector3();

	void Update () {
		transform.position = m_target.position + m_offset;

	}
}
