//using UnityEngine;
//using System.Collections;
//
//public class AiCar : MonoBehaviour {
//	
//	public WheelCollider FrontLeftWheel;
//	public WheelCollider FrontRightWheel;
//	public WheelCollider RearLeftWheel;
//	public WheelCollider RearRightWheel;
//	
//	public float vehicleCenterOfMass = 0.0f;
//	public float steeringSharpness = 12.0f;
//
//	// These variables are for the gears, the array is the list of ratios. The script
//	// uses the defined gear ratios to determine how much torque to apply to the wheels.
//	public float[] GearRatio;
//	public int CurrentGear = 0;
//
//	// These variables are just for applying torque to the wheels and shifting gears.
//	// using the defined Max and Min Engine RPM, the script can determine what gear the
//	// car needs to be in.
//	public float EngineTorque = 600.0f;
//	public float BrakePower = 0;
//	public float MaxEngineRPM = 3000.0f;
//	public float MinEngineRPM = 1000.0f;
//	private float EngineRPM = 0.0f;
//
//	// Here's all the variables for the AI, the waypoints are determined in the "GetWaypoints" function.
//	// the waypoint container is used to search for all the waypoints in the scene, and the current
//	// waypoint is used to determine which waypoint in the array the car is aiming for.
////	public GameObject waypointContainer;
////	private Transform[] waypoints;
////	PathNode currentWaypoint;
//
//	// input steer and input torque are the values substituted out for the player input. The 
//	// "NavigateTowardsWaypoint" function determines values to use for these variables to move the car
//	// in the desired direction.
//	private float inputSteer = 0.0f;
//	private float inputTorque = 0.0f;
//
//	CarData carData;
//
//	void Start () {
//		Vector3 com = rigidbody.centerOfMass;
//		com.y = (vehicleCenterOfMass);
//		rigidbody.centerOfMass = com;
//
//		carData = GetComponent<CarData> ();
////		GetWaypoints();
//	}
//	
//	void  FixedUpdate (){
//		// This is to limit the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
//		// but it's easy, and it doesn't interfere with the physics processing.
//		rigidbody.drag = rigidbody.velocity.magnitude / 250;
//
//		// Call the funtion to determine the desired input values for the car. This essentially steers and
//		// applies gas to the engine.
//		NavigateTowardsWaypoint();
//
//		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
//		EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
//		ShiftGears();
//
//		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
//		// up to twice it's pitch, where it wi
////		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;
////		if ( audio.pitch > 2.0f ) {
////			audio.pitch = 2.0f;
////		}
//
//		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
//		// multiplied by the calculated AI input variable.
//		FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;
//		FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;
//
//		RearLeftWheel.brakeTorque = BrakePower;
//		RearRightWheel.brakeTorque = BrakePower;
//
//		// the steer angle is an arbitrary value multiplied by the calculated AI input.
//		FrontLeftWheel.steerAngle = (steeringSharpness) * inputSteer;
//		FrontRightWheel.steerAngle = (steeringSharpness) * inputSteer;
//	}
//	
//	void ShiftGears () {
//		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
//		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
//		int AppropriateGear = CurrentGear;
//
//		if (EngineRPM >= MaxEngineRPM) {
//			for (int i= 0; i < GearRatio.Length; i++) {              
//					if (FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM) {
//						AppropriateGear = i;
//						break;
//				}
//				CurrentGear = AppropriateGear;
//			}
//		}
//
//		if (EngineRPM <= MinEngineRPM ) {
//			AppropriateGear = CurrentGear;
//			
//			for ( var j = GearRatio.Length-1; j >= 0; j -- ) {
//				if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
//					AppropriateGear = j;
//					break;
//				}
//			}
//			
//			CurrentGear = AppropriateGear;
//		}
//	}
//
////	void GetWaypoints () {
////		// Now, this function basically takes the container object for the waypoints, then finds all of the transforms in it,
////		// once it has the transforms, it checks to make sure it's not the container, and adds them to the array of waypoints.
////		Transform[] potentialWaypoints = waypointContainer.GetComponentsInChildren<Transform>();
////			
////		foreach(Transform potentialWaypoint in potentialWaypoints) {
////			if (potentialWaypoint != waypointContainer.transform) {
////				waypoints[waypoints.Length] = potentialWaypoint;	
////			}
////		}
////	}
//		
//	void  NavigateTowardsWaypoint () {
//		// now we just find the relative position of the waypoint from the car transform,
//		// that way we can determine how far to the left and right the waypoint is.
//
//		Vector3 targetPos = carData.lastWayPoint.GetComponent<PathNodeOld>().nextPaths [0].transform.position;
//
//		Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(
//			new Vector3( 
//	            targetPos.x, 
//    			transform.position.y, 
//	            targetPos.z 
//            ) 
//		);
//
//		float angle = Vector3.Angle(transform.forward, (transform.position - RelativeWaypointPosition).normalized);
//
//		if (angle < 90f) {
//				
//			// by dividing the horizontal position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
//			inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
//
//			// now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
//			if ( Mathf.Abs( inputSteer ) < 0.5f ) {
//				inputTorque = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs( inputSteer );
//			}else{
//				inputTorque = 0.0f;
//			}
//		} else {
////			Vector3 diff = transform.forward - (transform.position - RelativeWaypointPosition);
////			diff.Normalize();
////			if(diff.x < 0) {
////				inputSteer = -1f;
////			} else {
////				inputSteer = 1f;
////			}
//			Vector3 rot = Quaternion.LookRotation(transform.position - RelativeWaypointPosition).eulerAngles;
//			if(rot.x - transform.eulerAngles.x < 0)
//				inputSteer = -1f;
//			else inputSteer = 1f;
//		}
//
//		// this just checks if the car's position is near enough to a waypoint to count as passing it, if it is, then change the target waypoint to the
//		// next in the list.
////		if ( RelativeWaypointPosition.magnitude < 20 ) {
////			currentWaypoint ++;     
////			
////			if ( currentWaypoint >= waypoints.Length ) {
////				currentWaypoint = 0;    
////			}
////		}
//
//
//
//						
//
//	}
//}