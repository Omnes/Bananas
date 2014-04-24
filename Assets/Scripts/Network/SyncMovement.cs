using UnityEngine;
using System.Collections;

public struct SyncData{
	public Vector3 m_velocity;
	public Vector3 m_position;
	public Quaternion m_rotation;
	public float m_blowing;

	public SyncData(Vector3 position,Vector3 velocity,Quaternion rotation,float blowing = 0f){
		m_position = position;
		m_velocity = velocity;
		m_rotation = rotation;
		m_blowing = blowing;
	}
};


public class SyncMovement : MonoBehaviour {
	
	public Vector3 m_ghostPosition = new Vector3();
	private Rigidbody m_rigidbody;
	private Transform m_transform;
	private InputHub m_input;

	public int m_id;

	// Use this for initialization
	void Start () {
		m_rigidbody = rigidbody;
		m_transform = transform;
		m_ghostPosition = transform.position;
		m_input = GetComponent<InputHub>();
	}

	public void assignData(SyncData sd){
		m_ghostPosition = sd.m_position;
		m_rigidbody.velocity = sd.m_velocity;
		m_transform.rotation = sd.m_rotation;
		m_input.setCurrentBlowingPower(sd.m_blowing);
	}

	public SyncData getData(){
		Vector3 position = m_transform.position;
		Vector3 velocity = m_rigidbody.velocity;
		Quaternion rotation = m_transform.rotation;
		float blowing = m_input.getCurrentBlowingPower();
		return new SyncData(position,velocity,rotation,blowing);
	}

	void OnDrawGizmos(){
		if(Network.isClient){
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(m_ghostPosition,0.5f);
		}
	}

	public void setID(int id){
		m_id = id;
	}

	public int getID(){
		return m_id;
	}

}
