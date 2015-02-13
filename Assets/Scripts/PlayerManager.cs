﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	
	//State Machine - Game is currently in Intro, select mode, play mode
	
//	bool playersHaveControlOfCars; //because of start and end with 3 second delay at beginning

	public static PlayerManager s_instance;

	[System.NonSerialized]
	public List<CarData> cars;
	public List<GameObject> carPrefabs;
	public Transform carSpawnSpots;

	//all of the images on each players screen that say 1st 2nd 3rd etc
	public GameObject[] placeInRaceAssets;
	public GameObject[] lapNumberAssets;
	public Sprite win, lose;
	
	//getters
	public List<CarData> ReturnCars(){return cars;}
	public GameObject[] ReturnPlaceInRaceAssets(){return placeInRaceAssets;}
	public GameObject[] ReturnLapNumberAssets(){return lapNumberAssets;}
	public int lapsToWin = 3;

	void Awake() {
		s_instance = this;
		cars = new List<CarData>();
	}

	void Start() {

	}

	void Update() {
		UpdatePlaces ();
	}

	void Addplayer(int player) {
		cars.Add(((GameObject)GameObject.Instantiate(carPrefabs [player], carSpawnSpots.GetChild(player).position, carSpawnSpots.GetChild(player).rotation)).GetComponent<CarData>());
		cars [player].playerNumber = player;
	}

	//FIXME 
//	public void Win(int playerWhoWon) {
//		if (hasSomebodyWonYet == false) {
//			hasSomebodyWonYet = true;
//						for (int i = 1; i <= cars.Length; i++) {
//								if (playerWhoWon == i)
//										GameObject.Find ("win" + i.ToString ()).GetComponent<Image> ().sprite = win;
//								else
//										GameObject.Find ("win" + i.ToString ()).GetComponent<Image> ().sprite = lose;
//
//						}
//						StartCoroutine ("EndRace");
//				}
//
//	}


	//FIXME
//	IEnumerator EndRace () {
//		yield return new WaitForSeconds (5);
//		Application.LoadLevel (0);
//	}

	//sets the place of each car to 1st, 2nd, 3rd etc....
	public void UpdatePlaces() {

		//FIXME 
//		//sort cars by all metrics
//		List<CarData> sortCarList = new List<CarData>();
//		foreach (CarData car in PlayerManager.s_instance.cars) {
//			if (sortCarList.Count == 0) //if this car is playing, put him in the sort list
//				sortCarList.Insert(0, car);
//			else if (sortCarList.Count > 0 && sortCarList[0].ReturnLastWayPoint()!=null) { //if this car is playing, put him in the sort list, not sure why returnlastwaypoint is there
//				for (int i = 0; i < sortCarList.Count; i++) {
//					//if car greater lap than position i, insert at i
//					if (car.lap > sortCarList[i].lap) { 
//						sortCarList.Insert(i, car);
//						break;
//					}
//					
//					//if car same lap but farther along on track
//					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() < sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() 
//					         && 
//					         car.lap == sortCarList[i].lap
//					         &&
//					         //does not apply during stretch from start to first waypoint when two cars are on same lap but one sortList's is farther
//					         car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()!=0
//					         &&
//					         sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()!=0) {
//						sortCarList.Insert(i, car); 
//						break;
//					}
//					
//					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() > sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()
//					         &&
//					         sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()==0) {
//						sortCarList.Insert(i, car); 
//						break;
//					}
//					
//					//if cars same lap and same waypoint
//					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() == sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()
//					         && 
//					         car.lap == sortCarList[i].lap) {
//						//compare distance 
//						if (car.CalculateDistanceFromLastWayPoint() > sortCarList[i].CalculateDistanceFromLastWayPoint()) { 
//							sortCarList.Insert(i, car);
//							break;
//						}
//						else if (i == (sortCarList.Count-1)) { //if all the cars have been compared against, insert at the end
//							sortCarList.Insert(sortCarList.Count, car);
//							break;
//						}
//					}
//					
//					else if (i == (sortCarList.Count-1)) { //if all the cars have been compared against, insert at the end
//						sortCarList.Insert(sortCarList.Count, car);
//						break;
//					}
//				}
//			}
//		}
//		
//		
//		for (int i = 0; i < sortCarList.Count(); i++) { // iterate through an ordered list of cars and their current place to the index of where they are wrt the array
//			sortCarList[i].SetCarCurrentPlace(i+1);
//		}
		
	}
	//	}
	


	//part of dec 31st version of PLAYERMANAGER
	public void SendOSCDataToCar(int playerID, float pedalIntensity, float wheelIntensity) {
		cars [playerID].UpdateInput(pedalIntensity, wheelIntensity);
	}


}
