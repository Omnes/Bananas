using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class ServerList{
	private List<ServerWidget> m_widgets = new List<ServerWidget>();
	private float m_scrollOffset = 0;
	private Texture2D m_atlas;
	private Rect texCordsBackground;
	private Rect m_area;
	private Vector2 m_widgetSize;
	private float m_maxScrollOffset = 0;
	
	public ServerList(Rect area){
		m_area = area;
		m_atlas = Prefactory.texture_backgrounds;
		texCordsBackground = GUIMath.CalcTexCordsFromPixelRect(new Rect(338,456,360,457),1024);
		m_widgetSize = new Vector2(m_area.width*0.85f,m_area.height*0.2f);
	}
	
	public void SetHostList(HostData[] hostData){
		m_widgets.Clear();
		
		for (int i = 0; i < hostData.Length; i++) {
			if( hostData[i].connectedPlayers < hostData[i].playerLimit){
				m_widgets.Add(new ServerWidget(hostData[i],m_widgetSize));
			}
		}
		
		m_maxScrollOffset = Mathf.Clamp(m_widgetSize.y * m_widgets.Count - m_area.height,0,float.MaxValue);
		m_scrollOffset = Mathf.Clamp(m_scrollOffset,-m_maxScrollOffset,m_maxScrollOffset);
	}
	
	public void update(){
		if(m_widgetSize.y * m_widgets.Count > m_area.height){
			Touch[] touches = Input.touches;
			if(touches.Length > 0){
				if(m_area.Contains(touches[0].position)){
					scroll(touches[0].deltaPosition.y*2f);
				}
			}
			scroll(Input.GetAxis("Mouse ScrollWheel")*200f);
		}
	}
	private void scroll(float delta){
		m_scrollOffset += delta;
		m_scrollOffset = Mathf.Clamp(m_scrollOffset,-m_maxScrollOffset,m_maxScrollOffset);
	}
	
	public void Draw(){
		GUI.DrawTextureWithTexCoords(m_area,m_atlas,texCordsBackground); //draw background
		//		GUI.Box(m_area,"Area");
		GUI.BeginGroup(m_area);
		float paddingX = m_area.width * 0.03f;
		float paddingY = m_area.height * 0.01f;
		//		GUI.Box (m_area,"area");
		for (int i = 0; i < m_widgets.Count; i++) {
			float yPos = paddingY*2f + (m_widgets[i].getSize().y + paddingY) * i + m_scrollOffset;
			if((yPos < m_area.height ) && (yPos + m_widgetSize.y > 0f)){
				m_widgets[i].Draw(new Vector2(paddingX, yPos)); //add padding
			}
		}
		GUI.EndGroup();
	}
}