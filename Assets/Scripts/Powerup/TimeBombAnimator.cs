using UnityEngine;
using System.Collections;

public class TimeBombAnimator : MonoBehaviour {
	public Vector3 defaultAngle = Vector3.zero;
	public Vector3 rotationSpeed = Vector3.zero;
	private Vector3 angle = Vector3.zero;

	void LateUpdate () {
		transform.rotation = Quaternion.Euler(defaultAngle + angle);
		angle += Time.deltaTime * rotationSpeed;
	}

}
