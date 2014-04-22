using UnityEngine;
using System.Collections;

public abstract class InputMetod : MonoBehaviour{
	public abstract float getCurrentBlowingPower();
	public abstract Vector2 getCurrentInputVector();
	public abstract void setCurrentBlowingPower(float f);
	public abstract void setCurrentInputVector(Vector2 v);

}

public class InputHub : MonoBehaviour {

	public InputMetod m_input;


	public float getCurrentBlowingPower(){
		return m_input.getCurrentBlowingPower();
	}

	public Vector2 getCurrentInputVector(){
		return m_input.getCurrentInputVector();
	}
	public void setCurrentInputVector(Vector2 v){
		m_input.setCurrentInputVector(v);
	}

	public void setCurrentBlowingPower(float f){
		m_input.setCurrentBlowingPower(f);
	}
}
