using UnityEngine;
using System.Collections;

public class uvmapper : MonoBehaviour {

	public Rect map = new Rect(0,0,1,1);
	private MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponent<MeshFilter>();
		changeUvs();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.W)){
			changeUvs();
		}
	}

	void changeUvs(){
		Mesh mesh = meshFilter.mesh;
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
		meshFilter.mesh = mesh;

	}
}
