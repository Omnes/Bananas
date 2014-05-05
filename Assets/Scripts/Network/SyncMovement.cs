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

	public float m_resyncForce = 5f;
	public float m_resyncLimit = 0.05f;

	private Vector3 m_ghostPosition = new Vector3();
	private Vector3 m_predictedPosition = new Vector3();
	private Vector3 m_syncVelocity = new Vector3();
	private Rigidbody m_rigidbody;
	private Transform m_transform;
	private InputHub m_input;
	private MovementLogic m_movement;
	private RemoteMovement m_remoteMovement;
	private int m_id;

	private bool m_intiated = false;

	private float m_lastSyncTime = 0f;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
		m_ghostPosition = transform.position;
		m_predictedPosition = transform.position;
		m_syncVelocity = rigidbody.velocity;
		m_input = GetComponent<InputHub>();
		m_intiated = true;
		m_lastSyncTime = Time.time;
		if(Network.isServer){
			m_movement = GetComponent<MovementLogic>();
		}else{
			m_remoteMovement = GetComponent<RemoteMovement>();
		}
	}

	public void assignData(SyncData sd){
		if(m_intiated){
			m_ghostPosition = sd.m_position;
			//m_rigidbody.velocity = sd.m_velocity;
			m_remoteMovement.setSyncRotationSpeed(sd.m_rotationSpeed);
			m_input.setCurrentBlowingPower(sd.m_blowing);
			m_transform.rotation = sd.m_rotation; // this one might need a snap treshold
			m_syncVelocity = sd.m_velocity;

			float syncDeltaTime = Time.time - m_lastSyncTime;
			m_predictedPosition = m_ghostPosition + sd.m_velocity * syncDeltaTime;
		}
		m_lastSyncTime = Time.time;
	}

	void Update(){
		if(Network.isClient){
			Vector3 predictedDelta = m_predictedPosition - m_transform.position;
			
			//how much we should use of each velocity if we are more desynced we should use more of the predicted correction
			float max = m_syncVelocity.magnitude + predictedDelta.magnitude;
			if(max < m_resyncLimit){
				max = 1f;
			}
			float share = m_syncVelocity.magnitude / max;

			//normalize if over 1
			Vector3 predictedDir = predictedDelta.magnitude > 1 ? predictedDelta.normalized : predictedDelta;

			Vector3 newVelocity = m_syncVelocity * share + predictedDir * m_resyncForce * (1-share);
			
			m_rigidbody.velocity = newVelocity;
		}
	}

	public SyncData getData(){
		if(!m_intiated) return new SyncData();
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
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(m_predictedPosition,0.5f);
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
