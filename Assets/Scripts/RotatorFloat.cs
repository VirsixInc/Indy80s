using UnityEngine;
using System.Collections;

public class RotatorFloat : MonoBehaviour 
{

	public enum RotationAxis
	{
		All,
		Y,
		X,
		Z,
	}

	public RotationAxis axis;
	public float speedRot = 0.3f;

	void Update ()
	{
		float rot = Time.deltaTime * speedRot;

			switch( axis) 
			{
				default:
				case RotationAxis.All:
				transform.Rotate( new Vector3( rot, rot, rot));
				break;

				case RotationAxis.X:
				transform.Rotate ( new Vector3( rot, 0f, 0f));
				break;

				case RotationAxis.Y:
				transform.Rotate ( new Vector3( 0f, rot, 0f));
				break;

				case RotationAxis.Z:
				transform.Rotate ( new Vector3( 0f, 0f, rot)); 
				break;
			}
	}
}
//	void Start () {
	
//	}
	
//	void Update () {
//		transform.Rotate (axis * speed * Time.deltaTime);
//		transform.Rotate (Vector3.forward * Time.deltaTime);
//	}


