using UnityEngine;
using System.Collections;

public class otherTestCol : MonoBehaviour 
{
	MovementLogic me;
	MovementLogic opponent;

	public float oppAngleMinusValue = 10.0f;

	void Start()
	{

	}

	void OnCollisionEnter(Collision other)
	{
		opponent = other.gameObject.GetComponent<MovementLogic>();
		me = this.gameObject.GetComponent<MovementLogic> ();



		if(other.gameObject.tag == "Player")
		{
			//The centerline between the two "circles" .. 
			Vector3 cLine = other.transform.position - me.transform.position;
			
			//Info about "my" player..
			Vector3 myForwardVec = me.transform.forward;
			float myAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, myForwardVec));
			
			//Info about "opponent" player..
			Vector3 oppForwardVec = opponent.transform.forward;
			float oppAngleToCenter = Mathf.Abs (Vector3.Angle (cLine, oppForwardVec));

			if(opponent.getRigidVelocity() > me.getRigidVelocity() || myAngleToCenter > 45.0f || myAngleToCenter > (oppAngleToCenter - oppAngleMinusValue))
			{
//				Vector3 basisVector = other.transform.position - transform.position;
				cLine.Normalize();

				Vector3 othersVel = opponent.getRigidVelVect();
				float x1 = Vector3.Dot(cLine, othersVel);

				Vector3 othersXvel = cLine * x1;
				Vector3 othersYvel = othersVel 	- othersXvel;

				cLine *= -1.0f;
				Vector3 myVel = me.getRigidVelVect();
				float x2 = Vector3.Dot(cLine, myVel);

				Vector3 myXvel = cLine * x2;
				Vector3 myYVel = myVel - myXvel;


				Vector3 opponentsResultVel = myXvel + othersYvel;
				Vector3 myResultVel = othersXvel + myYVel;

//				opponent.setTackled(opponentsResultVel * 0.3f);
				me.setTackled(myResultVel);


				me.Invoke("restoreMovement", 1.0f);
				opponent.Invoke("restoreMovement", 1.0f);


			}
		}
	}
}
