using UnityEngine;
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
	public Dictionary<int, int> idToList; // Eh, efficient but not clean... //FIXME MAYBE?
	public List<GameObject> carPrefabs;
	public Transform carSpawnSpots;
	
	public Sprite win, lose;

	void Awake() {
		s_instance = this;
		cars = new List<CarData>();
		idToList = new Dictionary<int, int>();
	}
	
	void Update() {
		UpdatePlaces (); //TODO not every frame?
	}

	public void Addplayer(int player) {
		cars.Add(((GameObject)GameObject.Instantiate(carPrefabs [player], carSpawnSpots.GetChild(player).position, carSpawnSpots.GetChild(player).rotation)).GetComponent<CarData>());
		cars[cars.Count - 1].playerNumber = player;
		idToList.Add(player, cars.Count - 1);
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
		List<CarData> sortedCarList = new List<CarData>();

		for(int i = 0; i < cars.Count; i++) {
			if(sortedCarList.Count < 1)
				sortedCarList.Add(cars[i]);
			else {
				bool inserted = false;
				for(int j = 0; j < sortedCarList.Count && !inserted; j++) {
					if(cars[i].lap > sortedCarList[j].lap) {
						sortedCarList.Insert(j, cars[i]);
						inserted = true;
					} else {
						if(cars[i].lastWayPoint.distanceToEnd < sortedCarList[j].lastWayPoint.distanceToEnd) {
							sortedCarList.Insert(j, cars[i]);
							inserted = true;
						} else if(cars[i].lastWayPoint == sortedCarList[j].lastWayPoint) {
							float dist1 = Vector3.Distance(cars[i].carController.transform.position, cars[i].lastWayPoint.nextPaths[0].transform.position);
							float dist2 = Vector3.Distance(sortedCarList[j].carController.transform.position, cars[i].lastWayPoint.nextPaths[0].transform.position);
							Debug.DrawLine(cars[i].carController.transform.position, cars[i].lastWayPoint.nextPaths[0].transform.position, Color.yellow);
							Debug.DrawLine(sortedCarList[j].carController.transform.position, cars[i].lastWayPoint.nextPaths[0].transform.position, Color.red);
							if(dist1 < dist2) {
								sortedCarList.Insert(j, cars[i]);
								inserted = true;
							}
						}
					}
				}
				if(!inserted) 
					sortedCarList.Add(cars[i]);
			}
		}

		for(int i = 0; i < sortedCarList.Count; i++) {
			sortedCarList[i].UpdateCurrentPlace(i);
		}
	}
	
	public void SendOSCDataToCar(int playerID, float pedalIntensity, float wheelIntensity) {
		if (idToList.ContainsKey(playerID)) {
			int index = idToList [playerID];
			cars [index].UpdateInput(pedalIntensity, wheelIntensity);
		} else {
			print(playerID + " not in dict");
		}
	}
}
