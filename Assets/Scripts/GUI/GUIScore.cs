using UnityEngine;
using System.Collections;

public class GUIScore : MonoBehaviour {

	private TextMesh m_text;

	// Use this for initialization
	void Start () {
		m_text = GetComponent<TextMesh>();

		transform.localPosition = new Vector3(-0.5f,0.5f,transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		m_text.text = ScoreKeeper.GetScoreString();
	}
}
