using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform m_target;
	public Vector3 m_offset = new Vector3();

	private Vector3 m_internalOffset = new Vector3();

	void Start(){
		transform.parent = null;
	}

	void Update () {
		if(m_target != null){
			transform.position = m_target.position + m_internalOffset;
			m_internalOffset = m_offset;
			m_internalOffset.x = transform.forward.x * m_offset.x;
			m_internalOffset.z = transform.forward.z * m_offset.z;
		}

	}

	public void SetTarget(Transform target){
		m_target = target;

		Vector3 rot = transform.localEulerAngles;
		rot.y = target.localEulerAngles.y;
		transform.rotation = Quaternion.Euler(rot);
	}
	
}
