using UnityEngine;
using System.Collections;

public class SyncData{
	public Vector3 m_velocity;
	public Vector3 m_position;
	public Quaternion m_rotation;
	public float m_rotationSpeed;
	public float m_blowing;

	public SyncData(Vector3 position,Vector3 velocity,Quaternion rotation,float rotationSpeed,float blowing = 0f){
		m_position = position;
		m_velocity = velocity;
		m_rotation = rotation;
		m_rotationSpeed = rotationSpeed;
		m_blowing = blowing;
	}

	public SyncData(){
		m_velocity = Vector3.zero;
		m_position = Vector3.zero;
		m_rotation = Quaternion.identity;
		m_rotationSpeed = 0f;
		m_blowing = 0f;
	}
};


public class SyncMovement : MonoBehaviour {

	public static SyncMovement[] s_syncMovements = new SyncMovement[4];
	
	private Vector3 m_ghostPosition = new Vector3();
	private Rigidbody m_rigidbody;
	private Transform m_transform;
	private InputHub m_input;
	private MovementLogic m_movement;
	private RemoteMovement m_remoteMovement;

	public float m_snapDistance = 1f;
	public bool m_nudge = false;
	public float m_nudgeDistance = 0.05f;

	private int m_id;

	private bool intiated = false;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
		m_ghostPosition = transform.position;
		m_input = GetComponent<InputHub>();
		intiated = true;

		if(Network.isServer){
			m_movement = GetComponent<MovementLogic>();
		}else{
			m_remoteMovement = GetComponent<RemoteMovement>();
		}
	}

	public void assignData(SyncData sd){
		if(intiated){
			m_ghostPosition = sd.m_position;
			m_rigidbody.velocity = sd.m_velocity;
			m_remoteMovement.setSyncRotationSpeed(sd.m_rotationSpeed);
			m_input.setCurrentBlowingPower(sd.m_blowing);

			m_transform.rotation = sd.m_rotation; // this one might need a snap treshold

			if(Vector3.Distance(m_ghostPosition,transform.position) > m_snapDistance){
				transform.position = m_ghostPosition;
			}else if (m_nudge){
				Vector3 dir = (m_ghostPosition - transform.position);
				if(dir.magnitude > 1){
					dir.Normalize();
				}
				transform.position += dir * m_nudgeDistance;
			}

		}
	}

	public SyncData getData(){
		if(!intiated) return new SyncData();
		Vector3 position = m_transform.position;
		Vector3 velocity = m_rigidbody.velocity;
		Quaternion rotation = m_transform.rotation;
		float rotationSpeed = m_movement.getRotationSpeed();
		float blowing = m_input.getCurrentBlowingPower();
		return new SyncData(position,velocity,rotation,rotationSpeed,blowing);
	}

	void OnDrawGizmos(){
		if(Network.isClient){
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(m_ghostPosition,0.5f);
		}
	}

	public void setID(int id){
		m_id = id;
		s_syncMovements[id] = this;
	}

	public int getID(){
		return m_id;
	}

}
