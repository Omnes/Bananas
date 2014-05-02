using UnityEngine;
using System.Collections;

public class RemoteMovement : MonoBehaviour {

	public Vector3 m_rotationDelta = Vector3.zero;

//	private bool initiated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(m_rotationDelta*Time.deltaTime);
	}

	public void setSyncRotationSpeed(float rotSpeed){
		m_rotationDelta = new Vector3(0,rotSpeed,0);
	}
}
