using UnityEngine;
using System.Collections;

public class RotateInfinite : MonoBehaviour {

	public enum Axis { X, Y, Z };

	public float rotateSpeed = 3f;
	public Axis axis;

	void Update () {
		switch(axis){
		case Axis.X : transform.Rotate (rotateSpeed, 0f, 0f); break;
		case Axis.Y : transform.Rotate (0f, rotateSpeed, 0f); break;
		case Axis.Z : transform.Rotate (0f, 0f, rotateSpeed); break;
		}
	}
}
