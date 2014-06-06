using UnityEngine;
using System.Collections;

class ServerWidget{
	private Texture2D m_buttonAtlas;
	private Texture2D m_backgroundAtlas;
	private Rect m_texCordsBackground;
	private Rect m_texCordsButton;
	private Rect m_buttonPosition;
	private Rect m_textPosition;
	
	private HostData m_host;
	private Vector2 m_size;
	
	private GUIStyle m_guiStyle = GUIStyle.none;
	
	private LobbyButton m_joinButton;
	
	public ServerWidget(HostData host,Vector2 size){
		m_size = size;
		m_host = host;
		m_buttonAtlas = Prefactory.texture_buttonAtlas;
		m_backgroundAtlas = Prefactory.texture_backgrounds;

		// font
		m_guiStyle = new GUIStyle();
		m_guiStyle.alignment = TextAnchor.MiddleCenter;
		m_guiStyle.font = (Font)Resources.Load("Textures/Fonts/ARLRDBD");
		m_guiStyle.fontSize = Screen.height / 40;
		
		m_texCordsBackground = new Rect(0.7041f,1f - 0.2363f, 0.2929f,0.0859f);
		//		m_texCordsButton = new Rect(0.5634f,1f,0.3447f,0.1875f);
		m_texCordsButton = GUIMath.CalcTexCordsFromPixelRect(new Rect(577,835,350,190),1024);
		
		Vector2 buttonSize = new Vector2(m_size.x*0.3f,m_size.y * 0.45f);
		m_buttonPosition = new Rect(m_size.x*0.8f - buttonSize.x*0.5f,m_size.y*0.5f - buttonSize.y*0.5f,buttonSize.x,buttonSize.y);		
		m_textPosition = new Rect(m_size.x*0.1f,m_size.y*0.1f,m_size.x*0.6f,m_size.y*0.8f);
		
		m_joinButton = new LobbyButton(m_buttonPosition,m_texCordsButton);
	}

	public void Draw(Vector2 offset){
		
		GUI.DrawTextureWithTexCoords(new Rect(offset.x,offset.y,m_size.x,m_size.y),m_backgroundAtlas,m_texCordsBackground); //draw background
		string text = m_host.gameName + "\n" + (m_host.connectedPlayers)+"/"+m_host.playerLimit;
		GUI.Label(RectAddVector2(m_textPosition,offset), text,m_guiStyle);
		if(m_joinButton.isClicked(RectAddVector2(m_buttonPosition,offset))){
			Lobby.setName();
			Lobby.connectToServer(m_host);
		}
	}
	
	public Vector2 getSize(){
		return m_size;
	}
	
	Rect RectAddVector2(Rect r, Vector2 v){
		return new Rect(r.x+v.x, r.y+v.y,r.width,r.height);
	}
}