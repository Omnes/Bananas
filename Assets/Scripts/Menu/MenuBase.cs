using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuBase : MonoBehaviour 
{
	protected List<BaseMenuItem> m_menuItems;
	protected MenuManager m_instance;
	private bool m_firstTime = true;
	public bool FirstTime{set{m_firstTime = value;}}

	public Texture m_backGround;
	public Texture m_btnImg;

	//Sound btns .. 
	protected int m_currentSoundBtn;
	public int SoundBtn{ get { return m_currentSoundBtn; } set { m_currentSoundBtn = value; } }

	//button proportions..
	protected int screenWidth;
	protected int screenHeight;
	protected Vector2 size;

	protected float centerX;
	protected float centerY;

	protected float leftX;
	protected float rightX;


	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
		float centerX = screenWidth/2 - (size.x / 2);
		float centerY = screenHeight/6;
		
		m_instance = MenuManager.Instance;
	}

	public virtual void DoGUI()
	{
		GUI.DrawTexture (new Rect (0.0f, 0.0f, screenWidth, screenHeight), m_backGround);
		foreach(BaseMenuItem item in m_menuItems)
		{
			if(LeanTween.isTweening(item.LtRect) == false && m_firstTime == true)
			{
				LeanTween.move(item.LtRect, item.ToPos, 3.0f).setEase(item.LeanTweenType);
			}
			if(CustomButton(item.LtRect.rect, m_btnImg, item.UVRect))
			{
				if(item.OnClick != null)
				{
					Debug.Log("click!");
					item.OnClick(item);
				}
			}
		}
		m_firstTime = false;
	}
	
	protected void addMenuItem(BaseMenuItem aItem)
	{
		m_menuItems.Add (aItem);
	}
	public virtual void InitMenuItems()
	{
	}
	public void AdjustMenuItem(BaseMenuItem aItem, LTRect aLtRect, Vector2 aToPosition, Rect aUvRect, LeanTweenType aLeanTweenType = LeanTweenType.punch)
	{
		aItem.LtRect = aLtRect;
		aItem.ToPos = aToPosition;
		aItem.UVRect = aUvRect;
		aItem.LeanTweenType = aLeanTweenType;

	}

	public bool CustomButton(Rect aPosition, Texture aButtonTexture, Rect aUvRect)
	{
		//Rita ut "i vilket fall" ..
		GUI.DrawTextureWithTexCoords (aPosition, aButtonTexture, aUvRect);
		if(Input.touchCount > 0 || (Input.GetMouseButtonDown(0)))
		{
			if(aPosition.Contains(Event.current.mousePosition))
			{
				Debug.Log("Pressed btn");
				return true;
			}
		}
		return false;
	}

}
