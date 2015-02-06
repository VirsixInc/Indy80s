﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarData : MonoBehaviour {
	
	//CarData is attached to each car gameobject	
	public int lap = 1;
	[Range (1, 8)] public int id;
	[Range (1, 8)] public int currentPlace;
	public bool isPlayer, hasGoneHalfWay;
	public GameObject lastWayPoint;	//lastWayPoint is used to calculate place and spawnpoint
	public Image placeInRaceHolder;
	public Image lapHolder;
//	PlayerManager playerManager;
	public float isInverted = 1;
	public float invertPedal = 0f;
	public AudioSource engine;
	public float enginePitchRatio = .01f;

	MotoPhysics myHoverCarControl;
	CarAnimationController myCarAnimationController;
	
	public float distanceFromLastWayPoint;

  public GameObject explodeFab;
  bool isRespawning;
  float timeSinceDead;
  float deathDelay = 1f;
	
	void Start() {
		engine = GameObject.Find ("CarRaceSound" + id.ToString ()).GetComponent<AudioSource>();
		placeInRaceHolder = GameObject.Find ("Place" + id.ToString ()).GetComponent<Image>();
		if (placeInRaceHolder == null || placeInRaceHolder.sprite == null) {
			//print ("OK");	
			return;
		}
		lapHolder = GameObject.Find ("LapNumber" + id.ToString ()).GetComponent<Image>();
//		playerManager = PlayerManager.s_instance;
		myHoverCarControl = gameObject.GetComponent<MotoPhysics> ();
		myCarAnimationController = gameObject.GetComponentInChildren<CarAnimationController> ();
		SetPlaceInRaceAsset ();
		SetLapAsset ();
		//		lastWayPoint = PathNode.first; first cannot be isStart with the way PathNode is currently set up - we could make it this way later
		//		StartCoroutine ("CheckRespawn");
	}
	
	//getters
	public int ReturnLap(){return lap;}
	public GameObject ReturnLastWayPoint(){return lastWayPoint;}
	
	//setters
	
	//SetCarCurrentPlace, called from PlayerManager
	public void SetCarCurrentPlace(int newCurrentPlace) {
		currentPlace = newCurrentPlace;
//		SetPlaceInRaceAsset ();
	}
	
	public void SetLastWayPoint(GameObject newPathNode) {
		lastWayPoint = newPathNode;
	}
	
	void SetPlaceInRaceAsset() {
		GameObject[] cars = PlayerManager.s_instance.ReturnPlaceInRaceAssets ();
		if (cars [currentPlace - 1] != null) {
			if(cars[currentPlace - 1].GetComponent<Image>() != null) {
				if(placeInRaceHolder.sprite != null) {
					placeInRaceHolder.sprite = cars[currentPlace - 1].GetComponent<Image>().sprite;
					return;
				}
			}
		}
//		placeInRaceHolder.sprite = playerManager.ReturnPlaceInRaceAssets() [currentPlace - 1].GetComponent<Image>().sprite; //sets HUD image of 1st, 2nd, 3rd
	}
	
	void SetLapAsset () {
		lapHolder.sprite = PlayerManager.s_instance.ReturnLapNumberAssets () [lap-1].GetComponent<Image> ().sprite;
		
	}
	
  void Explode(){
		Instantiate(explodeFab, transform.position+new Vector3(0,4,0), Quaternion.identity);
		rigidbody.velocity = new Vector3(0,0,0);
		foreach (MeshRenderer x in GetComponentsInChildren<MeshRenderer>())
			x.enabled = false;
    timeSinceDead = Time.time;

		gameObject.collider.enabled = true;
    isRespawning = true;
  }
	void Update() {
    float zRotation = gameObject.transform.rotation.eulerAngles.z;
		if ((zRotation > 50f && zRotation < 140f) || (zRotation > 220 && zRotation < 310f)) {
			if (isRespawning == false){
        Explode();
      }
    }
    if(timeSinceDead + deathDelay < Time.time && isRespawning){
      Transform spawnPosition = lastWayPoint.transform;
      transform.position = new Vector3 (spawnPosition.position.x, spawnPosition.position.y + 20, spawnPosition.position.z) ;
      transform.rotation = spawnPosition.rotation;
      foreach (MeshRenderer x in GetComponentsInChildren<MeshRenderer>())
        x.enabled = true;
      isRespawning = false;
      myHoverCarControl.forwardThrust = 0;
      myHoverCarControl.turnStrength = 0;
    }
	}
	
	public void ReceivePedalWheelInput (float newPedalIntensity, float newWheelIntensity) {
				if (myHoverCarControl != null) {
						myHoverCarControl.turnAxis = newWheelIntensity;
						myHoverCarControl.aclAxis = (1 - newPedalIntensity + invertPedal);
						myCarAnimationController.wheelRotation = isInverted * newWheelIntensity;
						myCarAnimationController.pedalValue = (1 - newPedalIntensity + invertPedal);
						PitchShift (newPedalIntensity);
				}
		}
	
	//When car hits a new waypoint, set it to lastWaypoint hit.
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "InstaKill" && isRespawning == false){
      Explode();
    }    
		if (other.gameObject.tag == "WayPoint") {
			lastWayPoint = other.gameObject;
			//			print (lastWayPoint.GetComponent<PathNode>().ReturnDistanceToEnd());
			if (other.GetComponent<PathNode>().ReturnDistanceToEnd()==0) {
				if (lap == PlayerManager.s_instance.lapsToWin) {
					PlayerManager.s_instance.Win(id);
				}

				else if (hasGoneHalfWay) {
					lap++; //doesnt work if people can reverse but we cant reverse so it's chill.
					SetLapAsset();	
					hasGoneHalfWay = false;
				}
				
			}
			else if (other.GetComponent<PathNode>().IsHalfWay()) {
				hasGoneHalfWay = true;
			}
		}
	}
	
	public float CalculateDistanceFromLastWayPoint() {
		//we may want to take the negative value of this to invert order
		distanceFromLastWayPoint = Vector3.Distance (lastWayPoint.transform.position, transform.position); //for debug
		return Vector3.Distance(lastWayPoint.transform.position, transform.position);
	}
	
	void Respawn() {
		//TODO play some sort of respawn animation that resets the car position at the last waypoint
	}

	void PitchShift (float pedal) {
		engine.pitch = 1f + (1-pedal);
	}
}


