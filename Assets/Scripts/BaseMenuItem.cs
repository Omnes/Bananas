using UnityEngine;
using System.Collections;

public class BaseMenuItem //: MonoBehaviour 
{
	protected string m_SubSceneName = "";
	public string SubSceneName { get { return m_SubSceneName; } }

	private string m_name;
	public string Name{ get { return m_name; } }
	public delegate void OnClickFunc(BaseMenuItem aSender);
	private OnClickFunc m_onClickFunc;
	public OnClickFunc OnClick{ get { return m_onClickFunc; } }

	private bool m_hasSubMenu;

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

