using UnityEngine;
using System.Collections;

public class WinstateAnimation : MonoBehaviour {

	private SyncMovement[] m_playerObjs = new SyncMovement[4];
	private BuffManager[] m_buffManagers = new BuffManager[4];


	// Use this for initialization
	void Start () {
		m_playerObjs = SyncMovement.s_syncMovements;
		m_buffManagers = BuffManager.m_buffManagers;
	}

	public void playWinScene(int id){

		//for(int i = 0; i < m_playerObjs.Length; i++){
		if(m_playerObjs[id] != null && m_buffManagers[id] != null){
			//pos
			Vector3 tempPos = m_playerObjs[id].rigidbody.transform.position;
			m_playerObjs[id].rigidbody.transform.position = new Vector3(0,tempPos.y,0);

			//stunbuff
			//m_buffManagers[id].AddBuff(new StunBuff(m_buffManagers[id].gameObject, 0));

			for(int i = 0; i < m_buffManagers.Length; i++){
				if(m_buffManagers[i] != null){
					m_buffManagers[i].AddBuff(new StunBuff(m_buffManagers[i].gameObject, 0));
					if(i != id){

						m_buffManagers[i].gameObject.SetActive(false);

//						SkinnedMeshRenderer[] rendererList = m_buffManagers[i].gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
//						for(int j = 0; j < rendererList.Length; j++){
//							rendererList[j].enabled = false;
//						}
					}
				}
			}


			m_playerObjs[id].GetComponent<playerAnimation>().winAnim();

			//set position
			Camera.main.transform.position = new Vector3(-4,3,0);
			//disable smoothfollow
			Camera.main.GetComponent<CameraFollow>().enabled = false;

			//make mplayer look at camera
			m_playerObjs[id].transform.LookAt(new Vector3(-7,0,0));
			//
			Vector3 playerPos = m_playerObjs[id].transform.position - new Vector3(0,1,0);
			Camera.main.transform.LookAt(playerPos);

		}


		//}
	}
}
