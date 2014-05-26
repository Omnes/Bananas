using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuBase : MonoBehaviour 
{
	protected List<BaseMenuItem> m_menuItems;
	protected MenuManager m_instance;
	private bool m_firstTime = true;
	public bool FirstTime{set{m_firstTime = value;}}

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
		float centerY = screenWidth/6;


		m_instance = MenuManager.Instance;
	}

	public virtual void DoGUI()
	{
		int i = 0;
		foreach(BaseMenuItem item in m_menuItems)
		{
			if(LeanTween.isTweening(item.LtRect) == false && m_firstTime == true)
			{
				LeanTween.move(item.LtRect, item.ToPos, 3.0f).setEase(item.LeanTweenType);
			}
			if(GUI.Button(item.LtRect.rect, item.Name))
			{
				if(item.OnClick != null)
				{
					item.OnClick(item);
				}
			}
			i++;
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
	public void AdjustMenuItem(BaseMenuItem aItem, LTRect aLtRect, Vector2 aToPosition, LeanTweenType aLeanTweenType = LeanTweenType.easeOutBounce)
	{
		aItem.LtRect = aLtRect;
		aItem.ToPos = aToPosition;
		aItem.LeanTweenType = aLeanTweenType;
	}


}
