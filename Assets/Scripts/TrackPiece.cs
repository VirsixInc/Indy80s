//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//[ExecuteInEditMode]
//public class TrackPiece : MonoBehaviour {
//
//	public List<PathNode> pathNodes;
//
//	void Awake() {
//		if(null == pathNodes)
//			pathNodes = new List<PathNode>();
//	}
//
//#if UNITY_EDITOR
////	Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue}; 
//
//	void OnDrawGizmosSelected() {		
//		TrackPiece[] trackPieces = GameObject.FindObjectsOfType<TrackPiece>();
////		foreach(TrackPiece trackPiece in trackPieces) {
////			trackPiece.DrawGizmos ();
//		}
//	}
//
////	public void DrawGizmos() {
////		for (int i = 0; i < pathNodes.Count; i++) {
////			if(pathNodes[i].isConnection) {
////				if(false == pathNodes[i].connected)
////					Gizmos.DrawIcon(pathNodes[i].worldPosition, "Connection.png", false);
////			} else {
////				Gizmos.DrawIcon(pathNodes[i].worldPosition, "Waypoint.png", false);
////			}
////			for(int j = 0; j < pathNodes[i].nextPaths.Count; j++) {
////				Gizmos.color = colors[j % colors.Length];
////				Gizmos.DrawLine (pathNodes[i].worldPosition, pathNodes[i].nextPaths[j].worldPosition);
//////				if(pathNodes[i].nextPaths[j].transform.parent != this.gameObject) {
//////					pathNodes[i].nextPaths[j].transform.parent.GetComponent<TrackPiece>().DrawGizmos();
//////				}
////			}
////		}
////	}
//#endif	
//}