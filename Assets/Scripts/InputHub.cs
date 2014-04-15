using UnityEngine;
using System.Collections;

public abstract class InputMetod : MonoBehaviour{
	public abstract float getCurrentBlowingPower();
	public abstract Vector2 getCurrentInputVector();
}

public class InputHub : MonoBehaviour {

	public InputMetod m_input;

	// Use this for initialization
	void Start () {
	
	}
	
	public float getCurrentBlowingPower(){
		return m_input.getCurrentBlowingPower();
	}

	public Vector2 getCurrentInputVector(){
		return m_input.getCurrentInputVector();
	}
}
