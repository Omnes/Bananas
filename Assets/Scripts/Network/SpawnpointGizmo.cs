using UnityEngine;
using System.Collections;

public class SpawnpointGizmo : MonoBehaviour {

	public float m_gizmoSize = 1f;

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position,m_gizmoSize);
		Gizmos.DrawLine(transform.position,transform.position+transform.forward*m_gizmoSize);
	}
}
