using UnityEngine;
using System.Collections;

public class CtrlScript : MonoBehaviour 
{
	//src_touchInput touchIn = null;
	public float speed = 1;
	public Vector2 testVec;
	public float rotation = 1;
	Vector3 rDirVec = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 lDirVec = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 finalDirVec = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () 
	{
		//testVec = new Vector2 (1.0f, 1.0f);
//		touchIn = GetComponent<src_touchInput> (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		rDirVec.z = testVec.y;
		lDirVec.z = testVec.x;
		finalDirVec = rDirVec + lDirVec;

		//Adding rotation..
		transform.Rotate (finalDirVec.y);
		//transform.rotation = Quaternion.FromToRotation(,finalDirVec ;

		rigidbody.AddForce (finalDirVec * speed, ForceMode.VelocityChange);
	}
}
