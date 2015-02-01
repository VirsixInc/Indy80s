using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	public HingeJoint fl_Hinge, fr_Hinge;

	JointMotor fl_Motor, fr_Motor;

	public float speed = 100f;
	public float force = 100f;

	void Start () {
		fl_Hinge.useMotor = true;
		fr_Hinge.useMotor = true;

		fl_Motor = fl_Hinge.motor;
		fr_Motor = fr_Hinge.motor;

		fl_Motor.targetVelocity = 0f;
		fr_Motor.targetVelocity = 0f;

		fl_Motor.force = force;
		fr_Motor.force = force;
	}

	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			fl_Motor.targetVelocity = speed;
		} else {
			fl_Motor.targetVelocity = 0f;
		}
		
//		if (Input.GetKey (KeyCode.W)) {
//			fr_Motor.targetVelocity = -speed;
//		} else if (Input.GetKey (KeyCode.S)) {
//			fr_Motor.targetVelocity = speed;
//		} else {
//			fr_Motor.targetVelocity = 0f;
//		}

		fl_Hinge.motor = fl_Motor;
		fr_Hinge.motor = fr_Motor;

	}
}
