//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//#if UNITY_EDITOR
//[ExecuteInEditMode]
//#endif
//
//public class PathNodeOld : MonoBehaviour {
//
//	/*
//	This class is used to create waypoints that can determine which car is in 1st, 2nd, or 3rd place etc...
//	the distance to end is determined by going around the track backwards and sequentially adding up the distance to end
//	of linked waypoints. By checking isStart we are able to begin at the finish line and move through the entire track
//	calculating the distance to end for each waypoint
//
//
//	NOTE: Path must be built in opposite direction that the cars drive, or AI waypoints are set
//	*/
//
//#if UNITY_EDITOR
//	Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue}; 
//#endif
//	public static PathNodeOld first;
//
//	public List<PathNodeOld> nextPaths;
//	public List<PathNodeOld> lastPaths;
//
//	public float distanceToEnd = 0;
//	public bool isStart = false; //cars should start in front of the isStart PathNode in order for the placing to work properly
//
//	public bool halfWayPoint = false;
//	public bool isFirst = false; //this should be the node with the greatest distance to end - the first node the players hit
//
//	
//	void Awake () {
//		if(null == nextPaths)
//			nextPaths = new List<PathNodeOld>();
//		if (isStart) {
//			first = this;
////			CalculateDistanceToEnd (0f);
//			PathNodeOld[] nodes = FindObjectsOfType<PathNodeOld>();
//			foreach(PathNodeOld node in nodes) {
//				node.CalculateDistanceToEnd (0f);
//			}
//		}
//
//		if (isFirst) {
//			foreach (CarData car in PlayerManager.s_instance.cars)
//				car.lastWayPoint = gameObject.GetComponent<PathNode>(); //sets cars initial waypoint to Start waypoint so that they have 
//		}
//	}
//
//
////	public void CalculateDistanceToEnd(float totalDistance) {
////		distanceToEnd = 0f;
////		PathNode curNode = this;
//////		while(curNode.nextPaths[0] != null || curNode.isStart == true) {
//////			if(curNode.isStart)
//////				Debug.Break();
//////			distanceToEnd += Vector3.Distance (gameObject.transform.position, nextPaths[0].transform.position);
//////			curNode = curNode.nextPaths[0];
//////		}
////
////
//////		for(int i = 0; i < nextPaths.Count; i++) {
//////			if(nextPaths[i].GetComponent<PathNode>().isStart == false) {
//////				distanceToEnd = totalDistance + Vector3.Distance (gameObject.transform.position, nextPaths[i].transform.position);
//////				nextPaths[i].GetComponent<PathNode>().CalculateDistanceToEnd(distanceToEnd);
//////			}
//////		} 
////	}
//
//	public void CalculateDistanceToEnd(float totalDistance) {
//		for(int i = 0; i < nextPaths.Count; i++) {
//			if(nextPaths[i].GetComponent<PathNodeOld>().isStart == false) {
//				distanceToEnd = totalDistance + Vector3.Distance (gameObject.transform.position, nextPaths[i].transform.position);
//				nextPaths[i].GetComponent<PathNodeOld>().CalculateDistanceToEnd(distanceToEnd);
//			}
//		} 
//	}
//
//#if UNITY_EDITOR
//	void OnDrawGizmosSelected() {
//		PathNodeOld[] nodes = FindObjectsOfType<PathNodeOld> ();
//
//		for (int i = 0; i < nodes.Length; i++) {
//			Gizmos.DrawIcon(nodes[i].transform.position, "Waypoint.png", true);
//			for(int j = 0; j < nodes[i].nextPaths.Count; j++) {
//				Gizmos.color = colors[j % colors.Length];
//				Gizmos.DrawLine (nodes[i].transform.position, nodes[i].nextPaths[j].transform.position);
//			}
//		}
//	}
//#endif	
//}
