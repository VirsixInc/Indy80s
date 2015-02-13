using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

public class PathNode : MonoBehaviour {
	
	/*
	This class is used to create waypoints that can determine which car is in 1st, 2nd, or 3rd place etc...
	the distance to end is determined by going around the track backwards and sequentially adding up the distance to end
	of linked waypoints. By checking isStart we are able to begin at the finish line and move through the entire track
	calculating the distance to end for each waypoint
	*/
//	
//	#if UNITY_EDITOR
//	Color[] colors = {Color.cyan, Color.green, Color.red, Color.yellow, Color.blue}; 
//	#endif
	public static PathNode first;
	public List<PathNode> nextPaths;
	public List<PathNode> lastPaths;
	public float distanceToEnd = 0;
	public bool isStart = false; //cars should start in front of the isStart PathNode in order for the placing to work properly
	public bool halfWayPoint = false;
	public bool isFirst = false; //this should be the node with the greatest distance to end - the first node the players hit
	
	void Awake() {
		//		if(null == paths)
		//			paths = new List<PathNode>();
		if (isStart) {
			first = this;
			CalculateDistanceToEnd(0f);
		}
//		if (isFirst) {
////			StartCoroutine("SetLastWayPoint");
//		}
	}

//	IEnumerator SetLastWayPoint() {
//		yield return new WaitForSeconds(0);
//		foreach (CarData car in PlayerManager.s_instance.cars)
//			car.lastWayPoint = gameObject; //sets cars initial waypoint to Start waypoint so that they have 
//	}
	
	// Getters
	public float ReturnDistanceToEnd() {
		return distanceToEnd;
	} //used by PlayerManager when calculating car places
	public bool IsStart() {
		return isStart;
	}

	public bool IsHalfWay() {
		return halfWayPoint;
	}
	
	public void CalculateDistanceToEnd(float totalDistance) {
		for (int i = 0; i < nextPaths.Count; i++) {
			if (nextPaths [i].GetComponent<PathNode>().IsStart() == false) {
				distanceToEnd = totalDistance + Vector3.Distance(gameObject.transform.position, nextPaths [i].transform.position);
				nextPaths [i].GetComponent<PathNode>().CalculateDistanceToEnd(distanceToEnd);
			}
		} 
	}
//	#if UNITY_EDITOR
//	void OnDrawGizmosSelected() {
//		PathNode[] nodes = FindObjectsOfType<PathNode> ();
//		
//		for (int i = 0; i < nodes.Length; i++) {
//			Gizmos.DrawIcon(nodes[i].transform.position, "Waypoint.png", true);
//			for(int j = 0; j < nodes[i].nextPaths.Count; j++) {
//				Gizmos.color = colors[j % colors.Length];
//				Gizmos.DrawLine (nodes[i].transform.position, nodes[i].nextPaths[j].transform.position);
//			}
//		}
//	}
//	#endif	
}
