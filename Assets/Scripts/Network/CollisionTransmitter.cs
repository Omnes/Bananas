using UnityEngine;
using System.Collections;

public class CollisionTransmitter : MonoBehaviour {
	private GameObject _playerRef;
	public GameObject m_playerRef {
		set {
			_playerRef = value;
			m_buffManager = _playerRef.GetComponent<BuffManager> ();
			m_leafBlower = _playerRef.GetComponentInChildren<LeafBlower>();
			m_playerAnim = _playerRef.GetComponent<playerAnimation>();
		}
		get { return _playerRef;}
	}

	private int m_localPlayerID;
	private bool m_isLocal;

	private BuffManager m_buffManager;
	private playerAnimation m_playerAnim;
	private LeafBlower m_leafBlower;

	public float m_stunTime = 0.3f;
	public float m_dizzyTime = 2.0f;
	
	public void Start() {
//		m_buffManager = GetComponent<BuffManager> ();
//		m_leafBlower = m_playerRef.GetComponentInChildren<LeafBlower>();
//		m_playerAnim = GetComponent<playerAnimation>();

		m_localPlayerID = SeaNet.Instance.getLocalPlayer ();
		m_isLocal = SyncMovement.s_syncMovements [m_localPlayerID].isLocal;
	}

	public enum CollisionType {
		TACKLED,
		TACKLING,
		EQUAL
	}

	public void PlayerCollision(CollisionType type, int otherPlayerID) {
		networkView.RPC ("PlayerCollisionRPC", RPCMode.All, (int)type, otherPlayerID);
	}


	//Är inte säkert att den fungerar korrekt om RPC's kommer i fel ordning vid snabba kollisioner
	[RPC]
	private void PlayerCollisionRPC(int collisionType, int otherPlayerID)
	{
		if (otherPlayerID >= 0 && otherPlayerID < 4) {
			GameObject otherPlayer = SyncMovement.s_syncMovements[otherPlayerID].gameObject;

			if (m_isLocal) {
				SoundManager.Instance.play(SoundManager.KNOCKOUT);
			}
			
			if (collisionType == (int)CollisionType.TACKLED) {
				//Debug.Log ("Tackled");
				if (m_isLocal) {
					SoundManager.Instance.playOneShot(SoundManager.VOICE_TACKLED[m_localPlayerID]);
				}
				m_playerAnim.tackleLoseAnim(m_dizzyTime);
				m_leafBlower.requestDropAll();

				if (Network.isServer) {
					m_buffManager.AddBuff(new DizzyBuff(_playerRef, m_dizzyTime));
				}
			}
			else if (collisionType == (int)CollisionType.TACKLING) {
				//Debug.Log ("Tackling");
				if (m_isLocal) {
					SoundManager.Instance.playOneShot(SoundManager.VOICE_TACKLING[m_localPlayerID]);
				}
				m_playerAnim.tackleAnim(m_dizzyTime);
				
			}
			else if (collisionType == (int)CollisionType.EQUAL) {
				//Debug.Log ("Equal");
				m_playerAnim.tackleAnim(m_dizzyTime);
			}

			//Handle TimeBomb powerup
			if (m_buffManager.HasBuff((int)Buff.Type.TIME_BOMB)) {
				TimeBombBuff timeBombBuff = m_buffManager.GetBuff((int)Buff.Type.TIME_BOMB) as TimeBombBuff;
				if (timeBombBuff.CanTransfer()) {
					BuffManager othersBuffManager = otherPlayer.GetComponent<BuffManager> ();
					TimeBombBuff newTimeBombBuff = othersBuffManager.AddBuff(new TimeBombBuff(otherPlayer, timeBombBuff.m_duration)) as TimeBombBuff;
					newTimeBombBuff.TransferUpdate(timeBombBuff.m_durationTimer);
					m_buffManager.RemoveBuff((int)Buff.Type.TIME_BOMB);
				}
			}
		}
		else {
			Debug.LogError("CollisionTransmitter.cs: Received faulty playerID(" + otherPlayerID + ")");
		}
	}
}
