using UnityEngine;
using System.Collections;

public class EndCheck{

	private Rect m_startRect;
	private Texture2D m_texture;
	private Rect m_uvRect;

	public enum state { 
		CROSS,
		CHECK
	};

	public EndCheck(Rect newRect, state newState, Texture2D newTexture)
	{

		if(newState != state.CROSS){
			setCheck();
		}else{
			setCross();
		}

		m_startRect = newRect;
		m_texture = newTexture;
	}

	private void setCross(){
		m_uvRect =  new Rect(0.147f, 1-0.152f, 0.125f, 0.152f); 
	}

	private void setCheck(){
		m_uvRect = new Rect(0f, 1-0.152f, 0.148f, 0.152f);
	}

	public void drawCheck(){
		GUI.DrawTextureWithTexCoords(m_startRect, m_texture, m_uvRect);
	}
}
