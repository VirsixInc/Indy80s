using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCar : MonoBehaviour {

	Rigidbody body;
	float deadZone = 0.1f;
	public float forwardAcceleration = 100f;
	public float backwardAcceleration = 25f;

	float currentThrust = 0f;
	public float turnStrength = 10f;
	float currentTurn = 0f;

	int layerMask;
	public float hoverForce = 9f;
	public float hoverHeight = 2f;
	public GameObject[] hoverPoints;


	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();

		layerMask = 1 << LayerMask.NameToLayer("Characters");
		layerMask = ~layerMask;
	}
	
	// Update is called once per frame
	//Player Input
	void Update () {
		//MainThrust
		currentThrust = 0f;
		float accelerationAxis = Input.GetAxis("Vertical");
		if(accelerationAxis > deadZone){
			currentThrust = accelerationAxis * forwardAcceleration;
		}
		else if(accelerationAxis < -deadZone){
			currentThrust = accelerationAxis * backwardAcceleration;
		}
		//Turning
		currentTurn = 0f;
		float turnAxis = Input.GetAxis("Horizontal");
		if(Mathf.Abs(turnAxis) > deadZone){
			currentTurn = turnAxis;
		}
	}
	//Physics Calcualtions
	void FixedUpdate(){
		//moving forward
		if(Mathf.Abs(currentThrust) > 0){
			body.AddForce(transform.forward * currentThrust);
		}
		//turnm
		if(currentTurn > 0){
			body.AddRelativeTorque(Vector3.up * currentTurn * turnStrength);
		}
		else if(currentTurn < 0){
			body.AddRelativeTorque(Vector3.up * currentTurn * turnStrength);
		}

		//Hover Force
		RaycastHit hit;
		for(int i = 0; i < hoverPoints.Length; i++){
			var hoverPoint = hoverPoints[i];
			if(Physics.Raycast(hoverPoint.transform.position, 
			                   -Vector3.up*10f, out hit, 
			                   hoverHeight, layerMask)){
				body.AddForceAtPosition(Vector3.up 
				                        * hoverForce 
				                        * (1f - (hit.distance/hoverHeight)),
				                        hoverPoint.transform.position);
			}
			else{
				//upright
				if(transform.position.y > hoverPoint.transform.position.y){
					body.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				}
				//upsidedown
				else{
					body.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
				}
			}
		}
	}
	void OnDrawGizmos()
	{
		
		//  Hover Force
		RaycastHit hit;
		for (int i = 0; i < hoverPoints.Length; i++)
		{
			var hoverPoint = hoverPoints [i];
			if (Physics.Raycast(hoverPoint.transform.position, 
			                    -Vector3.up, out hit,
			                    hoverHeight, 
			                    layerMask))
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
				Gizmos.DrawSphere(hit.point, 0.5f);
			} else
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(hoverPoint.transform.position, 
				                hoverPoint.transform.position - Vector3.up * hoverHeight);
			}
		}
	}
}
