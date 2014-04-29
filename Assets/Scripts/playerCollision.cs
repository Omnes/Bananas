using UnityEngine;
using System.Collections;

public class playerCollision : MonoBehaviour {

	public float m_looserColForce;
	public scr_movementLogic m_movementLogic;
	public scr_movementLogic othersMovement;	
	public float e = 1.0f;
	float lastCollisionTime = 0.0f;


	void OnCollisionEnter(Collision other)
	{
		Vector3 StrongersDeltaForce = new Vector3();
		Vector3 WeakersDeltaForce = new Vector3();


		if(other.gameObject.tag == "Player" && Time.time > (lastCollisionTime + 1.0f))
		{
			lastCollisionTime = Time.time;
			othersMovement = other.gameObject.GetComponent<scr_movementLogic>();



			//Check if the "other player" has higher speed .. 
			if (othersMovement.getRigidVelocity() > m_movementLogic.getRigidVelocity()) //if other speed is higher than mine
			{
				CalculateForces(othersMovement, m_movementLogic, out StrongersDeltaForce, out WeakersDeltaForce);
				Debug.Log(m_movementLogic.name +" Calculated Stronger force: " + StrongersDeltaForce.ToString("F2"));
				Debug.Log(m_movementLogic.name + "Calculated Weaker force: " + WeakersDeltaForce.ToString("F2"));



//				m_movementLogic.rigidbody.AddForce(WeakersDeltaForce, ForceMode.VelocityChange);
//				othersMovement.rigidbody.AddForce(StrongersDeltaForce, ForceMode.VelocityChange);
				m_movementLogic.setTackled(WeakersDeltaForce);
				othersMovement.setTackled(StrongersDeltaForce * 0.05f);
				m_movementLogic.Invoke("restoreMovement", 1.0f);
				othersMovement.Invoke("restoreMovement", 1.0f);


				Debug.Log(m_movementLogic.name + " PlayerWithHigerVelAfter: " + othersMovement.rigidbody.velocity.ToString("F2"));
				Debug.Log(m_movementLogic.name + " PlayerWithLowerVelAfter: " + m_movementLogic.rigidbody.velocity.ToString("F2"));


			}
			else
			{
				scr_soundManager.Instance.playOneShot( "event:/Knockout! (1)", other.transform.position );
			}
		}
	}
	void CalculateForces(scr_movementLogic aStronger, scr_movementLogic aWeaker, out Vector3 aStrongerDeltaForce, out Vector3 aWeakerDeltaForce)
	{
		Debug.Log(m_movementLogic.name +" PlayerWithHigerVel: " + aStronger.rigidbody.velocity.ToString("F2"));
		Debug.Log(m_movementLogic.name +" PlayerWithLowerVel: " + m_movementLogic.rigidbody.velocity.ToString("F2"));

		//the normal .. 
		Vector3 normal = aStronger.transform.position - aWeaker.transform.position;
//		Debug.Log("Normal: " + normal.ToString("F2"));

		//Get some info about the colliders..
		Vector3 WeakerForwardForce = aWeaker.rigidbody.transform.forward; //(0, 0, 0)
		Vector3 StrongerForwardForce = aStronger.rigidbody.transform.forward; //(0, 0, 1)
//		Debug.Log(m_movementLogic.name + " StrongerForwardForce: " + StrongerForwardForce.ToString("F2"));
//		Debug.Log(m_movementLogic.name +" WeakerForwardForce: " + WeakerForwardForce.ToString("F2"));

		
		//Angles from "this" rigidbodys force to the normal.. 
		float WeakerAngle = Vector3.Angle(WeakerForwardForce, normal);
//		Debug.Log (m_movementLogic.name +" WeakerAngle: " + WeakerAngle.ToString ("F2"));
		//Angles from "other" rigidbodys force to the normal.. 
		float StrongerAngle = Vector3.Angle(StrongerForwardForce, normal);
//		Debug.Log (m_movementLogic.name +" StrongerAngle: " + StrongerAngle.ToString ("F2"));

		//forces in the two planes(before collision) for the rigidbody with the lowest velocity
		Vector3 WeakerTvecBefore = -(Mathf.Sin(WeakerAngle) * m_movementLogic.rigidbody.velocity);
		Vector3 WeakerNvecBefore = -(Mathf.Cos(WeakerAngle) * m_movementLogic.rigidbody.velocity);
		
		//forces in the two planes(before collision) for the rigidbody with the highest velocity
		Vector3 StrongerTvecBefore = (Mathf.Sin(StrongerAngle) * othersMovement.rigidbody.velocity);
		Vector3 StrongerNvecBefore = (Mathf.Cos(StrongerAngle) * othersMovement.rigidbody.velocity);
		
		//physical bullshit 
		Vector3 WeakerNvecAfter = (StrongerNvecBefore + WeakerNvecBefore + e * StrongerNvecBefore - e * WeakerNvecBefore) / 2.0f;
		Vector3 WeakerTvecAfter = WeakerTvecBefore;
		
		aWeakerDeltaForce = WeakerNvecAfter + WeakerTvecAfter;
		
		//---- || ----
		Vector3 StrongerNvecAfter = (StrongerNvecBefore + WeakerNvecBefore - e * StrongerNvecBefore + e * WeakerNvecBefore) / 2.0f;
		Vector3 StrongerTvecAfter = StrongerTvecBefore;


		aStrongerDeltaForce = StrongerNvecAfter + StrongerTvecAfter;
		




		

		

		//				m_movementLogic.Invoke("restoreMovement", 3);
		//If so make YOUR character "slidable" for 6 sec..
		
		//				rigidbody.AddExplosionForce(20.0f, other.rigidbody.position, 10.0f);
		//				other.rigidbody.AddExplosionForce(7.0f, rigidbody.position, 10.0f);

	}

}
