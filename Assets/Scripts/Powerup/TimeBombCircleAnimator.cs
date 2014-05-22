using UnityEngine;
using System.Collections;

public class TimeBombCircleAnimator : MonoBehaviour {
	public float rotationSpeed = -360;
	private float angle = 0;

	void LateUpdate () {
		transform.rotation = Quaternion.Euler(90, 0, angle);
		angle += Time.deltaTime * rotationSpeed;
	}

}
