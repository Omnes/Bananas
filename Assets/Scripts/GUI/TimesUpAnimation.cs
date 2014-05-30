using UnityEngine;
using System.Collections;

public class TimesUpAnimation : MonoBehaviour {

	public static TimesUpAnimation instance;

	// Use this for initialization
	void Start () {
		instance = this;
		renderer.enabled = false;
		transform.localPosition = new Vector3(0,-0.5f,transform.localPosition.z);
	}
	
	public void Play(Vector2 endPosition,float moveDuration,float fadeDuration){
		StartCoroutine(playAnimation(endPosition,moveDuration,fadeDuration));
	}

	private IEnumerator playAnimation(Vector2 endPosition,float moveDuration,float fadeDuration){
		Vector3 startPosition = new Vector3(0,0.5f,transform.localPosition.z);
		Vector3 startScale = new Vector3(0,0,0);
		Vector3 endScale = transform.localScale;
		float startTime = Time.time;
		renderer.enabled = true;

		while(startTime + moveDuration > Time.time){
			float f = (Time.time - startTime)/moveDuration;
			transform.localPosition = Vector3.Slerp(startPosition,endPosition,f);
			transform.localScale = Vector3.Slerp(startScale,endScale,f);

			yield return null;
		}
		while(startTime + moveDuration + fadeDuration > Time.time){
			float f = (Time.time - (startTime+moveDuration))/fadeDuration;
			renderer.material.color = Color.Lerp(Color.white,new Color(1,1,1,0),f);
			
			yield return null;
		}
		renderer.enabled = false;
		transform.localScale = endScale;
	}
	   
}
