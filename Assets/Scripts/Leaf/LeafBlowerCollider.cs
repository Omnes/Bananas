using UnityEngine;
using System.Collections;

public class LeafBlowerCollider : MonoBehaviour {


	private LeafBlower parentBlower;
	// Use this for initialization
	void Start () {
		parentBlower = transform.parent.GetComponent<LeafBlower>();
	}
	
	void OnTriggerEnter(Collider col){
		parentBlower.OnTriggerEnterInChild(col);
	}
}
