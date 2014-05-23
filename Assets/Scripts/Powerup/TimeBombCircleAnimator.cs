using UnityEngine;
using System.Collections;

//TODO:Fixa så att den alltid börjar roterad rakt ner
public class TimeBombCircleAnimator : MonoBehaviour {
	private float rotationSpeed = -360 / TimeBombBuff.BOMB_DURATION_MAX;
	private float angle = 0;

	void LateUpdate () {
		transform.rotation = Quaternion.Euler(90, 0, angle);
		angle += Time.deltaTime * rotationSpeed;
	}

}
