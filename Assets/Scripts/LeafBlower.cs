using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;

//TODO: Gör om distanceToLeaf så att den använder vector2D
public class LeafBlower : MonoBehaviour {

	public static LeafBlower[] s_leafBlowers = new LeafBlower[4];
	public int m_id = -1;
	
	public ParticleSystem m_particleSystem;
	public float m_randomLeafInBlowerRange = 1f;
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
	public float m_lowestSpeedModifier = 0.5f;

	void Start()
	{

		m_animation = transform.parent.GetComponent<playerAnimation>();

		m_touchInput = transform.parent.GetComponent<InputHub>();

		m_blowSound = SoundManager.Instance.play(SoundManager.LEAFBLOWER);
		playerTransform = transform;
	}

	void Update()
	{
		m_blowPower = m_touchInput.getCurrentBlowingPower();
		m_blowSound.setVolume (m_blowPower);

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

		if(Input.GetKeyDown(KeyCode.A)){ // TEMP DEBUG
			if(Network.isServer){
				requestDrop(10);
			}
		}
		tmp_canPickup = m_collectedLeafs.Count < m_maxLeaf;

	}
	
	public void addLeaf(Transform leaf){
		float randomAngle = Random.Range(0f,360f);
		float randomDistance = Random.Range(-m_randomLeafInBlowerRange,m_randomLeafInBlowerRange);
		Vector2 vec = new Vector2(Mathf.Cos (randomAngle),Mathf.Sin(randomAngle))* randomDistance;
		Vector3 randomPosition = new Vector3(vec.x,0,vec.y);
		leaf.GetComponent<LeafLogic>().addToWhirlwind(randomPosition,m_whirlwind);
		m_collectedLeafs.Add(leaf);
	}

	public void requestDropAll(){
		requestDrop(m_collectedLeafs.Count);
	}

	public void requestDrop(int count){
		LeafManager.s_lazyInstance.requestLeafDrop(m_id,count);
	}


	public void dropLeafs(int count,int seed){
		Random.seed = seed;
		count =  Mathf.Min(count,m_collectedLeafs.Count); //we cant drop more leafs than we have
		for (int i = 0; i < count; i++){
			m_collectedLeafs[i].GetComponent<LeafLogic>().dropFromWhirlwind(getRandomFallSpot());
		}
		m_collectedLeafs.RemoveRange(0,count);
	}

	private Vector3 getRandomFallSpot(){
		float randomAngle = Random.Range(0f,360f);
		float randomDistance = Random.Range(-m_randomLeafFallRange,m_randomLeafFallRange);
		return m_whirlwind.position + new Vector3(Mathf.Cos(randomAngle),0,Mathf.Sin(randomAngle)) * randomDistance;
	}
	


	public void OnDestroy()
	{
		if (SoundManager.IsNull() == false) {
			SoundManager.Instance.DestroySound (m_blowSound);
		}
	}

	//this is triggered by collisions in the child's colliders
	public void OnTriggerEnterInChild(Collider other)
	{
		if (Network.isServer) {
			if (other.CompareTag("Leaf") && tmp_canPickup) {
				Transform leaf = other.transform;

				LeafManager.s_lazyInstance.pickUpLeaf(m_id,leaf);

			}
		}
	}

	void returnLeafsToPool(int count){
		for(int i = 0;i < count; i++){
			m_collectedLeafs[i].GetComponent<LeafLogic>().clean();
			m_collectedLeafs[i].gameObject.SetActive(false);
		}
		m_collectedLeafs.RemoveRange(0,count);
	}

	//TODO: Authorativ
	//this is the own "leaf dumper" trigger -- this gives the score to the players
	public void OnTriggerEnter(Collider other)
	{
//		if (Network.isServer) {
		if (other.CompareTag("Leaf_collector")) {
			int nrOfLeafs = m_collectedLeafs.Count;
			returnLeafsToPool(nrOfLeafs);
			int id = other.gameObject.GetComponent<CollectorCollider>().m_ID;
			ScoreKeeper.m_scores[id] += nrOfLeafs;
			SoundManager.Instance.playOneShot(SoundManager.SCORE);
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


	public void setWhirlwind(Transform whirl){
		m_whirlwind = whirl;
	}
}


























