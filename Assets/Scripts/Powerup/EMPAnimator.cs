using UnityEngine;
using System.Collections;

public class EMPAnimator : MonoBehaviour {
	public const float MAX_SIZE = 100;
	public const float DURATION = 2.5f;
	private Vector3 targetSize;
	private float f;

	void Start () {
		transform.localScale = Vector3.zero;
		targetSize = new Vector3 (MAX_SIZE, MAX_SIZE, MAX_SIZE);
		f = 0;
	}

	void FixedUpdate () {
		transform.localScale = Vector3.Lerp(Vector3.zero, targetSize, f);
		f += Time.fixedDeltaTime / DURATION;

		if (f > 1.0f) {
			Destroy(gameObject);
		}
	}
}
