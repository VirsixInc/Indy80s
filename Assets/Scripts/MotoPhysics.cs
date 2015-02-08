using UnityEngine;
using System.Collections;

public class MotoPhysics : MonoBehaviour {

	Rigidbody thisRigidbody;
	public float floatHeight;
	public float hoverForce;
	public RaycastHit frontHit, backHit; //overwritten every frame
	public float forwardThrust;
	public float turnStrength;
	public float aclAxis;
	public float turnAxis;
	public GameObject front, back;
	public float bufferZone;
	public float rollScalar;
	public bool invertTurning;
	public bool invertAcl;
	public float speedBoostDecrement = 100f;
	public float bumpStrength = 1000;

	void Start () {
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
    Hover ();
	}

	void Hover () {

		//if the front thruster, ray from position    , in direction, output data to, length of raycast, is hitting
		if (Physics.Raycast (front.transform.position, -Vector3.up, out frontHit, 1000f)) {
			if (frontHit.distance <= floatHeight && frontHit.collider.gameObject.tag != "WayPoint") {
				thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (frontHit.distance / floatHeight)), front.transform.position); //push up on the car if it is close to track
			}
			else {
				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce, front.transform.position); //push down on the car if it is in the air
//				if (transform.rotation.x >= 0) {
//					thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce, front.transform.position); //fixes offset in the air
//				}
//				else if (transform.rotation.x <= 0) {
//					thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce, back.transform.position);
//				}
			}
		}

		if (Physics.Raycast (back.transform.position, -Vector3.up, out backHit, 1000f)) {
			if (backHit.distance <= floatHeight) {
				thisRigidbody.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (backHit.distance / floatHeight)), back.transform.position); //push up on the car if it is close to track
			}
			else {
				thisRigidbody.AddForceAtPosition(-Vector3.up * hoverForce, back.transform.position); //push down on the car if it is in the air
			}
		}
	}

	void ForwardThrust(){
		thisRigidbody.AddForce(transform.forward * forwardThrust * aclAxis);
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

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			ContactPoint contact = collision.contacts[0];
			//Instantiate(spark, contact.point, Quaternion.identity);
			Vector3 pos = contact.point;
			Vector3 collisionPointAtCarHeight= new Vector3(pos.x, transform.position.y, pos.z);
			Vector3 directionOfBumpForce = transform.position - collisionPointAtCarHeight;
			Debug.DrawRay (pos, directionOfBumpForce, Color.magenta);
			transform.rigidbody.AddForceAtPosition (bumpStrength * directionOfBumpForce, collisionPointAtCarHeight);
			//spawn a particle effect at the collision point and transform height
		}
	}

	IEnumerator SpeedBoost () {
		float currentThrust = forwardThrust;
		float startTime = Time.time;
		forwardThrust *= 2;

		while (forwardThrust > currentThrust) {
			yield return new WaitForEndOfFrame();
			forwardThrust -= speedBoostDecrement;
		}
	}
}
