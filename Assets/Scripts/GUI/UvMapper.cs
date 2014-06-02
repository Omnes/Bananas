using UnityEngine;
using System.Collections;

/*
 * UVmaps a quad to apart of a texture
 * 
 */

//[ExecuteInEditMode]
public class UvMapper : MonoBehaviour {

	public Rect map = new Rect(0,0,1,1);
	private MeshFilter meshFilter;
	public bool m_updateInEditor = true;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponent<MeshFilter>();
		changeUvs();
	}
	/*
	void Update(){
		if(Application.isEditor && m_updateInEditor){
			changeUvs();
		}
	}
	*/

	public void changeUvs(int colum = 0,int row = 0){
		//hämta meshen
		Mesh mesh = meshFilter.mesh;
		Vector2[] uvs = new Vector2[4];
		//förkortningar
		float l = map.x + map.width*colum;
		float r = map.x + map.width + map.width*colum;
		float u = 1-(map.y + map.height + map.height*row);
		float d = 1-(map.y + map.height*row);

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
