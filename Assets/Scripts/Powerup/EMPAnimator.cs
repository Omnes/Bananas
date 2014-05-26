using UnityEngine;
using System.Collections;

public class EMPAnimator : MonoBehaviour {
	//Design parameters
	public const float DURATION = 1.0f;
	public const float START_SCALE = 0;
	public const float TARGET_SCALE = 3.5f;

	//Variables
	private float delta;
	private Vector3 targetScale;

	/**
	 * Initialize variables
	 */
	void Start () {
		transform.localScale = Vector3.one * START_SCALE;
		targetScale = Vector3.one * TARGET_SCALE;
		delta = 0;
	}

	/**
	 * Increase the scale from START_SCALE to TARGET_SCALE over DURATION seconds
	 * Automatically destroys the object after reaching its TARGET_SCALE
	 */
	void Update () {
		transform.localScale = Vector3.Lerp(Vector3.one * START_SCALE, targetScale, delta);
		delta += Time.deltaTime / DURATION;

		if (delta > 1.0f) {
			Destroy(gameObject);
		}
	}
}
