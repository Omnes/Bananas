using UnityEngine;
using System.Collections;

//TODO: Ändra forward/back så att det passar modellen, Annars blåser det åt fel håll!
public class scr_leafBlower : MonoBehaviour {
	public float m_blowPower = 0.0f;
	public float m_forwardPower = 1.0f;
	public float m_centripetalPower = 1.0f;
//	public float m_powerVariation = 0.0f;
//	public bool m_blowStraight

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

			scr_leafPhysics leafPhysics = leaf.GetComponent<scr_leafPhysics>();

			m_blowPower = m_touchInput.getCurrentBlowingPower();

			//Centreipetal power
			Vector3 playerDirection = transform.parent.TransformDirection( -Vector3.forward );	
			Vector3 projection = transform.parent.position + Vector3.Project(leaf.transform.position - transform.parent.position, playerDirection);

			Vector3 projectionDirection = (projection - leaf.transform.position).normalized;
//			scr_leafPhysics.AddForce(projectionDirection * m_centripetalPower * m_blowPower);
			leaf.rigidbody.AddForce(projectionDirection * m_centripetalPower * m_blowPower);

			//Blow power
			Transform blow_point = transform.parent.FindChild("blow_point");
			if (blow_point != null) {
				Vector3 directionVector = (leaf.transform.position - blow_point.position).normalized;
//				scr_leafPhysics.AddForce(directionVector * m_forwardPower * m_blowPower);
				leaf.rigidbody.AddForce(directionVector * m_forwardPower * m_blowPower);
			}

			//leaf.transform.position += transform.parent.transform.TransformDirection( Vector3.back ) * (power + Random.Range(0, powerVariation)) * Time.deltaTime;
//			Vector3 targetPosition = Vector3.MoveTowards(leaf.transform.position, transform.position, power);
//			targetPosition = new Vector3(targetPosition.x, leaf.transform.position.y, targetPosition.z);
			//leaf.transform.position = targetPosition;
//			leaf.GetComponent<scr_leafPhysics>().velocity = targetPosition;
		}

	}

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
