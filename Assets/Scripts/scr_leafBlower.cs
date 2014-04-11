using UnityEngine;
using System.Collections;
using FMOD.Studio;

//TODO: Fixa så att 
public class scr_leafBlower : MonoBehaviour {
	private float m_blowPower = 0.0f;	

	public float m_forwardPower = 1.0f;
	public float m_distanceMultiplier = 0.0f;

	public float m_centripetalPower = 1.0f;
	public float m_centDistMult = 0.0f;
	
//	public float m_torquePower = 0.0f;
//	public float m_powerVariation = 0.0f;
//	public bool m_blowStraight

	public float m_minVelocity = 0.0f;
	public bool m_minVelocityDependsOnBlowPower = true;

	private scr_touchInput m_touchInput;
	private AudioSource m_blowSound;

//	void OnTriggerEnter(Collider col)
//	{
//		Debug.Log("Enter: " + col.gameObject);
//	}

	void Start()
	{
		m_touchInput = transform.parent.GetComponent<scr_touchInput>();

//		scr_soundManager.Instance.playOneShot ("event:/music_concept");
//		FMOD.Studio.ParameterInstance progression;
//		sound.getParameter ("Progression", out progression);
//		progression.setValue (0.7f);
	}
	
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.CompareTag("Leaf")) {
			GameObject leaf = col.gameObject;

			m_blowPower = m_touchInput.getCurrentBlowingPower();

			//Centreipetal power
			Vector3 playerDirection = transform.parent.TransformDirection( Vector3.forward );	
			Vector3 projectionPoint = transform.parent.position + Vector3.Project(leaf.transform.position - transform.parent.position, playerDirection);

			Vector3 projectionDirection = projectionPoint - leaf.transform.position;
			float distance = projectionDirection.magnitude;
			projectionDirection.Normalize();
			leaf.rigidbody.AddForce(projectionDirection * m_centripetalPower * m_blowPower * (1 + distance * m_centDistMult));

			//Blow power
			Vector3 directionVector = (leaf.transform.position - transform.parent.position).normalized;
			float distanceToLeaf = Vector3.Distance( transform.position, leaf.transform.position );
//			distanceToLeaf = distanceToLeaf / 
			leaf.rigidbody.AddForce(directionVector * m_forwardPower * m_blowPower * (1 + distanceToLeaf * m_distanceMultiplier));

			//Minimum velocity
			if ( leaf.rigidbody.velocity.magnitude < m_minVelocity ) {
				if ( m_minVelocityDependsOnBlowPower ) {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity * m_blowPower;
				}
				else {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity;
				}
			}

//			Transform blow_point = transform.parent.FindChild("blow_point");
//			if (blow_point != null) {
//				Vector3 directionVector = (leaf.transform.position - blow_point.position).normalized;
				

//				Mathf.DeltaAngle( transform.
//				leaf.rigidbody.AddTorque(directionVector * m_torquePower * m_blowPower);	//TODO: Max torque

//				Vector3 forwardVector = transform.TransformDirection( -Vector3.forward ).normalized;
//				debug_angle = signedAngle(new Vector2(forwardVector.x, forwardVector.z), new Vector2(directionVector.x, directionVector.z));
				//debug_angle -= signedAngle(new Vector2(0,0), new Vector2(forwardVector.x, forwardVector.z));
//				debug_angle = Vector3.Angle( forwardVector, directionVector );
//				Debug.DrawLine( new Vector3(0,0,0), directionVector, Color.cyan );
//				Debug.DrawLine( new Vector3(0,0,0), forwardVector, Color.magenta );
//			}


//			leaf.rigidbody.AddTorque( leaf.rigidbody.velocity );

			//leaf.transform.position += transform.parent.transform.TransformDirection( Vector3.back ) * (power + Random.Range(0, powerVariation)) * Time.deltaTime;
//			Vector3 targetPosition = Vector3.MoveTowards(leaf.transform.position, transform.position, power);
//			targetPosition = new Vector3(targetPosition.x, leaf.transform.position.y, targetPosition.z);
			//leaf.transform.position = targetPosition;
//			leaf.GetComponent<scr_leafPhysics>().velocity = targetPosition;
		}

	}

}
