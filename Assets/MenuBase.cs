using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuBase : MonoBehaviour 
{
	protected List<BaseMenuItem> m_menuItems;
	protected List<LTRect> m_buttonRects;
	protected List<Vector2> m_centers;
	protected MenuManager m_instance;
	protected bool m_firstTime = true;

	private LTRect dasRect = null;
	private Vector2 center = new Vector2(300.0f, 100.0f);
	private Hashtable htb = new Hashtable();

	public float fcenter = 500.0f;

	// Use this for initialization
	void Start () 
	{
		htb.Add ("ease", LeanTweenType.easeOutElastic);
		m_instance = MenuManager.Instance;
//		Debug.Log ("base" + m_menuItems.Count.ToString ());
		Debug.Log ("base");
	}

//	}
	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//	}
	public virtual void DoGUI()
	{
		int i = 0;
		foreach(BaseMenuItem item in m_menuItems)
		{
			if(LeanTween.isTweening(m_buttonRects[i]) == false && m_firstTime == true)
			{
				LeanTween.move(m_buttonRects[i], m_centers[i], 1.0f).setEase(LeanTweenType.easeOutElastic);
			}
			if(GUI.Button(m_buttonRects[i].rect, item.Name))
			{
				if(item.OnClick != null)
				{
//					LeanTween.scale
					item.OnClick(item);
				}
			}
			i++;
		}
		m_firstTime = false;
	}
	
	protected void addMenuItem(BaseMenuItem aItem)
	{
		Debug.Log ("Added");
		m_menuItems.Add (aItem);
	}
}
