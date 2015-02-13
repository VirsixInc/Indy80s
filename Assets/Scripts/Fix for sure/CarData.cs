using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarData : MonoBehaviour {
	
	//CarData is attached to each car gameobject	
	public int lap = 1;
	[HideInInspector]
	public int playerNumber;
	[HideInInspector]
	public int currentPlace;
	bool hasGoneHalfWay;
	GameObject lastWayPoint;	//lastWayPoint is used to calculate place and spawnpoint

	public bool invertWheel = false;
//	public bool invertPedal = false;
	public AudioSource engine;
//	float enginePitchRatio = .01f;
	MotoPhysics carController;
	CarAnimationController myCarAnimationController;
//	public float distanceFromLastWayPoint;
	PlayerGUI gui;
	
	void Start() {
//		engine = GameObject.Find("CarRaceSound" + playerNumber.ToString()).GetComponent<AudioSource>(); //FIXME

		carController = gameObject.GetComponent<MotoPhysics>();
		myCarAnimationController = gameObject.GetComponentInChildren<CarAnimationController>();

//		lastWayPoint = PathNode.first; first cannot be isStart with the way PathNode is currently set up - we could make it this way later
//		StartCoroutine ("CheckRespawn");

		gui = transform.FindChild("GUI").GetComponent<PlayerGUI>();

		lastWayPoint = PathNode.first.gameObject;
	}
	
	//SetCarCurrentPlace, called from PlayerManager
	public void SetCarCurrentPlace(int newPlace) {
		currentPlace = newPlace;
		gui.UpdatePlace(newPlace);
	}
	
	public void SetLastWayPoint(GameObject newPathNode) {
		lastWayPoint = newPathNode;
	}

	public void UpdateInput(float newPedalIntensity, float newWheelIntensity) {
		if (carController != null) {
			carController.turnAxis = newWheelIntensity;
			carController.aclAxis = (1 - newPedalIntensity);
			myCarAnimationController.wheelRotation = (invertWheel ? -1f : 1f) * newWheelIntensity;
			myCarAnimationController.pedalValue = (1 - newPedalIntensity);
			UpdateSound(newPedalIntensity);
		}
	}
	
	//When car hits a new waypoint, set it to lastWaypoint hit.
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "InstaKill" && isRespawning == false) {
			StartCoroutine("Explode");
		}    
		if (other.gameObject.tag == "WayPoint") {
			lastWayPoint = other.gameObject;
			if (other.GetComponent<PathNode>().ReturnDistanceToEnd() == 0) {
				if (lap == PlayerManager.s_instance.lapsToWin) {
//					PlayerManager.s_instance.Win(id); //FIXME
				} else if (hasGoneHalfWay) {
					lap++;
					gui.UpdateLap(lap);
					hasGoneHalfWay = false;
				}
				
			} else if (other.GetComponent<PathNode>().IsHalfWay()) {
				hasGoneHalfWay = true;
			}
		}
	}
	
	public float CalculateDistanceFromLastWayPoint() {
		//we may want to take the negative value of this to invert order
//		float distanceFromLastWayPoint = Vector3.Distance(lastWayPoint.transform.position, transform.position); //for debug
		return Vector3.Distance(lastWayPoint.transform.position, transform.position);
	}

	void UpdateSound(float pedal) {
		engine.pitch = 1f + (1 - pedal);
	}

	//RESPAWN.CS

	float explosionTime = 0;//, heightAboveTrackForRespawn = 100f;
	public GameObject explosionPrefab;
	Transform spawnPosition;
	//	GameObject tempExplosion;
	bool isRespawning = false;

	public void TriggerExplode() {
		StartCoroutine("Explode");
	}

	IEnumerator Explode() {
		carController.canMove = false;
		isRespawning = true;
		print("Explode");
		Instantiate(explosionPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);

		rigidbody.velocity = new Vector3(0, 0, 0);

		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
			rend.enabled = false;

		yield return new WaitForSeconds(1);

		Transform spawnPosition = gameObject.GetComponent<CarData>().lastWayPoint.transform;
		transform.position = new Vector3(spawnPosition.position.x, spawnPosition.position.y + 20, spawnPosition.position.z);
		transform.rotation = spawnPosition.rotation;
		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
			rend.enabled = true;

		isRespawning = false;
		gameObject.collider.enabled = true;

		yield return new WaitForSeconds(1);

		carController.Reset();
		carController.canMove = true;
	}
	
	void Update() {
		float zRotation = gameObject.transform.rotation.eulerAngles.z;
		if ((zRotation > 50f && zRotation < 140f) || (zRotation > 220 && zRotation < 310f)) {
			if (isRespawning == false)
				StartCoroutine("Explode");		
		} 
	}
}


