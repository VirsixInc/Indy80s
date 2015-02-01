using UnityEngine;
using System.Collections;

public class PlayerCar : MonoBehaviour {
	
	// These variables allow the script to power the wheels of the car.
	public WheelCollider frontLeftWheel;
	public WheelCollider frontRightWheel;

	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public float[] gearRatio;
	public int currentGear = 0;

	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	public float engineTorque = 230.0f;
	public float maxEngineRPM = 3000.0f;
	public float minEngineRPM = 1000.0f;
	private float engineRPM = 0.0f;
	
	void Start () {
		// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
		rigidbody.centerOfMass += new Vector3(0f, -1f, .25f);
	}
	
	void  Update (){
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		engineRPM = (frontLeftWheel.rpm + frontRightWheel.rpm)/2 * gearRatio[currentGear];
		ShiftGears();

//		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
//		// up to twice it's pitch, where it will suddenly drop when it switches gears.
//		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;
//		if ( audio.pitch > 2.0f ) {
//			audio.pitch = 2.0f;
//			
//		}

		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the user input variable.
		frontLeftWheel.motorTorque = engineTorque / gearRatio[currentGear] * Input.GetAxis("Vertical");
		frontRightWheel.motorTorque = engineTorque / gearRatio[currentGear] * Input.GetAxis("Vertical");

		// the steer angle is an arbitrary value multiplied by the user input.
		frontLeftWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
		frontRightWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
		
	}
	
	void  ShiftGears (){
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		int AppropriateGear = 0;
		if (engineRPM >= maxEngineRPM) {
			AppropriateGear = currentGear;
				
			for (int i= 0; i < gearRatio.Length; i ++ ) {
				if (frontLeftWheel.rpm * gearRatio[i] < maxEngineRPM) {
					AppropriateGear = i;
					break;
				}
			}
			currentGear = AppropriateGear;
		}
		
		if (engineRPM <= minEngineRPM ) {
			AppropriateGear = currentGear;
			
			for (int j= gearRatio.Length-1; j >= 0; j--) {
				if (frontLeftWheel.rpm * gearRatio[j] > minEngineRPM) {
					AppropriateGear = j;
					break;
				}
			}
			currentGear = AppropriateGear;
		}
	}
}