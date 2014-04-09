using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class uvmapper : MonoBehaviour {

	public Rect map = new Rect(0,0,1,1);
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
		Mesh mesh = meshFilter.sharedMesh;
		Vector2[] uvs = new Vector2[4];
		float l = map.x;
		float r = map.width;
		float u = map.y;
		float d = map.height;

		uvs[0] = new Vector2(l,d); 
		uvs[1] = new Vector2(r,u);
		uvs[2] = new Vector2(r,d); 
		uvs[3] = new Vector2(l,u); 
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		meshFilter.sharedMesh = mesh;

	}
}
