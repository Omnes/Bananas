/*using UnityEngine;
using System.Collections;
using UnityEditor;

public class ReplaceObjectWithPrefab : EditorWindow {

	public Transform prefabToReplaceWith;
	
	[MenuItem("Window/Replace object with prefab")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(ReplaceObjectWithPrefab));
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		prefabToReplaceWith = EditorGUILayout.ObjectField(prefabToReplaceWith,typeof(Transform),true) as Transform;
		if(GUILayout.Button("Replace")){
			foreach (Transform t in Selection.transforms) {
				Transform go = Instantiate(prefabToReplaceWith,t.position,t.rotation)as Transform;
				go.lossyScalecale = t.lo;
				go.parent = t.parent;
				//DestroyImmediate (t.gameObject);
				
			}
		}
		EditorGUILayout.EndVertical();
		

	}
}
*/