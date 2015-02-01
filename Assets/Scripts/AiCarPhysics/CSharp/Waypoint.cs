using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	public static Waypoint start;
	
	public Waypoint next;
	
	public bool isStart = false;
	
	Vector3 CalculateTargetPosition ( Vector3 position  ){
		if (Vector3.Distance (transform.position, position) < 6) {		
			return next.transform.position;
		} else {
			return transform.position;
		}
	}
		
	void  Awake () {
		if (!next)
			Debug.Log ("This waypoint is not connected,you need to set the next waypoint!", this);
		if (isStart)
			start = this;
	}

	void  OnDrawGizmos (){
		Gizmos.color = new Color(1f, 0f, 0f, .3f);
		Gizmos.DrawCube (transform.position, new Vector3 (5,5,5));
		
		if (next) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, next.transform.
			                 position);
		}
	}
}