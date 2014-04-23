using UnityEngine;
using System.Collections;

public class Windfield : MonoBehaviour {

	public float m_force = 1;

	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Leaf"){
			//Debug.Log ("Löv is in the house");
			col.rigidbody.AddForce(transform.forward*m_force);
		}

	}
}
