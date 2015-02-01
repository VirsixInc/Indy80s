using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using OSC.NET;
using System;
using System.Threading;

public struct playerEvent {
	public int pid;
	public float pedIntensity;
	public float wheelIntensity;
};

public enum States {startScreen, levelSelect, playing, config};

public class GameManager : MonoBehaviour {

	public static GameManager s_instance;
	public static States currentState = States.startScreen;
	public bool[] playerBools = {false,false,false,false,false,false,false,false};
	public int counter = 10;
	bool isCountingDown = false;
	public Image[] isPlayingTexts;
	public Text counterText;
	private OSCTransmitter transmit;
	bool networkE = true;
	float[] previousWheelIntensity = {0,0,0,0,0,0,0,0};
	float[] formerWheelIntensity = {0,0,0,0,0,0,0,0};
	[Range (0, .5f)]
	public float wheelGlitchBuffer = .6f;
	public bool canControlCars = true;
	public Animation introCameraMove;
	public AudioSource joinUpSound, beep;

	void OnLevelWasLoaded(int level){
//		if(level == 1)
//		HideIdleCars();
	}

	void Awake(){
		s_instance = this;
		if(networkE){
			transmit = new OSCTransmitter("192.168.1.255", 9999);
		}
		DontDestroyOnLoad(transform.gameObject);
	}
	
	IEnumerator CountDown() {
		while (counter > 0) {
			yield return new WaitForSeconds(1);
			counter--;
			counterText.text = counter.ToString();
			beep.Play();
		}
		Application.LoadLevel (1);
//		yield return new WaitForSeconds (introCameraMove.animation.clip.length);
		currentState = States.playing;
		//HideIdleCars ();
	}


	public void HideIdleCars() {
		for (int i =0; i < PlayerManager.s_instance.cars.Length; i++) {
			if (!playerBools[i]) {
				if(PlayerManager.s_instance != null) {
					if(PlayerManager.s_instance.cars[i] != null) {
						PlayerManager.s_instance.cars[i].gameObject.transform.parent.gameObject.SetActive(false);
						GameObject.Find("Place" + (i + 1)).SetActive(false);
						GameObject.Find("LapNumber" + (i + 1)).SetActive(false);
						GameObject.Find("Lap" + (i + 1)).SetActive(false);
					} else {

						print ("CAR BITCH");
					}
				} else {

					print ("bitch");
				}
			}
		}
	}

	public void OSCMessageReceived (OSC.NET.OSCMessage message){  
				ArrayList args = message.Values;
				playerEvent d;
				d.pid = (int)args [0];
				d.pedIntensity = (float)args [1];
				d.wheelIntensity = (float)args [2];
				float tempWheelIntensity;
				//check if OSC is giving an outliar, if it is, then give an alternate, adequate value.
				if ((Mathf.Abs (d.wheelIntensity - previousWheelIntensity [d.pid]) > wheelGlitchBuffer) && formerWheelIntensity != previousWheelIntensity) {
						tempWheelIntensity = previousWheelIntensity [d.pid];
				} else {
						tempWheelIntensity = d.wheelIntensity;
				}
				switch (currentState) {
					case States.playing:
						if (canControlCars) {
						PlayerManager.s_instance.SendOSCDataToCar (d.pid, d.pedIntensity, tempWheelIntensity);
						formerWheelIntensity[d.pid] = previousWheelIntensity[d.pid];
						previousWheelIntensity[d.pid] = tempWheelIntensity;
				
			}
				break;
					case States.startScreen:
						if(d.pedIntensity < .5f && playerBools[d.pid]==false) {
							
							playerBools[d.pid] = true;
							isPlayingTexts[d.pid].gameObject.SetActive(true);
							if (!isCountingDown) {
								StartCoroutine("CountDown");
								isCountingDown = true;
								joinUpSound.Play();
							}
							else {
								counter = 10;
								joinUpSound.Play();
							}
						}
						break;
				}

		}
}
