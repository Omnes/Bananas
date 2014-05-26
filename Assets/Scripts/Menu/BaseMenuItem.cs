using UnityEngine;
using System.Collections;

public class BaseMenuItem //: MonoBehaviour 
{
	protected string m_SubMenuName = "";
	public string SubMenuName { get { return m_SubMenuName; } }

	private string m_name;
	public string Name{ get { return m_name; } }



	//Individual button properties(ease type etc) ..
	private LTRect m_ltRect;
	public LTRect LtRect{ get { return m_ltRect; }set{m_ltRect = value; } }

	private Vector2 m_toPosition;
	public Vector2 ToPos{ get { return m_toPosition; }set{ m_toPosition = value; }}

	private LeanTweenType m_leanTweenType;
	public LeanTweenType LeanTweenType{ get { return m_leanTweenType; } set { m_leanTweenType = value; } }



	public delegate void OnClickFunc(BaseMenuItem aSender);

	private OnClickFunc m_onClickFunc;
	public OnClickFunc OnClick{ get { return m_onClickFunc; } }

//	private bool m_hasSubMenu;

	public BaseMenuItem(string aName, OnClickFunc aClickFunc)
	{
		m_onClickFunc = aClickFunc;
		m_name = aName;

	}
	// Use this for initialization
	void Start () 
	{
	
	}
	

	// Update is called once per frame
	void Update () 
	{
	
	}

}

