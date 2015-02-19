using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarData : MonoBehaviour {
	public int lap = 1;
	[HideInInspector]
	public int playerNumber;
	[HideInInspector]
	public int currentPlace;
//	bool hasGoneHalfWay;
	[HideInInspector]
	public PathNode lastWayPoint;	//lastWayPoint is used to calculate place and spawnpoint

	public bool invertWheel = false;
//	public bool invertPedal = false;

	public AudioSource engine;

	[HideInInspector]
	public MotoPhysics carController;
	CarAnimationController myCarAnimationController;

	PlayerGUI gui;

	int lapsToWin = 3;
	
	void Start() {
//		engine = GameObject.Find("CarRaceSound" + playerNumber.ToString()).GetComponent<AudioSource>(); //FIXME

		carController = GetComponentInChildren<MotoPhysics>();
		myCarAnimationController = gameObject.GetComponentInChildren<CarAnimationController>();

		lastWayPoint = PathNode.first;

		gui = transform.FindChild("GUI").GetComponent<PlayerGUI>();

		lastWayPoint = PathNode.first;
	}

	void Update() {
		float zRotation = carController.gameObject.transform.rotation.eulerAngles.z;
		if ((zRotation > 50f && zRotation < 140f) || (zRotation > 220 && zRotation < 310f)) {
			if (isRespawning == false)
				StartCoroutine("Explode");		
		} 
	}
	
	//SetCarCurrentPlace, called from PlayerManager
	public void UpdateCurrentPlace(int newPlace) {
		currentPlace = newPlace;
		gui.UpdatePlace(newPlace);
	}

	public void UpdateInput(float newPedalIntensity, float newWheelIntensity) {
		if (carController != null) {
			carController.turnAxis = (invertWheel ? -1f : 1f) * newWheelIntensity;
			carController.aclAxis = (1 - newPedalIntensity);
			myCarAnimationController.wheelRotation =  newWheelIntensity;
			myCarAnimationController.pedalValue = (1 - newPedalIntensity);
			UpdateSound(newPedalIntensity);
		}
	}
	
	//When car hits a new waypoint, set it to lastWaypoint hit.
	void GhettoTriggerEnter(Collider other) { //FIXME DEAR GOD
		if (other.gameObject.tag == "InstaKill" && isRespawning == false) {
			StartCoroutine("Explode");
		}    
		if (other.gameObject.tag == "WayPoint") {
			PathNode pathNode = other.GetComponent<PathNode>();
			lastWayPoint = pathNode;
			if (pathNode.IsStart()) {
				lap++;
				print(lap + " -> lap++ = " + lap);
				gui.UpdateLap(lap);
				if (lap == lapsToWin) {
//					PlayerManager.s_instance.Win(id); //FIXME
				}
				
			}
		}
	}

	void UpdateSound(float pedal) {
		engine.pitch = 1f + (1 - pedal);
	}

	//RESPAWN.CS
	
	public GameObject explosionPrefab;
	Transform spawnPosition;
	bool isRespawning = false;

	public void TriggerExplode() {
		StartCoroutine("Explode");
	}

	IEnumerator Explode() {
		carController.enabled = false;
//		carController.canMove = false;
		isRespawning = true;
		print("Explode");
		Instantiate(explosionPrefab, carController.transform.position + new Vector3(0, 4, 0), Quaternion.identity);

		carController.rigidbody.velocity = new Vector3(0, 0, 0);

		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
			rend.enabled = false;

		yield return new WaitForSeconds(1);

		Transform spawnPosition = lastWayPoint.transform;
		carController.transform.position = new Vector3(spawnPosition.position.x, spawnPosition.position.y + 20, spawnPosition.position.z);
		carController.transform.rotation = spawnPosition.rotation;
		foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
			rend.enabled = true;

		isRespawning = false;
		carController.gameObject.collider.enabled = true;

		yield return new WaitForSeconds(1);

//		carController.Reset();
		carController.enabled = true;
//		carController.canMove = true;
	}
}


