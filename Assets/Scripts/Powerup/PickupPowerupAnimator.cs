using UnityEngine;
using System.Collections;

public class PickupPowerupAnimator : MonoBehaviour {
	private const float DURATION = 2.5f;
	private float m_fadeSpeed = 0.185f;
	private float m_growSpeed = 0.125f;
	private float delta;

	private float m_curAlpha = 1.0f;

	private Material m_material;

	void Awake () {
		delta = 0;
		transform.Rotate (new Vector3 (-90, 0, 0));
		m_material = transform.GetChild (0).renderer.material;
	}

	void Update () {
		delta += Time.deltaTime;
		transform.localScale += Vector3.one * Time.deltaTime / m_growSpeed;

		m_curAlpha -= Time.deltaTime / m_fadeSpeed;
		m_material.SetColor ("_Color", new Color(1, 1, 1, m_curAlpha));
		
		if (delta > DURATION) {
			Destroy(gameObject);
		}
	}
}
