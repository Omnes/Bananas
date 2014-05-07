using UnityEngine;
using System.Collections;

public abstract class InputMetod : MonoBehaviour{
	public abstract float getCurrentBlowingPower();
	public abstract Vector2 getCurrentInputVector();
	public abstract void setCurrentBlowingPower(float f);
	public abstract void setCurrentInputVector(Vector2 v);

}

public class InputHub : InputMetod{

	public InputMetod m_input;
	private uint m_stunned = 0;
	public void Stun(){m_stunned += 1;}
	public void UnStun(){m_stunned -= 1;}
	public void ClearStuns(){m_stunned = 0;}
	public bool Stunned{get{ return m_stunned > 0; }}

	public void setInputMetod(InputMetod input){
		m_input = input;
	}
	
	public override float getCurrentBlowingPower(){
		return Stunned ? 0 : m_input.getCurrentBlowingPower();
	}

	public override Vector2 getCurrentInputVector(){
		return Stunned ? Vector2.zero : m_input.getCurrentInputVector();
	}
	public override void setCurrentInputVector(Vector2 v){
		m_input.setCurrentInputVector(v);
	}

	public override void setCurrentBlowingPower(float f){
		m_input.setCurrentBlowingPower(f);
	}
}
