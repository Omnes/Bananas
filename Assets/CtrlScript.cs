using UnityEngine;
using System.Collections;

public class CtrlScript : MonoBehaviour 
{
	src_touchInput touchIn = null;
	public float speed;

	public Vector2 testVec = new Vector2(1.0f, 0.5f);
	Vector3 targetVec = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 finalDirVec = new Vector3(0, 0, 0);
	Vector3 tmpVec;
	float right;
	float left;
	float diff;
	float angleToNew;
	float angleFactor;

	// Use this for initialization
	void Start () 
	{
		//testVec = new Vector2 (1.0f, 1.0f);
		touchIn = GetComponent<src_touchInput> (); 
	}
	
	// Update is called once per frame
	void Update () 
	{

		right = testVec.y;
		left = testVec.x;


		float tmpSpeed =  (right + left)/2;
		diff = left - right;
		angleFactor = diff / 0.1f; //always comparing to 0.1(9 degrees)
		angleToNew = 9 * angleFactor; //getting the angle to the new direction through multiplying 9 with the factor calculated above

		Vector3 currDir = transform.rotation.eulerAngles;
		Vector3 temp = Vector3.down * left + Vector3.up * right;

		//Adding rotation..
//		finalDirVec = Vector3.RotateTowards (transform.forward, targetVec, angleToNew, 0.0f);
//		Debug.DrawRay (transform.position, finalDirVec, Color.red);
//		transform.Rotate = Quaternion.AngleAxis(angleToNew, transform.up); 

		transform.Rotate (temp);

		rigidbody.AddForce (transform.forward * speed * tmpSpeed, ForceMode.VelocityChange);
	}
}
