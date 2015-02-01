//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//
//[CustomEditor(typeof(PathNodeOld)), CanEditMultipleObjects]
//public class PathNodeEditor : Editor {
//
//	[MenuItem("PathEditor/Edit Path", true)]
//	static bool CheckNewPath() {
//		if (null == Selection.activeGameObject)
//			return false;
//
//		if(null == Selection.activeGameObject.GetComponent<TrackPiece> ())
//			return false;
//
//		return true;
//	}
//
//	[MenuItem("PathEditor/Edit Path")]
// 	static void NewPath() {
//		TrackPiece trackPiece = Selection.activeGameObject.GetComponent<TrackPiece> ();
//
//		if(null != trackPiece.transform.FindChild ("Path")) {
//			Debug.Log("Path already made. Selecting...");
//			Selection.activeObject = trackPiece.transform.FindChild ("Path").gameObject;
//		} else {
//			Debug.Log("Creating new path");
//			GameObject path = new GameObject ("Path");
//			path.transform.parent = trackPiece.transform;
//			path.transform.localPosition = Vector3.zero;
////			Selection.activeObject = SceneView.currentDrawingSceneView;
////			Camera sceneCam = SceneView.currentDrawingSceneView.camera;
////			path.transform.position = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
//
//			GameObject start = new GameObject("Start", typeof(PathNodeOld));
//			PathNodeOld.first = start.GetComponent<PathNodeOld>();
//			
//			GameObjectUtility.SetParentAndAlign(start, path);
//			Undo.RegisterCreatedObjectUndo(path, "Create " + path.name);
//			Selection.activeObject = start;
//		}
//	}
//
////	Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue};
//
//	public override void OnInspectorGUI() {
////		UpdateFirstNodeRef ();
////
//		DrawDefaultInspector ();
////
////		GUILayout.Space (10f);
////		if(targets.Length == 2) {
////			if(GUILayout.Button("Join")) {
////				NewJoinNode();
////			}
////			if(GUILayout.Button("Connect")) {
////				ConnectNodes();
////			}
////		} else { 
////			if (GUILayout.Button ("Insert before")) { //TODO make buttons centered and pretty
////				InsertBefore();
////			}
////			if (GUILayout.Button ("New/Split")) {
////				InsertAfter();
////			}
////			if (GUILayout.Button ("Delete")) {
////				DeleteNode();
////			}
////			serializedObject.Update();
////
////			EditorGUI.indentLevel++;
////			EditorGUILayout.Separator();
////			SerializedProperty curNodePaths = serializedObject.FindProperty ("nextPaths");
////			for(int i = 0; i < curNodePaths.arraySize; i++) {
////				GUILayout.BeginHorizontal();
////				GUIStyle style = new GUIStyle();
////				style.normal.textColor = colors[i % colors.Length];
////				
////				EditorGUILayout.LabelField("Path " + i, style);
////				if(GUILayout.Button("Reverse")) {
////					Reverse(Selection.activeGameObject.GetComponent<PathNode>(), i);
////				}
////				GUILayout.EndHorizontal();
////			}
////
////			serializedObject.ApplyModifiedProperties();
////		}
//	}
//
//	void OnSceneGUI() {
////		PathNode[] nodes = FindObjectsOfType<PathNode> ();
////		
////		for (int i = 0; i < nodes.Length; i++) {
////			if(nodes[i].nextPaths == null) {
////				Debug.Log("null");
////				return;
////			}
////			for(int j = 0; j < nodes[i].nextPaths.Count; j++) {
////				Handles.color = colors[j % colors.Length];
////				Quaternion rotation = Quaternion.LookRotation((nodes[i].nextPaths[j].transform.position - nodes[i].transform.position).normalized);
////				Handles.ArrowCap(0, nodes[i].transform.position, rotation, HandleUtility.GetHandleSize(nodes[i].transform.position));
////			}
////		}
//	}
//
//	/// <summary>
//	/// Reverse the specified from node and to index.
//	/// </summary>
//	void Reverse(PathNodeOld from, int to) {
//		from.nextPaths[to].nextPaths.Add (from);
//		from.nextPaths.Remove(from.nextPaths[to]);
//	}
//
//	void ConnectNodes() {
//		Selection.gameObjects [0].GetComponent<PathNodeOld> ().nextPaths.Add (Selection.gameObjects [1].GetComponent<PathNodeOld> ());
//		Repaint ();
//	}
//
//	void NewJoinNode() {
//		GameObject newNode = CreateNewNode ();
//		Vector3 averagePos = Vector3.zero;
//		for(int i = 0; i < Selection.gameObjects.Length; i++) {
//			averagePos = (averagePos + Selection.gameObjects[i].transform.position)/(i + 1);
//		}
//		SerializedObject[] serializedObjects = new SerializedObject[targets.Length];
//		for(int i = 0; i < targets.Length; i++) {
//			serializedObjects[i] = new SerializedObject(targets[i]);
//			serializedObjects[i].Update();
//			SerializedProperty curNodePaths = serializedObjects[i].FindProperty ("nextPaths");
//			curNodePaths.InsertArrayElementAtIndex (curNodePaths.arraySize);
//			curNodePaths.GetArrayElementAtIndex (curNodePaths.arraySize - 1).objectReferenceInstanceIDValue = newNode.GetInstanceID();
//
//			serializedObjects[i].ApplyModifiedProperties();
//		}
//	}
//
//	void DeleteNode() {
//		serializedObject.Update ();
//		SerializedProperty curNodePaths = serializedObject.FindProperty ("nextPaths");
//
//		GameObject curNode = (GameObject)Selection.activeObject;
//
//		// Kill me
//		List<PathNodeOld> toSearch = new List<PathNodeOld> ();
//		toSearch.Add(PathNodeOld.first);
//
//		List<PathNodeOld> searched = new List<PathNodeOld>();
//		List<PathNodeOld> linked = new List<PathNodeOld> ();
//
//		while(toSearch.Count > 0) {
//			PathNodeOld cur = toSearch[0];
//			Debug.Log("moved");
//			for(int j = 0; j < cur.nextPaths.Count; j++) {
////				if(!searched.Contains(cur.paths[j])) {
//				if(cur.nextPaths[j] == curNode.GetComponent<PathNodeOld>()) {
//					linked.Add(cur.nextPaths[j]);
//				}
//				toSearch.Add(cur.nextPaths[j]);
//				searched.Add(cur.nextPaths[j]);
////				}
//			}
//			toSearch.Remove(cur);
//		}
//
//		Debug.Log ("Linked: " + linked.Count);
//
//		for(int i = 0; i < linked.Count; i++) {
//			for(int j = 0; j < linked[i].nextPaths.Count; j++) {
//				if(linked[i].nextPaths[j] == curNode.GetComponent<PathNodeOld>()) {
//					linked[i].nextPaths[j] = curNode.GetComponent<PathNodeOld>().nextPaths[0];
//				}
//			}
//		}
//
//		if(curNodePaths.arraySize == 0) {
//			DestroyImmediate(curNode);
//		}
//
////		if(null != serializedObject)
////			serializedObject.ApplyModifiedProperties ();
//	}
//
//	void InsertBefore() {
//		PathNodeOld oldNode = Selection.activeGameObject.GetComponent<PathNodeOld> ();
//		GameObject newNode = CreateNewNode ();
//
//		newNode.GetComponent<PathNodeOld> ().nextPaths = oldNode.nextPaths;
//		oldNode.nextPaths.Clear();
//		oldNode.nextPaths.Add(newNode.GetComponent<PathNodeOld>());
//	}
//
//	void InsertAfter() {
//		GameObject newNode = CreateNewNode ();
//
//		serializedObject.Update ();
//		SerializedProperty curNodePaths = serializedObject.FindProperty ("nextPaths");
//		curNodePaths.InsertArrayElementAtIndex (curNodePaths.arraySize);
//		curNodePaths.GetArrayElementAtIndex (curNodePaths.arraySize - 1).objectReferenceInstanceIDValue = newNode.GetInstanceID();
//		
//		serializedObject.ApplyModifiedProperties ();
//	}
//	
//	GameObject CreateNewNode() {
//		GameObject newNode = new GameObject ("Node");
//		newNode.AddComponent<PathNodeOld> ();
//		newNode.transform.parent = PathNodeOld.first.transform.parent;
//		newNode.transform.position = Selection.activeGameObject.transform.position;
//		Selection.activeObject = newNode;
//		return newNode;
//	}
//
//	void UpdateFirstNodeRef() {
//		if(null == PathNodeOld.first) 
//			PathNodeOld.first = GameObject.Find("Path").transform.FindChild("Start").GetComponent<PathNodeOld>();
//	}
//}
