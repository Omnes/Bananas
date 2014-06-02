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

//	private Rect texCordsMute = new Rect (0.267f, 0.8455f, 0.154f, 0.155f);
//	private Rect texCordsUnmute = new Rect (0.422f, 0.8455f, 0.154f, 0.155f);

	//button proportions..
	protected int screenWidth;
	protected int screenHeight;
	protected Vector2 size;

	protected float centerX;
	protected float centerY;

	protected float leftX;
	protected float rightX;

	private static float s_lastClickTime = 0f;
	private const float CLICKCOOLDOWN = 0.3f;


	private static bool buttonDown = false;
	private static Rect btnPos;

	private LobbyButton m_muteButton;
	// Use this for initialization
	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		size = GUIMath.InchToPixels(new Vector2(1.5f, 0.8f));
		centerX = screenWidth/2 - (size.x / 2);
		centerY = screenHeight/6;

		m_instance = MenuManager.Instance;

	}

	public virtual void DoUpdate()
	{

	}


	public virtual void DoGUI()
	{

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

	public static bool CustomButton(Rect aPosition, Texture aButtonTexture, Rect aUvRect)
	{
		GUI.DrawTextureWithTexCoords (aPosition, aButtonTexture, aUvRect);
		//Rita ut "i vilket fall" ..
//		if(Time.time > (s_lastClickTime + CLICKCOOLDOWN ))
//		{
//			if(Input.GetMouseButtonDown(0) /*|| Input.GetTouch(0).phase == TouchPhase.Began)*/ && buttonDown == false)
//			{
//				if(aPosition.Contains(Event.current.mousePosition)/* || aPosition.Contains(Input.GetTouch(0).position)*/)
//				{
//					btnPos = aPosition;
//					Debug.Log("Down  " + aPosition);
//					buttonDown = true;
//				}
//			}
//			else 

		if(Input.touchCount > 0 || Input.GetMouseButtonDown(0) && (Time.time > s_lastClickTime + CLICKCOOLDOWN))
		{
			if(aPosition.Contains(Event.current.mousePosition))
			{
//				Debug.Log("up0  " + btnPos);
				buttonDown = false;
				if(aPosition.Contains(Event.current.mousePosition)/* || aPosition.Contains(Input.GetTouch(0).position)*/)
				{
//					Debug.Log("Up  " + btnPos);
//					s_lastClickTime = Time.time;
					SoundManager.Instance.playOneShot(SoundManager.BUTTON_CLICK);
					return true;
				}
			}
		}
		return false;
	}
}