using UnityEngine;
using System.Collections;

public class GlideAgainstWall : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			GameObject player = other.gameObject;

			Vector3 curDir = player.transform.forward;
			Vector3 newDir = Vector3.Angle(curDir, transform.right) < Vector3.Angle(curDir, -transform.right) ? transform.right : -transform.right;
			player.transform.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);

//			Vector3 curDir = player.transform.forward;
//			Vector3 normal = transform.forward;
//			Vector3 newDir = Vector3.Reflect(curDir, normal);
//			Quaternion newAngle = Quaternion.FromToRotation(Vector3.forward, newDir);
//			player.transform.rotation = Quaternion.Lerp(player.transform.rotation, newAngle, 0.1f);
		}
	}
}
