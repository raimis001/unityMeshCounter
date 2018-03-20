using UnityEngine;
using UnityEditor;
public class SelectionCount : EditorWindow
{


	[MenuItem("Window/SelectionCount")]  

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SelectionCount));
	}


	void OnInspectorUpdate()
	{
		Repaint();
	}

	private float trissActive;
	private float trissDisable;
	private float skinedActive;
	private float skinedDisable;
	private float meshesActive;
	private float meshesDisabled;

	public void OnGUI()
	{

		GameObject[] selectedObjects = Selection.gameObjects;

		int objCount = 0;
		int objDisabled = 0;
		string mainName = "";

		trissActive = 0;
		trissDisable = 0;
		skinedActive = 0;
		skinedDisable = 0;
		meshesActive = 0;
		meshesDisabled = 0;

		if (selectedObjects.Length > 0)
		{

			mainName = selectedObjects[0].name;

			foreach (GameObject c in selectedObjects)
			{
				Transform[] gs = c.GetComponentsInChildren<Transform>(true);
				objCount += gs.Length;

				foreach (Transform tr in gs)
				{
					if (tr.gameObject.activeInHierarchy) continue;
					objDisabled++;
				}

				FindInChild(c);
			}

		}

		GUILayout.Label("Selected Object:" + mainName);
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("child count:" + objCount);
		GUILayout.Label("disabled count:" + objDisabled);
		GUILayout.EndHorizontal();

		GUILayout.Space(5);
		GUILayout.Label("Active objects");

		GUILayout.BeginHorizontal("box");
		GUILayout.Label(string.Format("3ds Max Tris: {0:### ### ###}",trissActive / 3f));
		GUILayout.Label(string.Format("Unity Tris: {0:### ### ###}", trissActive));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Mesh count " + meshesActive);
		GUILayout.Label("Skinned mesh " + skinedActive);
		GUILayout.EndHorizontal();

		GUILayout.Space(2);
		GUILayout.Label("Disabled objects");

		GUILayout.BeginHorizontal("box");
		GUILayout.Label(string.Format("3ds Max Tris: {0:### ### ###}", trissDisable / 3f));
		GUILayout.Label(string.Format("Unity Tris: {0:### ### ###}", trissDisable));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Mesh count " + meshesDisabled);
		GUILayout.Label("Skinned mesh " + skinedDisable);
		GUILayout.EndHorizontal();
	}

	private void FindInChild(GameObject g)
	{
		
		MeshFilter[] meshs = g.GetComponentsInChildren<MeshFilter>(true);

		foreach (MeshFilter mesh in meshs)
		{
			if (!mesh || !mesh.sharedMesh) continue;
			float m = mesh.sharedMesh.triangles.Length;
			MeshRenderer ms = mesh.gameObject.GetComponent<MeshRenderer>();
			if (ms && ms.enabled && mesh.gameObject.activeInHierarchy)
			{
				meshesActive++;
				trissActive += m;
			}
			else
			{
				meshesDisabled++;
				trissDisable += m;
			}
		}

		SkinnedMeshRenderer[] skins = g.GetComponentsInChildren<SkinnedMeshRenderer>(true);

		foreach (SkinnedMeshRenderer mesh in skins)
		{
			float m = mesh.sharedMesh.triangles.Length;
			if (mesh.enabled && mesh.gameObject.activeInHierarchy)
			{
				skinedActive++;
				trissActive += m;
			}
			else
			{
				skinedDisable++;
				trissDisable += m;
			}
		}


	}

}