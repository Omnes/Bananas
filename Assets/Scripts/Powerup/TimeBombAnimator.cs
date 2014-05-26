using UnityEngine;
using System.Collections;

public class TimeBombAnimator : MonoBehaviour {
	public float rotationSpeed = 45;
	private float angle = 0;

//	public float scaleSpeed = 500;
//	public float scaleAmplitude = 0.25f;

	void LateUpdate () {
		transform.rotation = Quaternion.Euler(0, angle, 0);
		angle += Time.deltaTime * rotationSpeed;
//		transform.localScale = Vector3.one + Vector3.one * Mathf.Sin (Time.time * scaleSpeed) * scaleAmplitude;
	}

}
