using UnityEngine;
using System.Collections;

public class ParentMenuItem : BaseMenuItem 
{
	public ParentMenuItem(string aName, OnClickFunc aClickFunc, string aSceneName = "") : base(aName, aClickFunc)
	{
		m_SubMenuName = aSceneName;
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
