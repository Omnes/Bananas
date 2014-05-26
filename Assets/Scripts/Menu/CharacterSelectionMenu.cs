using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionMenu : MenuBase
{

	
	private int positionInArray = 0;
	
	//Swipe stuff ..
	private bool swiping = true;
	private Vector2 swipeStartPos;
	private float swipeStartTime;
	private float comfortZone = 800.0f;			//Use to make sure that the swipe is fairly straight .. 

	private float maxSwipeTime = 0.5f;
	private float minSwipeDist = 500.0f;

	private bool cameraIsMoving = false;

	private static GameObject[] charObjs = new GameObject[4];
	private GameObject currentObj;
	private float swipeDir;


	//Debug ..
	private string stat ="";
	private Vector2 latest;

	void Start()
	{
		charObjs[0] = GameObject.Find("first");
		charObjs[1] = GameObject.Find("second");
		charObjs[2] = GameObject.Find("third");
		charObjs[3] = GameObject.Find("fourth");
	}
	public override void DoGUI()
	{

		if(Input.touchCount > 0 && swiping)
		{
			Touch theTouch = Input.touches[0];
			switch(theTouch.phase)
			{
			case TouchPhase.Began:
				swiping = true;
				swipeStartPos = theTouch.position;
				swipeStartTime = Time.time;
				break;
			case TouchPhase.Moved:
				latest = theTouch.position;
				if(Mathf.Abs (theTouch.position.y - swipeStartPos.y) >= comfortZone)
				{
					swiping = false;
				}
				break;
			case TouchPhase.Ended:
				float totalSwipeTime = Time.time - swipeStartTime;
				float totalSwipeDist = Mathf.Abs(theTouch.position.x - swipeStartPos.x);
				if(swiping && (totalSwipeTime < maxSwipeTime) && (totalSwipeDist > minSwipeDist))
				{
					swiping = false;
					swipeDir = Mathf.Sign(theTouch.position.magnitude - swipeStartPos.magnitude);

					//"Circle rotation" .. 
					if(swipeDir < 0.0f && positionInArray == 0)
					{
						positionInArray = 3;
					}
					else if(swipeDir > 0.0f && positionInArray == 3)
					{
						positionInArray = 0;
					}
					else
					{
						positionInArray +=(int) Mathf.Floor(swipeDir);
					}



					currentObj = charObjs[positionInArray];
					if(cameraIsMoving == false)
					{
						StartCoroutine(SmoothCameraRotate(currentObj));
					}
				}
				break;
			default:
				break;
			}
		}
		GUI.TextArea(new Rect(1600.0f, 100.0f, 100.0f, 30.0f), latest.ToString("F2"));
		GUI.TextArea(new Rect(1200.0f, 100.0f, 100.0f, 30.0f), stat);
		GUI.TextArea(new Rect(800.0f, 100.0f, 100.0f, 30.0f), swipeStartPos.ToString("F2"));
		GUI.TextArea(new Rect(300.0f, 100.0f, 100.0f, 30.0f), swipeDir.ToString("F2"));
		GUI.TextArea(new Rect(500.0f, 100.0f, 100.0f, 30.0f), positionInArray.ToString("F2"));
	}
	
	IEnumerator SmoothCameraRotate(GameObject aTargetObj)
	{
		cameraIsMoving = true;
		float i = 0.0f;
		float rate = 1 / 4.0f;

		while(i < 1)
		{
			i += Time.deltaTime * rate;
			Vector3 relativeToPlayer = currentObj.transform.position - Camera.main.transform.position;
			Quaternion lookAt = Quaternion.LookRotation(relativeToPlayer);
			Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, i);
			yield return null;
		}
		cameraIsMoving = false;

	}
	
}
