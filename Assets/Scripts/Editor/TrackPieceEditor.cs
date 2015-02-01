//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using System.Collections.Generic;
//
//[CustomEditor(typeof(TrackPiece))]
//public class TrackPieceEditor : Editor {
//
//	const float SNAP_DISTANCE = 8.5f;
//	const float SNAP_ANGLE = -0.5f;
//
//	void OnSceneGUI() {
//		if(null == Selection.activeGameObject) // Nothing selected
//			return;
//
//		TrackPiece trackPiece = Selection.activeGameObject.GetComponent<TrackPiece> ();
//
//		if (null == trackPiece) // Is it a track piece? *Should* always work
//			return;
//
//		if(Selection.activeTransform.hasChanged) {
//			Event e = Event.current;
//			if (e.button == 0 && e.isMouse && e.type == EventType.MouseUp) {
////				SnapToNearest ();
//				ConnectToNeighbors();
//				Selection.activeTransform.hasChanged = false;
//			} else {
//				BreakConnections();
//			}
//		}
//
//		for (int i = 0; i < trackPiece.pathNodes.Count; i++) {
//			trackPiece.pathNodes[i].worldPosition = trackPiece.transform.TransformPoint(trackPiece.pathNodes[i].localPosition); 
//		}
//
////		if (null == trackPiece.pathNodes) {
////			trackPiece.pathNodes = new TrackPiece.PathNode[0];
////		}
//
////		Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue}; 
//
//		for (int i = 0; i < trackPiece.pathNodes.Count; i++) {
//			Event e = Event.current;
//			if (e.shift) {
//				trackPiece.pathNodes[i].localPosition = trackPiece.transform.InverseTransformPoint(Handles.PositionHandle(trackPiece.pathNodes[i].worldPosition, trackPiece.transform.rotation));
//			} else {
//				if(trackPiece.pathNodes[i].isConnection && !trackPiece.pathNodes[i].connected) {
//					Vector3 pos = trackPiece.pathNodes[i].worldPosition;
//					Quaternion rot = Quaternion.LookRotation(trackPiece.transform.TransformDirection(trackPiece.pathNodes[i].axis + Vector3.one * Vector3.kEpsilon));
//					Handles.color = Color.yellow;
//					Handles.ArrowCap(0, pos, rot, HandleUtility.GetHandleSize(pos)*1.2f);
//				}
//			}
//		}
//
//		DrawDirection ();
//	}
//
//	void DrawDirection() {
//		Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue};
//
//		TrackPiece[] trackPieces = GameObject.FindObjectsOfType<TrackPiece>();
//		for(int i = 0; i < trackPieces.Length; i++) {
//			for(int j = 0; j < trackPieces[i].pathNodes.Count; j++) {
//				for(int k = 0; k < trackPieces[i].pathNodes[j].nextPaths.Count; k++) {
////					if(trackPieces[j].pathNodes[i] != null) {
//					Handles.color = colors[k % colors.Length];
//					Vector3 lookDir = trackPieces[i].pathNodes[j].nextPaths[k].worldPosition - trackPieces[i].pathNodes[j].worldPosition;
//					if(lookDir == Vector3.zero)
//						lookDir += trackPieces[i].pathNodes[j].axis*0.01f;
//					Quaternion rotation = Quaternion.LookRotation((lookDir).normalized);
//					Handles.ArrowCap(0, trackPieces[i].pathNodes[j].worldPosition, rotation, HandleUtility.GetHandleSize(trackPieces[i].pathNodes[j].worldPosition)*0.8f);
////					}
//				}
//			}
//		}
//	}
//
//	void ConnectToNeighbors() {
//		TrackPiece[] trackPieces = GameObject.FindObjectsOfType<TrackPiece> ();
//		
//		if (trackPieces.Length < 2) {
//			return;
//		}
//		
//		// Get close enough nodes
//		List<PathNode> close = new List<PathNode> ();
//		List<int> closeToSelectedIdx = new List<int> ();
//		List<PathNode> selected = Selection.activeGameObject.GetComponent<TrackPiece>().pathNodes;
//		for(int i = 0; i < selected.Count; i++) {
//			if(!selected[i].isConnection || selected[i].connected)
//				continue;
//
//			for (int j = 0; j < trackPieces.Length; j++) {
//				if (selected == trackPieces [j].pathNodes)
//					continue;
//
//				List<PathNode> pathNodes = trackPieces[j].pathNodes;
//				for (int k = 0; k < pathNodes.Count; k++) {
//					if(!pathNodes[k].isConnection || trackPieces[j].pathNodes[k].connected)
//						continue;
//
//					float dist = Vector3.Distance(selected [i].worldPosition, pathNodes[k].worldPosition);
//					if(dist < SNAP_DISTANCE) {
//						close.Add(pathNodes[k]);
//						closeToSelectedIdx.Add(i);
//					}
//				}
//			}
//		}
//
//		if (close.Count < 1)
//			return;
//
//		Debug.Log ("Close: " + close.Count);
//
//		// Find closest node
//		int closest = 0;
//		for(int j = 0; j < close.Count; j++) {
//			float dist1 = Vector3.Distance(selected[closest].worldPosition, close[closest].worldPosition);
//			float dist2 = Vector3.Distance(selected[j].worldPosition, close[j].worldPosition);
//			if(dist1 > dist2) {
//				closest = j;
//			}
//		}
//
//		// Connect to closest
//		//TODO snap to
//		PathNode selectedPath = selected[closeToSelectedIdx [closest]];
//
//		close [closest].nextPaths.Add(selectedPath);
//		selectedPath.prevPaths.Add (close [closest]);
//
//		close [closest].connected = true;
//		selectedPath.connected = true;
//
//		//Connect to remaining nodes
//		close.RemoveAt (closest);
//		closeToSelectedIdx.RemoveAt (closest);
//		for(int i = 0; i < close.Count; i++) {
//			if(Vector3.Distance(close[i].worldPosition, selected[closeToSelectedIdx[i]].worldPosition) < 1.9f) {
//				close[i].nextPaths.Add (selected[closeToSelectedIdx[i]]);
//				selected[closeToSelectedIdx[i]].prevPaths.Add (close[i]);
//				
//				close[i].connected = true;
//				selected[closeToSelectedIdx[i]].connected = true;
//			}
//		}
//	}
//
//	void BreakConnections() {
//		List<PathNode> connections = Selection.activeGameObject.GetComponent<TrackPiece> ().pathNodes;
//
//		for(int i = 0; i < connections.Count; i++) {
//			if(!connections[i].connected || !connections[i].isConnection)
//				continue;
//
//			List<PathNode> nextPaths = connections[i].nextPaths;
//			for(int j = 0; j < nextPaths.Count; j++) {
//				if(nextPaths[j].isConnection) {
//					connections[i].connected = false;
//					nextPaths[j].connected = false;
//
//					nextPaths[j].prevPaths.Remove(connections[i]); // Remove from next's prev list
//					nextPaths.RemoveAt(j);
//				}
//			}
//
//			List<PathNode> prevPaths = connections[i].prevPaths;
//			for(int j = 0; j < prevPaths.Count; j++) {
//				if(prevPaths[j].isConnection) {
//					connections[i].connected = false;
//					prevPaths[j].connected = false;
//
//					prevPaths[j].nextPaths.Remove(connections[i]); // Remove from prev's next list
//					prevPaths.RemoveAt(j);
//				}
//			}
//		}
//	}
//
//	void SnapToNearest () {
//		TrackPiece[] trackPieces = GameObject.FindObjectsOfType<TrackPiece> ();
//
//		if (trackPieces.Length > 1) {
//			PathNode closest = null;
//			int closestIdx = -1;
//			List<PathNode> selectedConnections = Selection.activeGameObject.GetComponent<TrackPiece>().pathNodes;
//			int selectedIdx = -1;
//			for (int i = 0; i < selectedConnections.Count; i++) {
//				if(selectedConnections[i].isConnection && !selectedConnections[i].connected) {
//					for (int j = 0; j < trackPieces.Length; j++) {
//						if (selectedConnections != trackPieces [j].pathNodes) {
//							for (int k = 0; k < trackPieces [j].pathNodes.Count; k++) {
//								if(trackPieces[j].pathNodes[k].isConnection && !trackPieces[j].pathNodes[k].connected) {
//									if (null == closest) {
//										closest = trackPieces [j].pathNodes [k];
//										selectedIdx = i;
//										closestIdx = j;
//									} else {
//										float dist1 = Vector3.Distance (
//											selectedConnections [selectedIdx].worldPosition, 
//											closest.worldPosition
//											);
//										float dist2 = Vector3.Distance (
//											selectedConnections[i].worldPosition, 
//											trackPieces[j].pathNodes[k].worldPosition
//											);
//										if (dist1 > dist2) {
//											closest = trackPieces [j].pathNodes[k];
//											selectedIdx = i;
//											closestIdx = j;
//										}
//									}
//								}
//							}
//						}
//					}
//				}
//			}
//
//			if(null == closest) //No other connections found
//				return;
//
//			Vector3 pos1 = selectedConnections [selectedIdx].worldPosition;
//			Vector3 pos2 = closest.worldPosition;
////			Debug.Log(Vector3.Distance (pos1,	pos2));
//			if (Vector3.Distance (pos1,	pos2) < SNAP_DISTANCE ) {
//				Vector3 dir1 = Selection.activeTransform.TransformDirection(selectedConnections[selectedIdx].axis.normalized);
//				Vector3 dir2 = trackPieces[closestIdx].transform.TransformDirection(closest.axis.normalized);
//				if(Vector3.Dot(dir1, dir2) < SNAP_ANGLE) {
//					// Position
//					Selection.activeTransform.position =
//						trackPieces[closestIdx].transform.position 
//						+ (
//							Vector3.Distance(closest.worldPosition, trackPieces[closestIdx].transform.position)
//				    		+ Vector3.Distance(selectedConnections[selectedIdx].worldPosition, Selection.activeTransform.position
//		                   )
//						) * trackPieces[closestIdx].transform.TransformDirection(closest.axis.normalized);
//
//					// Rotate
//					float angle = Vector3.Angle(Selection.activeTransform.forward, Selection.activeTransform.TransformDirection(selectedConnections[selectedIdx].axis.normalized));
////					Debug.Log("fwd: " + Selection.activeTransform.forward + " axis: " + Selection.activeTransform.TransformDirection(selectedConnections[selectedConnectionIdx].axis.normalized) + " angle: " + angle);
//					Vector3 axis = Vector3.Cross(Selection.activeTransform.TransformDirection(selectedConnections[selectedIdx].axis.normalized), Selection.activeTransform.forward);
//
//					Selection.activeTransform.LookAt(closest.worldPosition);
//
//					Selection.activeTransform.Rotate(axis, angle);
////					Quaternion fromTo = Quaternion.FromToRotation(Selection.activeTransform.forward, Selection.activeTransform.TransformDirection(selectedConnections[selectedConnectionIdx].axis.normalized));
////					Selection.activeTransform.Rotate(fromTo.eulerAngles);
//				}
//			}
//		}
//	}
//
//	public override void OnInspectorGUI() {
////		DrawDefaultInspector ();
//
//		serializedObject.Update ();
//
//		TrackPiece trackPiece = Selection.activeGameObject.GetComponent<TrackPiece>();
//
//		SerializedProperty connections = serializedObject.FindProperty ("pathNodes");
//		if(connections.arraySize < 1) {
//			if (GUILayout.Button ("New Connection", GUILayout.MaxWidth(140f))) {
//				InsertNode(-1, true);
//			} 
//		}
//		EditorGUILayout.Separator ();
//		GUIContent label = new GUIContent ("Path Nodes");
//		EditorGUILayout.PropertyField(connections, label);
//		if (connections.isExpanded) {
//			for (int i = 0; i < connections.arraySize; i++) {
//				PathNode[] pathChildren = trackPiece.GetComponentsInChildren<PathNode>();
//				if(i > pathChildren.Length - 1)
//					return;
//				SerializedObject connection = new SerializedObject(pathChildren[i]);
//
//				GUILayout.BeginHorizontal();
//				if(GUILayout.Button("-", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false))) {
//					SerializedProperty nextPaths = connection.FindProperty("nextPaths");
//
//					if(nextPaths.arraySize < 1) {
//						bool deleteNode = false;
//
//						if(connections.arraySize == 1) { // Might be bad design? Might just be edge case. anyway, TODO fix
////							PathNode pathRef = pathChildren[i];
//							DestroyImmediate(pathChildren[i].gameObject);
//							trackPiece.pathNodes.Remove(pathChildren[i]);
//						} else {
//							for(int j = 0; j < connections.arraySize; j++) {
//								if(j == i)
//									continue;
//
//								SerializedObject otherNode = new SerializedObject(pathChildren[j]);
//								SerializedProperty otherNodeNext = otherNode.FindProperty("nextPaths");
//								for(int k = 0; k < otherNodeNext.arraySize; k++) {
//									if(otherNodeNext.GetArrayElementAtIndex(k).objectReferenceValue == connection.targetObject) {
//										if(otherNodeNext.GetArrayElementAtIndex(k).objectReferenceValue != null) // Serialized arrays SUCK
//											otherNodeNext.GetArrayElementAtIndex(k).objectReferenceValue = null;
//										otherNodeNext.DeleteArrayElementAtIndex(k);
//										otherNodeNext.serializedObject.ApplyModifiedProperties();
//										k--;
//										deleteNode = true;
//									}
//								}
//							}
//
//							if(deleteNode) {
////								PathNode pathRef = pathChildren[i];
//								trackPiece.pathNodes.Remove(pathChildren[i]);
//								DestroyImmediate(pathChildren[i].gameObject);
//							}
//						}
//					} else {
//						Debug.Log("Must delete dependent nodes");
////						connections.DeleteArrayElementAtIndex(i);
//					}
//				} else {
//					EditorGUI.indentLevel++;
//
////					SerializedObject connection = new SerializedObject(trackPiece.transform.GetChild(i).GetComponent<PathNode>());
//					if(connection == null) {			// Debug
//						Debug.Log("null connection");	//
//						GUILayout.EndHorizontal();		//
//						return;							//
//					}									// /Debug
//
//					SerializedProperty position = connection.FindProperty("localPosition");
//					if(position == null) {				// Debug
//						Debug.Log("null position");		//
//						GUILayout.EndHorizontal();		//
//						return;							//
//					}									// /Debug
//
//					EditorGUILayout.LabelField("Position", GUILayout.MaxWidth(70f));
//					position.vector3Value = EditorGUILayout.Vector3Field("", position.vector3Value, GUILayout.MaxWidth(340f));
//					GUILayout.EndHorizontal();
//
//					SerializedProperty isConnection = connection.FindProperty("isConnection");
//					if(isConnection.boolValue) {
//						GUILayout.BeginHorizontal();
//						EditorGUI.indentLevel++;
//						SerializedProperty axis = connection.FindProperty("axis");
//						GUILayout.Space(5f);
//						EditorGUILayout.LabelField("Axis", GUILayout.MaxWidth(70f));
//						axis.vector3Value = EditorGUILayout.Vector3Field("", axis.vector3Value, GUILayout.MaxWidth(355f));
//						EditorGUI.indentLevel--;
//						GUILayout.EndHorizontal();
//					}
//
//					GUILayout.BeginHorizontal();
//					if (GUILayout.Button ("New Connection", GUILayout.MaxWidth(140f))) {
//						InsertNode(i, true);
//					} 
//					if (GUILayout.Button ("New Node", GUILayout.MaxWidth(140f))) {
//						InsertNode(i, false);
//					}
//
//					connection.ApplyModifiedProperties();
//
//					GUILayout.EndHorizontal();
//
//					EditorGUI.indentLevel--;
//					EditorGUILayout.Separator();
//					EditorGUILayout.Separator();
//				}
//			}
//		}
//
//		serializedObject.ApplyModifiedProperties ();
//
//		Repaint ();
//	}
//
//	void InsertNode(int index, bool isConnection) {
//		GameObject trackPiece = Selection.activeGameObject;
//
//		PathNode newNode = EditorUtility.CreateGameObjectWithHideFlags("Node", HideFlags.None, typeof(PathNode)).GetComponent<PathNode>();
//		newNode.isConnection = isConnection;
//		newNode.prevPaths = new List<PathNode>();
//		newNode.nextPaths = new List<PathNode>();
//		trackPiece.GetComponent<TrackPiece> ().pathNodes.Add(newNode);
//		newNode.transform.parent = trackPiece.transform;
//
//		if(index < 0) {
//			newNode.localPosition = Vector3.up * 0.2f;
//		} else {
//			PathNode lastNode = trackPiece.GetComponent<TrackPiece> ().pathNodes [index];
//
//			newNode.localPosition = lastNode.localPosition + Vector3.up * 0.2f;
//
//			newNode.prevPaths.Add (lastNode);	
//			lastNode.nextPaths.Add (newNode);
//		}
//	}
//}
