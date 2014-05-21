using UnityEngine;
using System.Collections;

public class ExplosionUVAnimator : MonoBehaviour {
	public float scale = 5;

	public const float DURATION = 0.48f;
	private const int NR_FRAMES = 6;
	private const int FRAMES_IN_WIDTH = 2;

	private float m_changeFrame = DURATION / NR_FRAMES;
	private int m_currentFrame = 0;
	private float m_time = 0;

	private MeshFilter m_meshFilter;
	private Rect m_map = new Rect(0.574f, 0.002f, 0.21f, 0.21f);

	void Start () {
		m_meshFilter = GetComponent<MeshFilter>();

		transform.localScale = Vector3.one * scale;
		transform.LookAt (transform.position + Camera.main.transform.forward);
		transform.position += -Camera.main.transform.forward;
		UpdateFrame ();
	}

	void Update () {
		m_time += Time.deltaTime;
		if (m_time > m_changeFrame) {
			m_currentFrame++;
			UpdateFrame();
			m_time = 0;
		}
	}

	private void UpdateFrame() {
		//hämta meshen
		Mesh mesh = m_meshFilter.mesh;
		Vector2[] uvs = new Vector2[4];

		//förkortningar
		float l = m_map.x + m_map.width * (m_currentFrame % FRAMES_IN_WIDTH);
		float r = l + m_map.width;
		float u = 1f - (m_map.y + m_map.height * (int)(m_currentFrame / FRAMES_IN_WIDTH));
		float d = u - m_map.height;
		
		//skapar nya uv kordinater
		uvs[0] = new Vector2(l, u); 
		uvs[1] = new Vector2(r, d);
		uvs[2] = new Vector2(r, u); 
		uvs[3] = new Vector2(l, d);

		//assignar dem till meshen
		mesh.uv = uvs;
		m_meshFilter.mesh = mesh;
	}
}
