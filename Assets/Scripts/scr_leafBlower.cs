using UnityEngine;
using System.Collections;

//TODO: Ändra forward/back så att det passar modellen, Annars blåser det åt fel håll!
public class scr_leafBlower : MonoBehaviour {
	private float m_blowPower = 0.0f;	
	public float m_forwardPower = 1.0f;
	public float m_centripetalPower = 1.0f;
	public float m_centDistMult = 0.0f;
//	public float m_torquePower = 0.0f;
//	public float m_powerVariation = 0.0f;
//	public bool m_blowStraight

	public float m_minVelocity = 0.0f;
	public bool m_minVelocityDependsOnBlowPower = true;


//	private float debug_angle = 0.0f;

	private scr_touchInput m_touchInput;

//	void OnTriggerEnter(Collider col)
//	{
//		Debug.Log("Enter: " + col.gameObject);
//	}

	void Start()
	{
		m_touchInput = transform.parent.GetComponent<scr_touchInput>();
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.CompareTag("Leaf")) {
			GameObject leaf = col.gameObject;

//			scr_leafPhysics leafPhysics = leaf.GetComponent<scr_leafPhysics>();

			m_blowPower = m_touchInput.getCurrentBlowingPower();

			//Centreipetal power
			Vector3 playerDirection = transform.parent.TransformDirection( Vector3.forward );	
			Vector3 projectionPoint = transform.parent.position + Vector3.Project(leaf.transform.position - transform.parent.position, playerDirection);

			Vector3 projectionDirection = projectionPoint - leaf.transform.position;
			float distance = projectionDirection.magnitude;
			projectionDirection.Normalize();
//			scr_leafPhysics.AddForce(projectionDirection * m_centripetalPower * m_blowPower);
			leaf.rigidbody.AddForce(projectionDirection * m_centripetalPower * m_blowPower * (1 + distance * m_centDistMult));

			//Blow power
//			Transform blow_point = transform.parent.FindChild("blow_point");
//			if (blow_point != null) {
//				Vector3 directionVector = (leaf.transform.position - blow_point.position).normalized;
				Vector3 directionVector = (leaf.transform.position - transform.parent.position).normalized;
//				scr_leafPhysics.AddForce(directionVector * m_forwardPower * m_blowPower);
				leaf.rigidbody.AddForce(directionVector * m_forwardPower * m_blowPower);

//				Mathf.DeltaAngle( transform.
//				leaf.rigidbody.AddTorque(directionVector * m_torquePower * m_blowPower);	//TODO: Max torque

//				Vector3 forwardVector = transform.TransformDirection( -Vector3.forward ).normalized;
//				debug_angle = signedAngle(new Vector2(forwardVector.x, forwardVector.z), new Vector2(directionVector.x, directionVector.z));
				//debug_angle -= signedAngle(new Vector2(0,0), new Vector2(forwardVector.x, forwardVector.z));
//				debug_angle = Vector3.Angle( forwardVector, directionVector );
//				Debug.DrawLine( new Vector3(0,0,0), directionVector, Color.cyan );
//				Debug.DrawLine( new Vector3(0,0,0), forwardVector, Color.magenta );
//			}

			if ( leaf.rigidbody.velocity.magnitude < m_minVelocity ) {
				if ( m_minVelocityDependsOnBlowPower ) {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity * m_blowPower;
				}
				else {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity;
				}
			}
//			leaf.rigidbody.AddTorque( leaf.rigidbody.velocity );

			//leaf.transform.position += transform.parent.transform.TransformDirection( Vector3.back ) * (power + Random.Range(0, powerVariation)) * Time.deltaTime;
//			Vector3 targetPosition = Vector3.MoveTowards(leaf.transform.position, transform.position, power);
//			targetPosition = new Vector3(targetPosition.x, leaf.transform.position.y, targetPosition.z);
			//leaf.transform.position = targetPosition;
//			leaf.GetComponent<scr_leafPhysics>().velocity = targetPosition;
		}

	}

	float signedAngle(Vector2 v0, Vector2 v1) {
//		return Mathf.Atan2 (v0.y - v1.y, v0.x - v1.y) * 180 / Mathf.PI;
//		return Mathf.Acos (Vector2.Dot(v0, v1)) * 180 / Mathf.PI;
		return Vector2.Angle (v0, v1);

	}

//	void OnGUI(){
//		GUI.Label(new Rect(10, 25, 200, 25), "Angle: " + debug_angle.ToString("F2"));
//	}

//	void OnTriggerExit(Collider col)
//	{
//		Debug.Log("Exit: " + col.gameObject);
//	}

//	void OnCollisionEnter(Collision col)
//	{
//		Debug.Log("Enter: " + col.gameObject);
//	}
//
//	void OnCollisionStay(Collision col)
//	{
//		Debug.Log("Stay: " + col.gameObject);
//		foreach (ContactPoint contact in col.contacts){
//			Debug.DrawRay(contact.point, contact.normal, Color.white);
//		}
//	}
//
//	void OnCollisionExit(Collision col)
//	{
//		Debug.Log("Exit: " + col.gameObject);
//	}

}
