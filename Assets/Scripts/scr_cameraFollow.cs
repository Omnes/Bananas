﻿using UnityEngine;
using System.Collections;

public class scr_cameraFollow : MonoBehaviour {
	public Transform m_target;
	public float m_distance = 5f;
<<<<<<< HEAD
	public float m_xdistance = -2f;
	public float m_zdistance =3f;
=======
	public float m_zdistance= 10f;
	public float m_xdistance = 2f;
>>>>>>> origin/viktor
	
	void Update () {
		transform.position = new Vector3 (m_target.position.x-m_xdistance, m_target.position.y + m_distance, m_target.position.z-m_zdistance);
	}
}
 
