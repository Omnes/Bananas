﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UvMapperForLeifs : MonoBehaviour {

	public Rect map = new Rect(0,0,1,1);
	public int m_numberOfFrames = 5;
	private MeshFilter meshFilter;
	public bool m_updateInEditor = true;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponent<MeshFilter>();
		changeUvs();
	}

	void Update(){
		if(Application.isEditor && m_updateInEditor){
			changeUvs();
		}
	}

	void changeUvs(){

		int randomFrame = Random.Range(0,m_numberOfFrames);
		//hämta meshen
		Mesh mesh = meshFilter.mesh;
		Vector2[] uvs = new Vector2[4];
		//förkortningar
		float l = map.x+map.width*randomFrame;
		float r = l+map.width;
		float u = 1-map.height;
		float d = 1-map.y;

		//skapar nya uv kordinater
		 /*  0 - 2
		 * 	 | \ |
		 *   3 - 1
		 */
		uvs[0] = new Vector2(l,u); 
		uvs[1] = new Vector2(r,d);
		uvs[2] = new Vector2(r,u); 
		uvs[3] = new Vector2(l,d); 
		//assignar dem till meshen
		mesh.uv = uvs;
		//mesh.RecalculateNormals();
		meshFilter.mesh = mesh;

	}
}
