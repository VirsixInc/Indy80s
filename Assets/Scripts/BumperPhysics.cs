using UnityEngine;
using System.Collections;

//this class allows an object to bump into another object tagged as player
//you must place a triggered "bumper" collider on the car

public class BumperPhysics : MonoBehaviour {

	public float bumpStrength = 1000;
	//GameObject spark;

	void Start() {
		//spark = Resources.Load ("MikeAssets/SparksMaterial/Materials/Sparks/SparksParent") as GameObject;
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
}
