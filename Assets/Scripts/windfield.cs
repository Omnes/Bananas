using UnityEngine;
using System.Collections;

public class windfield : MonoBehaviour {

	public float m_force = 1;

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Leaf"){
			Debug.Log ("Löv is in the house");
			col.rigidbody.AddForce(transform.TransformDirection(new Vector3(0,0,1)*m_force),ForceMode.VelocityChange);
		}

	}
}
