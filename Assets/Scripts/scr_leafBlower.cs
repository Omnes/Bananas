using UnityEngine;
using System.Collections;
using FMOD.Studio;

//TODO: Gör om distanceToLeaf så att den använder vector2D
public class scr_leafBlower : MonoBehaviour {
	private float m_blowPower = 0.0f;	

	public float m_forwardPower = 1.0f;
	public float m_distanceMultiplier = 0.0f;

	public float m_centripetalPower = 1.0f;
	public float m_centDistMult = 0.0f;
	//INTE FINT FIXA PLS
	public float m_colliderLength = 4f;
	
//	public float m_torquePower = 0.0f;
//	public float m_powerVariation = 0.0f;
//	public bool m_blowStraight

	public float m_minVelocity = 0.0f;
	public bool m_minVelocityDependsOnBlowPower = true;

	private scr_touchInput m_touchInput;
	private FMOD.Studio.EventInstance m_blowSound;

//	void OnTriggerEnter(Collider col)
//	{
//		Debug.Log("Enter: " + col.gameObject);
//	}

	void Start()
	{
		m_touchInput = transform.parent.GetComponent<scr_touchInput>();

//		FMOD.Studio.EventInstance s = scr_soundManager.Instance.play( "event:/gameplay_concept" );
//		s.setTimelinePosition (60000);

		m_blowSound = scr_soundManager.Instance.play( "event:/leafblower (ytterst kass)" );
	}

	void Update()
	{
		m_blowPower = m_touchInput.getCurrentBlowingPower();
		m_blowSound.setVolume (m_blowPower);

//		Debug.Log ("Power: " + m_blowPower);
//		if (Input.GetKeyDown (KeyCode.Q)) {
//			m_blowSound = scr_soundManager.Instance.playOneShot( "event:/leafblower (ytterst kass)" );
//			m_blowPower
//			FMOD_StudioSystem.instance.PlayOneShot ("event:/leafblower (ytterst kass)", transform.position);
//		}
	}
	
	public void OnTriggerStayInChild(Collider col)
	{
		if (col.gameObject.CompareTag("Leaf")) {
			GameObject leaf = col.gameObject;

//			m_blowPower = m_touchInput.getCurrentBlowingPower();

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
			leaf.rigidbody.AddForce(directionVector * m_forwardPower * m_blowPower * (1 + (m_colliderLength - distanceToLeaf) * m_distanceMultiplier));

			//Minimum velocity
			if ( leaf.rigidbody.velocity.magnitude < m_minVelocity ) {
				if ( m_minVelocityDependsOnBlowPower ) {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity * m_blowPower;
				}
				else {
					leaf.rigidbody.velocity = leaf.rigidbody.velocity.normalized * m_minVelocity;
				}
			}

		}

	}

}
