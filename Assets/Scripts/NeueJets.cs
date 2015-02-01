using UnityEngine;
using System.Collections;

public class NeueJets : MonoBehaviour {
	RaycastHit hit;
	public float carHeight;
	public float hoverForce;
	public float deadZone;

	Rigidbody body;
	
	CarAnimationController myAnimator;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
	}
	//TODO: make the jets not AddForceAtPoint! Stop that rotation
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){


		if(Physics.Raycast(transform.position, -transform.up, out hit, 100f)){
			print("hit");
			Vector3 temp;
			temp = hit.normal;
			Debug.DrawRay(transform.position, -transform.up*100f, Color.cyan);
			transform.up = temp;
			//transform.up = (transform.up * 15f + temp)/16f;

		


			//JETS
			if(hit.distance < carHeight){
				body.AddForce(transform.up 
				                              * hoverForce
				                              * (1.0f - (hit.distance / carHeight)), ForceMode.Acceleration);
			}
			else{
				body.AddForce(-transform.up * hoverForce 
				              *  (1.0f - (carHeight/hit.distance )), ForceMode.Acceleration);
			}

		}
	}
}
