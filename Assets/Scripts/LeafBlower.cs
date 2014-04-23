﻿using UnityEngine;
using System.Collections;
using FMOD.Studio;

//TODO: Gör om distanceToLeaf så att den använder vector2D
public class LeafBlower : MonoBehaviour {
	private float m_blowPower = 0.0f;	

	public float m_forwardPower = 1.0f;
	public float m_distanceMultiplier = 0.0f;

	public float m_centripetalPower = 1.0f;
	public float m_centDistMult = 0.0f;
	//INTE FINT FIXA PLS
	public float m_colliderLength = 4f;

	public float m_minVelocity = 0.0f;
	public bool m_minVelocityDependsOnBlowPower = true;

	private Transform playerTransform;

	private InputHub m_touchInput;
	private FMOD.Studio.EventInstance m_blowSound;

	public ParticleSystem m_particleSystem;
	private bool m_particleEmit = false;

	void Start()
	{
		m_touchInput = transform.parent.GetComponent<InputHub>();

//		FMOD.Studio.EventInstance s = SoundManager.Instance.play( "event:/gameplay_concept" );
//		s.setTimelinePosition (60000);

		m_blowSound = SoundManager.Instance.play( "event:/leafblower (ytterst kass)" );
		playerTransform = transform;
	}

	void Update()
	{
		m_blowPower = m_touchInput.getCurrentBlowingPower();
		m_blowSound.setVolume (m_blowPower / 3);
//		m_blowSound.setVolume (0);

		if(m_blowPower > 0){
			if(!m_particleEmit){
				m_particleEmit = true;
				m_particleSystem.Play();
			}
		}else if(m_particleEmit){
			m_particleEmit = false;
			m_particleSystem.Stop();
		}
	}

	public void OnTriggerStayInChild(Collider col)
	{
		if (col.gameObject.CompareTag("Leaf")) {
			GameObject leaf = col.gameObject;

			Transform leafTransform = leaf.transform;

			//Centreipetal power
			Vector3 playerDirection = playerTransform.parent.TransformDirection( Vector3.forward );	
			Vector3 projectionPoint = playerTransform.parent.position + Vector3.Project(leafTransform.position - transform.parent.position, playerDirection);

			Vector3 projectionDirection = projectionPoint - leafTransform.position;
			float distance = projectionDirection.magnitude;
			projectionDirection.Normalize();
			Vector3 centripetalForce = projectionDirection * m_centripetalPower * m_blowPower * (1 + distance * m_centDistMult);
//			leaf.rigidbody.AddForce(projectionDirection * m_centripetalPower * m_blowPower * (1 + distance * m_centDistMult));

			//Blow power
			Vector3 directionVector = (leafTransform.position - playerTransform.parent.position).normalized;
//			float distanceToLeaf = Vector3.Distance( playerTransform.position, leafTransform.position );
//			float distanceToLeaf = Vector3.Distance( new Vector3(playerTransform.position.x, 0, playerTransform.position.z), new Vector3(leafTransform.position.x, 0, leafTransform.position.z) );
			float distanceToLeaf = Vector2.Distance( new Vector2(playerTransform.position.x, playerTransform.position.z), new Vector2(leafTransform.position.x, leafTransform.position.z) );
			Vector3 forwardForce = directionVector * m_forwardPower * m_blowPower * (1 + (m_colliderLength - distanceToLeaf) * m_distanceMultiplier);
//			leaf.rigidbody.AddForce(directionVector * m_forwardPower * m_blowPower * (1 + (m_colliderLength - distanceToLeaf) * m_distanceMultiplier));

			leaf.rigidbody.AddForce((centripetalForce + forwardForce) * Time.deltaTime);

//			if(leaf.rigidbody.velocity.magnitude > m_maxVelocity){
//				Vector3 newSpeed = leaf.rigidbody.velocity.normalized * m_maxVelocity;
//				rigidbody.AddForce(newSpeed - leaf.rigidbody.velocity,ForceMode.VelocityChange);
//			}

			Rigidbody leafRigidbody = leaf.rigidbody;
			//Minimum velocity
			if ( leafRigidbody.velocity.magnitude < m_minVelocity ) {
				if ( m_minVelocityDependsOnBlowPower ) {
					leafRigidbody.velocity = leafRigidbody.velocity.normalized * m_minVelocity * m_blowPower;
				}
				else {
					leafRigidbody.velocity = leafRigidbody.velocity.normalized * m_minVelocity;
				}
			}

		}

	}

}