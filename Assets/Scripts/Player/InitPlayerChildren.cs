﻿using UnityEngine;
using System.Collections;

public class InitPlayerChildren : MonoBehaviour {

	public Transform m_whirlwindPrefab;
	public Transform m_airTriggerPrefab;

	// Use this for initialization
	public void Init () {
	 	Transform whirlwind = Instantiate(m_whirlwindPrefab) as Transform;
		whirlwind.GetComponent<Whirlwind>().m_target = transform;
		whirlwind.localPosition = whirlwind.position;
		whirlwind.localRotation = Quaternion.identity;
		whirlwind.name = m_whirlwindPrefab.name;
		Transform airTrigger = Instantiate(m_airTriggerPrefab) as Transform;
		airTrigger.parent = transform;
		airTrigger.localRotation = Quaternion.identity;
		airTrigger.localPosition = Vector3.zero;
		airTrigger.name = m_airTriggerPrefab.name;

		airTrigger.GetComponent<LeafBlower>().setWhirlwind(whirlwind);

	}

}
