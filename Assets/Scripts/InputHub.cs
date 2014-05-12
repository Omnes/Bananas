using UnityEngine;
using System.Collections;

public abstract class InputMetod : MonoBehaviour {
	public abstract float getCurrentBlowingPower();
	public abstract Vector2 getCurrentInputVector();
	public abstract void setCurrentBlowingPower(float f);
	public abstract void setCurrentInputVector(Vector2 v);
}

public class InputHub : InputMetod {
	public InputMetod m_input;

	private uint m_movementStunned = 0;
	public void StunMovement(){m_movementStunned += 1;}
	public void UnStunMovement(){m_movementStunned -= 1;}
	public void ClearMovementStuns(){m_movementStunned = 0;}
	public bool MovementStunned{get{ return m_movementStunned > 0; }}

	private uint m_leafBlowerStunned = 0;
	public void StunLeafBlower(){m_leafBlowerStunned += 1;}
	public void UnStunLeafBlower(){m_leafBlowerStunned -= 1;}
	public void ClearLeafBlowerStuns(){m_leafBlowerStunned = 0;}
	public bool LeafBlowerStunned{get{ return m_leafBlowerStunned > 0; }}

	public void setInputMetod(InputMetod input) {
		m_input = input;
	}
	
	public override float getCurrentBlowingPower() {
		return LeafBlowerStunned ? 0 : m_input.getCurrentBlowingPower();
	}

	public override Vector2 getCurrentInputVector() {
		return MovementStunned ? Vector2.zero : m_input.getCurrentInputVector();
	}
	public override void setCurrentInputVector(Vector2 v) {
		m_input.setCurrentInputVector(v);
	}

	public override void setCurrentBlowingPower(float f) {
		m_input.setCurrentBlowingPower(f);
	}
}
