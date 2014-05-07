using UnityEngine;
using System.Collections;

public class GUITouches : MonoBehaviour {

	private Transform[] m_plupps =  new Transform[5];
	private Renderer[] m_renderers = new Renderer[5];
	private int m_foundTouches = 0;

	public TouchInput m_touchInput;
	//private Vector2 m_guiStickSize; 

	// Use this for initialization
	void Start () {
		//m_guiStickSize = m_touchInput.getGUIStickSize();
		m_foundTouches = transform.childCount;
		//saves all children and their renderers
		for(int i = 0; i < m_foundTouches; i++){
			m_plupps[i] = transform.GetChild(i);
			m_renderers[i] = m_plupps[i].renderer;
		}
	}

	public void init(TouchInput input){
		m_touchInput = input;
	}
	
	// Update is called once per frame
	void Update () {
		//kanske är oeffektivt att stänga av och sätta på dem
		//stänger av alla renders 
		for(int i = 0; i < m_foundTouches; i++){
			m_renderers[i].enabled = false;
		}
		//placerar alla pluppar på rätt ställen och enablar de som behövs
		Touch[] touches = Input.touches;
		for(int i = 0; i < Mathf.Min(touches.Length,m_foundTouches); i++){
			placeJoystick(touches[i].position,i);
		}
		//debug för editorn
		if(Application.isEditor && Input.GetMouseButton(0)){
			placeJoystick(Input.mousePosition,0);
		}
	}

	void placeJoystick(Vector2 inputPosition,int i){
		m_renderers[i].enabled = true;
		//offset med en halv skärm eftersom mitten av skärmen är 0,0
		//begränsar plupparna till sträcken
		//float min = 0.5f-m_guiStickSize.x/2;
		//float max = 0.5f+m_guiStickSize.x/2;
		//Vector2 constrainedPos = new Vector2(Mathf.Clamp(inputPosition.x,min,max),Mathf.Clamp(inputPosition.y,min,max));

		Vector2 screen_center = new Vector2(Screen.width/2,Screen.height/2);
		Vector2 pos = GUIMath.pixelsToPercent(inputPosition - screen_center); 


		//byt ut x mot pos.x för att frigöra plupparna från sträcken
		Vector3 finalpos = new Vector3(pos.x, pos.y, m_plupps[i].localPosition.z); 
		m_plupps[i].localPosition = finalpos;
	}
	

	Vector2 asVec2(Vector3 v){
		return new Vector2(v.x,v.y);
	}
}
