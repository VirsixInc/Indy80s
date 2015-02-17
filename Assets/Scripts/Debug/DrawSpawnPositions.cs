using UnityEngine;
using System.Collections;

public class DrawSpawnPositions : MonoBehaviour {

	#if UNITY_EDITOR

	public bool onlyOnSelected = false;

	Color[] gizmoColors = {Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.black};
	
	void OnDrawGizmosSelected() {
		if (onlyOnSelected) {
			for (int i = 0; i < transform.childCount; i++) {
				Gizmos.color = gizmoColors [i];
				Gizmos.DrawCube(transform.GetChild(i).position, Vector3.one * 3f);
				DrawArrow(transform.GetChild(i).position, transform.GetChild(i).forward, transform.GetChild(i).right, 5.5f);
				DrawArrow(transform.GetChild(i).position, transform.GetChild(i).up, transform.GetChild(i).right, 3.5f);
			}
		}
	}

	void OnDrawGizmos() {
		if (!onlyOnSelected) {
			for (int i = 0; i < transform.childCount; i++) {
				Gizmos.color = gizmoColors [i];
				Gizmos.DrawCube(transform.GetChild(i).position, Vector3.one * 3f);
				DrawArrow(transform.GetChild(i).position, transform.GetChild(i).forward, transform.GetChild(i).right, 5.5f);
				DrawArrow(transform.GetChild(i).position, transform.GetChild(i).up, transform.GetChild(i).right, 3.5f);
			}
		}
	}

	public static void DrawArrow(Vector3 pos, Vector3 forward, Vector3 right, float length) {
		Gizmos.DrawRay(pos, forward * length);
		Gizmos.DrawRay(pos + forward * length, (right - forward) * length/4f);
		Gizmos.DrawRay(pos + forward * length, (-right - forward) * length/4f);
	}
	
	#endif
}
