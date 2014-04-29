using UnityEngine;
using System.Collections;

public class SpawnpointGizmo : MonoBehaviour {

	//this might not be that relevant for this script (its for sorting the spawnpoints)
	public int m_id = 0;

	public float m_gizmoSize = 1f;

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position,m_gizmoSize);
		Gizmos.DrawLine(transform.position,transform.position+transform.forward*m_gizmoSize);
	}
}
