using UnityEngine;
using System.Collections;

public class DumbCar : MonoBehaviour {
	public float carSpeed = 3f;
	public float rotationSpeed = 15f;
	public float forceAmount = 10f;


	public float RaycastDistance = 10f;
	public RaycastHit hit;
	public Vector3 RaycastDirection;

	public Vector3 lastGoodPosition;

	//Going to try to fix the cameras rotation
	public GameObject leftFrontPoint;
	public GameObject leftRearPoint;
	public GameObject rightFrontPoint;
	public GameObject rightRearPoint;
	
	public RaycastHit rightFrontHit;
	public RaycastHit leftFrontHit;
	public RaycastHit rightRearHit;
	public RaycastHit leftRearHit;

	public float rotationStep = 100f;
	public float rotationThreshhold = 1f;

	public RaycastHit lastFrontHit;
	public RaycastHit lastRearHit;

	public RaycastHit otherHit;
	float timer = 0f;
	float heightTimer = 0f;
	
	float distanceFromGround;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//ghetto local gravity ;)
		//rigidbody.AddRelativeForce(Vector3.down * forceAmount);
		//StickToTrackPhysics();
		//StayPerpendicular();
		//StickToTrackTranslate();
		//StickToTrackLerp();

		//Local Downward Raycast From Car
		//RaycastDirection = transform.down;


		DumbControls();
		perpendicularness();
		StickToTrackLerp();
		
		
	}

	void DumbControls(){
		if(Input.GetKey("w")){
			transform.Translate(new Vector3(0,0,Input.GetAxis("Vertical"))*carSpeed*Time.deltaTime);
		}
		if(Input.GetKey("s")){
			transform.Translate(new Vector3(0,0,Input.GetAxis("Vertical"))*carSpeed*Time.deltaTime);
		}
		if(Input.GetKey("d")){
			transform.Rotate(transform.up * rotationSpeed * Time.deltaTime);
		}
		if(Input.GetKey("a")){
			transform.Rotate(-transform.up * rotationSpeed * Time.deltaTime);
		}
	}

	void StickToTrackPhysics(){
		Physics.Raycast(transform.position, RaycastDirection*RaycastDistance, out hit);
		//if mroe than 2 meters from ground, stop moving up in local y direction
		if(hit.distance > 1f){
			rigidbody.AddRelativeForce(Vector3.down * forceAmount*10);
		}
		if(hit.distance <1f && hit.distance > .1f){
			rigidbody.AddRelativeForce(-Vector3.down * forceAmount*10);
		}
	}

	void StickToTrackTranslate(){
		Physics.Raycast(transform.position, -transform.up*RaycastDistance, out hit);
		//if mroe than 2 meters from ground, stop moving up in local y direction
		if(hit.distance > 2f){
			lastGoodPosition = transform.position;
			transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, 50f*Time.deltaTime);
		}
		else if(hit.distance < 0.2f){
			//transform.Translate(new Vector3(0, lastGoodPosition.y, 0));
			Debug.Log("Bad News Bears :(");
		}
//		if(hit.distance <1f && hit.distance > .1f){
//			rigidbody.AddRelativeForce(-Vector3.down * forceAmount*10);
//		}
	}

	void StickToTrackLerp(){
		heightTimer += Time.deltaTime;
		if(Physics.Raycast(transform.position, -transform.up*RaycastDistance, out hit)){
			if(Vector3.Distance(transform.position, hit.transform.position) > 2f || Vector3.Distance(transform.position, hit.transform.position) < 2f){
				Vector3 targetPosition = hit.normal;
				targetPosition = targetPosition + transform.position;
				
				
				//print (Vector3.Distance(transform.position, targetPosition));
				if(heightTimer < 1f){
					heightTimer = Mathf.Clamp(heightTimer, 0f,1f);
					transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, heightTimer);
					
				}
				else if(heightTimer > 1f){
					heightTimer = 0f;
				}
			}
//			else if( Vector3.Distance(transform.position, hit.transform.position) < 1f){
//				Vector3 targetPosition = new Vector3(transform.localPosition.x, hit.transform.localPosition.y+2f, transform.localPosition.z);
//				
//				print (Vector3.Distance(transform.position, targetPosition));
//				if(heightTimer < 1f){
//					heightTimer = Mathf.Clamp(heightTimer, 0f,1f);
//					transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, heightTimer);
//					
//				}
//				else{
//					heightTimer = 0f;
//				}
//			}

		}

		//distanceFromGround = Vector3.Distance(transform.position, hit.transform.position);


			

	}

	void StayPerpendicular(){
		Physics.Raycast(leftFrontPoint.transform.position, -leftFrontPoint.transform.up*10f, out leftFrontHit);
		Debug.DrawRay(leftFrontPoint.transform.position, -leftFrontPoint.transform.up*10f, Color.green);

		Physics.Raycast(leftRearPoint.transform.position, -leftRearPoint.transform.up*10f, out leftRearHit);
		Debug.DrawRay(leftRearPoint.transform.position, -leftRearPoint.transform.up*10f, Color.green);

		Physics.Raycast(rightFrontPoint.transform.position, -rightFrontPoint.transform.up*10f, out rightFrontHit);
		Debug.DrawRay(rightFrontPoint.transform.position, -rightFrontPoint.transform.up*10f, Color.green);

		Physics.Raycast(rightRearPoint.transform.position, -rightFrontPoint.transform.up*10f, out rightRearHit);
		Debug.DrawRay(rightRearPoint.transform.position, -rightRearPoint.transform.up*10f, Color.green);
		//		if(lastFrontHit){
//			lastFrontHit = frontHit;
//		}
//		if(lastRearHit){
//			lastRearHit = rearHit;
//		}

		//RotateAroundLocalX();

		//RotateAroundLocalZ();


		
	}

	void RotateAroundLocalZ(){
//		//balances front side
//		if(leftFrontHit.distance - rightFrontHit.distance >= rotationThreshhold){
//			if(leftFrontHit.distance < rightFrontHit.distance){
//				transform.Rotate(-transform.up*Time.deltaTime*100f);
//			}
//			if(leftFrontHit.distance > rightFrontHit.distance){
//				transform.Rotate(transform.up*Time.deltaTime*100f);
//			}
//		}
//		//balances rear side
//		if(leftRearHit.distance - rightRearHit.distance >= rotationThreshhold){
//			if(leftRearHit.distance < rightRearHit.distance){
//				transform.Rotate(-transform.up*Time.deltaTime*100f);
//			}
//			if(leftRearHit.distance > rightRearHit.distance){
//				transform.Rotate(transform.up*Time.deltaTime*100f);
//			}
//		}
		Vector3 temp = transform.rotation.eulerAngles;
		Debug.Log(leftFrontHit.distance - rightFrontHit.distance);
		if(leftFrontHit.distance - rightFrontHit.distance > 0.01f){
			Debug.Log("Should be spinning counterclockwise");
			temp += new Vector3(0,0,rotationStep);
			if(temp.z < 0f){
				//temp.z += 360f;
				Debug.Log("Woops" + temp);
			}

			transform.Rotate(-temp*Time.deltaTime);
		}
		else if(rightFrontHit.distance - leftFrontHit.distance > 0.01f){
			Debug.Log("Should be clockwise");
			temp -= new Vector3(0,0,rotationStep);
			if(temp.z >= 360){
				//temp.z = temp.z - 360f;
			}
			transform.Rotate(temp*Time.deltaTime);
		}
	}
	void RotateAroundLocalX(){
//		//Raises front left side
//		if(leftFrontHit.distance - leftRearHit.distance >= rotationThreshhold){
//			//left front is closer to ground that rear. must subtract from local X rotation
//			if(leftFrontHit.distance < leftRearHit.distance){
//				transform.Rotate(-transform.right*Time.deltaTime*100f);
//			}
//			if(leftFrontHit.distance > leftRearHit.distance){
//				transform.Rotate(transform.right*Time.deltaTime*100f);
//			}
//		}
//		
//		//raises front right side
//		if(rightFrontHit.distance - rightRearHit.distance >= rotationThreshhold){
//			//rightfront is closer to ground that righrear...""
//			if(rightFrontHit.distance < rightRearHit.distance){
//				transform.Rotate(-transform.right*Time.deltaTime*100f);
//			}
//		}

		if(leftFrontHit.distance < rightFrontHit.distance){
			transform.Rotate(transform.right * rotationStep * Time.deltaTime);
		}
		if(leftFrontHit.distance > rightFrontHit.distance){
			transform.Rotate(-transform.right * rotationStep * Time.deltaTime);
		}

	}

	void perpendicularness(){
		Debug.DrawRay(transform.position, -transform.up*RaycastDistance, Color.green);
		Physics.Raycast(transform.position, -transform.up*RaycastDistance, out otherHit);

		Quaternion targetRotation = Quaternion.FromToRotation(transform.up, otherHit.normal);

		targetRotation.y = transform.rotation.y;
		//print(targetRotation.eulerAngles);

		timer += Time.deltaTime*2;
		if(timer < 1f) {
			//print(timer);
			timer = Mathf.Clamp(timer, 0f, 1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timer);
			timer = 0f;
		}

	}
}