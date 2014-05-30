using UnityEngine;
using System.Collections;

public class CountdownAnimation : MonoBehaviour {

	public Transform m_numbers;
	public Transform m_brawl;

	public static CountdownAnimation instance;

	void Start(){
		instance = this;
		m_numbers.renderer.enabled = false;
		m_brawl.renderer.enabled = false;
	}

	public void Play(){
		StartCoroutine(PlayAnimation());
	}

	private IEnumerator PlayAnimation(){

		StartCoroutine(Animate(m_numbers,0.3f,0.3f));
		yield return new WaitForSeconds(0.6f);
		m_numbers.GetComponent<UvMapper>().changeUvs(1,0);

		StartCoroutine(Animate(m_numbers,0.3f,0.3f));
		yield return new WaitForSeconds(0.6f);
		m_numbers.GetComponent<UvMapper>().changeUvs(2,0);

		StartCoroutine(Animate(m_numbers,0.3f,0.3f));
		yield return new WaitForSeconds(0.5f);

		StartCoroutine(Animate(m_brawl,0.9f,0.6f));
		yield return new WaitForSeconds(0.65f);

		for(int i = 0; i < 4;i++){
			if(SyncMovement.s_syncMovements[i] != null){
				SyncMovement.s_syncMovements[i].GetComponent<InputHub>().ClearMovementStuns();
				SyncMovement.s_syncMovements[i].GetComponent<InputHub>().ClearLeafBlowerStuns();
			}
		}
	}

	private IEnumerator Animate(Transform obj,float moveDuration,float fadeDuration){

		Vector3 endScale = obj.localScale;
		Vector3 startPosition = new Vector3(0,-0.25f,transform.localPosition.z);
		Vector3 startScale = new Vector3(0,0,0);
		Vector3 endPosition = new Vector3(0,0,transform.localPosition.z);
		float startTime = Time.time;
		obj.renderer.enabled = true;
		
		while(startTime + moveDuration > Time.time){
			float f = (Time.time - startTime)/moveDuration;
			obj.renderer.material.color = Color.Lerp(new Color(1,1,1,0),Color.white,f);
			obj.localPosition = Vector3.Slerp(startPosition,endPosition,f);
			obj.localScale = Vector3.Slerp(startScale,endScale,f);
			
			yield return null;
		}
		while(startTime + moveDuration + fadeDuration > Time.time){
			float f = (Time.time - (startTime+moveDuration))/fadeDuration;
			obj.renderer.material.color = Color.Lerp(Color.white,new Color(1,1,1,0),f);
			
			yield return null;
		}
		obj.renderer.enabled = false;
		obj.renderer.material.color = Color.white;
		obj.localScale = endScale;
	}
	
}
