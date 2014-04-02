using UnityEngine;
using System.Collections;

public class scr_leafBlower : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Debug.Log ("Initializing Leaf blower!");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionStay(Collision col)
	{
//		Debug.Log ("Test");
		foreach (ContactPoint contact in col.contacts){
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
	}

}
