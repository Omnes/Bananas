using UnityEngine;
using System.Collections;

public class scr_leafBlowerCollider : MonoBehaviour {


	private scr_leafBlower parentBlower;
	// Use this for initialization
	void Start () {
		parentBlower = transform.parent.GetComponent<scr_leafBlower>();
	}
	
	void OnTriggerStay(Collider col){
		parentBlower.OnTriggerStayInChild(col);
	}
}
