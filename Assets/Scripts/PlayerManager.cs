using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	
	//State Machine - Game is currently in Intro, select mode, play mode
	
	bool playersHaveControlOfCars; //because of start and end with 3 second delay at beginning

	public static PlayerManager s_instance;


	//CarDatas
	public CarData[] cars;
	
	//all of the images on each players screen that say 1st 2nd 3rd etc
	public GameObject[] placeInRaceAssets;
	public GameObject[] lapNumberAssets;
	public Sprite win, lose;
	
	//getters
	public CarData[] ReturnCars(){return cars;}
	public GameObject[] ReturnPlaceInRaceAssets(){return placeInRaceAssets;}
	public GameObject[] ReturnLapNumberAssets(){return lapNumberAssets;}
	public int lapsToWin = 3;
	bool hasSomebodyWonYet = false;

	void Awake() {
		s_instance = this;
	}

	void Start() {
		for (int i = 1; i <= cars.Length; i++) {
			cars [i - 1] = GameObject.Find ("car" + i.ToString ()).GetComponent<CarData> ();
		}
		GameObject.Find ("GameManager").SendMessage ("HideIdleCars");
	}

	public void Win(int playerWhoWon) {
		if (hasSomebodyWonYet == false) {
			hasSomebodyWonYet = true;
						for (int i = 1; i <= cars.Length; i++) {
								if (playerWhoWon == i)
										GameObject.Find ("win" + i.ToString ()).GetComponent<Image> ().sprite = win;
								else
										GameObject.Find ("win" + i.ToString ()).GetComponent<Image> ().sprite = lose;

						}
						StartCoroutine ("EndRace");
				}

	}

	IEnumerator EndRace () {
		yield return new WaitForSeconds (5);
		Application.LoadLevel (0);
	}

	//sets the place of each car to 1st, 2nd, 3rd etc....
	public void UpdatePlaces() {
		
		//sort cars by all metrics
		List<CarData> sortCarList = new List<CarData>();
		for (int j = 0; j < cars.Length; j++) {
			CarData car = cars[j];
			if (sortCarList.Count == 0 && GameManager.s_instance.playerBools[j]) //if this car is playing, put him in the sort list
				sortCarList.Insert(0, car);
			else if (sortCarList.Count > 0 && sortCarList[0].ReturnLastWayPoint()!=null && GameManager.s_instance.playerBools[j]) { //if this car is playing, put him in the sort list, not sure why returnlastwaypoint is there
				for (int i = 0; i < sortCarList.Count; i++) {
					//if car greater lap than position i, insert at i
					if (car.lap > sortCarList[i].lap) { 
						sortCarList.Insert(i, car);
						break;
					}
					
					//if car same lap but farther along on track
					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() < sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() 
					         && 
					         car.lap == sortCarList[i].lap
					         &&
					         //does not apply during stretch from start to first waypoint when two cars are on same lap but one sortList's is farther
					         car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()!=0
					         &&
					         sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()!=0) {
						sortCarList.Insert(i, car); 
						break;
					}
					
					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() > sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()
					         &&
					         sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()==0) {
						sortCarList.Insert(i, car); 
						break;
					}
					
					//if cars same lap and same waypoint
					else if (car.ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd() == sortCarList[i].ReturnLastWayPoint().GetComponent<PathNode>().ReturnDistanceToEnd()
					         && 
					         car.lap == sortCarList[i].lap) {
						//compare distance 
						if (car.CalculateDistanceFromLastWayPoint() > sortCarList[i].CalculateDistanceFromLastWayPoint()) { 
							sortCarList.Insert(i, car);
							break;
						}
						else if (i == (sortCarList.Count-1)) { //if all the cars have been compared against, insert at the end
							sortCarList.Insert(sortCarList.Count, car);
							break;
						}
					}
					
					else if (i == (sortCarList.Count-1)) { //if all the cars have been compared against, insert at the end
						sortCarList.Insert(sortCarList.Count, car);
						break;
					}
				}
			}
		}
		
		
		for (int i = 0; i < sortCarList.Count(); i++) { // iterate through an ordered list of cars and their current place to the index of where they are wrt the array
			sortCarList[i].SetCarCurrentPlace(i+1);
		}
		
	}
	//	}
	
	void Update() {
		UpdatePlaces ();
	}

	//part of dec 31st version of PLAYERMANAGER
	public void SendOSCDataToCar(int newPlayerID, float newPedalIntensity, float newWheelIntensity) {
		cars [newPlayerID].ReceivePedalWheelInput (newPedalIntensity, newWheelIntensity);
	}


}
