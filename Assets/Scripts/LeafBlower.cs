using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;

//TODO: Gör om distanceToLeaf så att den använder vector2D
public class LeafBlower : MonoBehaviour {
	
	public ParticleSystem m_particleSystem;
	public float m_randomLeafRange = 1f;
	public float m_randomLeafFallRange = 4f;

	private bool m_particleEmit = false;
	private playerAnimation m_animation;
	private float m_blowPower = 0.0f;	
	private Transform playerTransform;
	private InputHub m_touchInput;
	private FMOD.Studio.EventInstance m_blowSound;

	private List<Transform> m_collectedLeafs = new List<Transform>();

	public Transform m_whirlwind;

	bool tmp_canPickup = true; //TEMP

	public int m_leafThreshold = 10;
	public int m_maxLeaf = 30;
	public float m_lowestSpeedModifier = 0.1f;

	void Start()
	{

		m_animation = transform.parent.GetComponent<playerAnimation>();

		m_touchInput = transform.parent.GetComponent<InputHub>();

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
				m_animation.blowAnim();

				m_particleEmit = true;
				m_particleSystem.Play();
			}
		}else if(m_particleEmit){
			m_animation.stopBlowAnim();
			m_particleEmit = false;
			m_particleSystem.Stop();
		}

		if(Input.GetKeyDown(KeyCode.A)){
			dropLeafs(10);
		}
		tmp_canPickup = m_collectedLeafs.Count < m_maxLeaf;

	}


//	void FixedUpdate(){
//		for(int i = 0; i < m_collectedLeafs.Count; i++){
//			m_collectedLeafs[]
//		}
//	}
	private void addLeaf(Transform leaf){
		float randomAngle = Random.Range(0f,360f);
		float randomDistance = Random.Range(-m_randomLeafRange,m_randomLeafRange);
		Vector2 vec = new Vector2(Mathf.Cos (randomAngle),Mathf.Sin(randomAngle))* randomDistance;
		Vector3 randomPosition = new Vector3(vec.x,0,vec.y);
		leaf.GetComponent<LeifLogic>().addToWhirlwind(randomPosition,m_whirlwind);
		m_collectedLeafs.Add(leaf);
	}

	public void dropLeafs(int count){
		count =  Mathf.Min(count,m_collectedLeafs.Count); //we cant drop more leafs than we have
		for (int i = 0; i < count; i++){
			m_collectedLeafs[i].GetComponent<LeifLogic>().dropFromWhirlwind(getRandomFallSpot());
		}
		m_collectedLeafs.RemoveRange(0,count);
	}

	Vector3 getRandomFallSpot(){
		float randomAngle = Random.Range(0f,360f);
		float randomDistance = Random.Range(-m_randomLeafFallRange,m_randomLeafFallRange);
		return m_whirlwind.position + new Vector3(Mathf.Cos(randomAngle),0,Mathf.Sin(randomAngle)) * randomDistance;
	}


	public void OnTriggerEnterInChild(Collider other)
	{
//		if (Network.isServer) {
			if (other.CompareTag("Leaf") && tmp_canPickup) {
				Transform leaf = other.transform;

				addLeaf(leaf);

			}
//		}
	}


	public float getLeafSpeedModifier(){
		float modifier = (float)(m_collectedLeafs.Count -  m_leafThreshold) / (float)(m_maxLeaf - m_leafThreshold); // current/max = percent
//		Debug.Log("Pre: " + modifier);
		modifier = Mathf.Clamp(1f - modifier,m_lowestSpeedModifier,1f); // invert and clamp
//		Debug.Log("Post: " + modifier);
		return modifier;
	}
}


























