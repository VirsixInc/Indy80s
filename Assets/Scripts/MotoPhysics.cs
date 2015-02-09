using UnityEngine;
using System.Collections;

public class MotoPhysics : MonoBehaviour {

	Rigidbody thisRigidbody;
	public float floatHeight;
	public float hoverForce;
	public RaycastHit frontHit, backHit; //overwritten every frame
	public float forwardThrust;
	public const float initThrust = 10000;
	public const float initTurnStrength = 3000;
	public float turnStrength;
	public float aclAxis;
	public float turnAxis;
	public GameObject front, back;
	public float bufferZone;
	public float rollScalar;
	public bool invertTurning;
	public bool invertAcl;
	float speedBoostDecrement = 100f, speedBoostMultiplier = 1.5f;


	void Start () {
		forwardThrust = initThrust;
		turnStrength = initTurnStrength;
		thisRigidbody = GetComponent<Rigidbody> ();
		if (invertTurning) GetComponent<CarData> ().isInverted = -1;
		if (invertTurning)
						GetComponent<CarData> ().invertPedal = 1f;
	}
	
	void Update () {
		ForwardThrust ();
		Turning ();
	}

	void FixedUpdate() {
		if (GetComponent<CarData> ().isRespawning == false)
			Hover ();
	}

	void Hover () {

		//if the front thruster, ray from position    , in direction, output data to, length of raycast, is hitting
		if (Physics.Raycast (front.transform.position, -Vector3.up, out frontHit, 1000f)) { //FRONT
			if (frontHit.distance <= floatHeight) {
				thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (frontHit.distance / floatHeight)), front.transform.position); //push up on the car if it is close to track
			}
//			else if (transform.rotation.eulerAngles.x <= 0){ //if not hitting
//				print ("add force");
//				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce, front.transform.position); //push down on the car if it is in the air
//				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce/2, back.transform.position);
//				
//					thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce/2, front.transform.position); //extra push
//				}
				//				if (transform.rotation.eulerAngles.x >= 0) {
//					thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce, front.transform.position); //fixes offset in the air
//				}
//				else if (transform.rotation.eulerAngles.x <= 0) {
//					thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce, back.transform.position);
//				}
//			}
			else{
				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce, front.transform.position);
			}
		}

		if (Physics.Raycast (back.transform.position, -Vector3.up, out backHit, 1000f)) { //BACK
			if (backHit.distance <= floatHeight) {
				thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (backHit.distance / floatHeight)), back.transform.position); //push up on the car if it is close to track
			}
			else {
				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce, back.transform.position); //push down on the car if it is in the air
			}
		}
		transform.rotation = Quaternion.Euler(new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f)); //freeze Z axis
	}

	void ForwardThrust(){
		thisRigidbody.AddForce(new Vector3(transform.forward.x, 0f, transform.forward.z) * forwardThrust * aclAxis);
	}

	void Turning(){
		if(invertTurning){
			thisRigidbody.AddRelativeTorque(Vector3.up * turnStrength * (-turnAxis));
		}
		else{
			thisRigidbody.AddRelativeTorque(Vector3.up * turnStrength * turnAxis);
		}
	}

	void FixRoll(){
		if (transform.rotation.z > 0) {
			thisRigidbody.AddRelativeTorque (Vector3.forward * transform.rotation.z * rollScalar);		
		} 
		else if (transform.rotation.z < 0) {
			thisRigidbody.AddRelativeTorque(Vector3.forward * transform.rotation.z * rollScalar);		
		}
	}

	IEnumerator SpeedBoost () {
		float currentThrust = forwardThrust;
//		float startTime = Time.time;
		forwardThrust *= speedBoostMultiplier;

		while (forwardThrust > currentThrust) {
			yield return new WaitForEndOfFrame();
			forwardThrust -= speedBoostDecrement;
		}
	}
}
