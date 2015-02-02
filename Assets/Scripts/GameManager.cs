using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using OSC.NET;
using System;
using System.Threading;

//public struct playerEvent {
//	public int pid;
//	public float pedIntensity;
//	public float wheelIntensity;
//};

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
//	float[] previousWheelIntensity = {0,0,0,0,0,0,0,0};
//	float[] formerWheelIntensity = {0,0,0,0,0,0,0,0};
//	[Range (0, .5f)]
//	public float wheelGlitchBuffer = .6f;
	public bool canControlCars = true;
	public Animation introCameraMove;
	public AudioSource joinUpSound, beep;

	InputSystem inputSystem;

	void OnLevelWasLoaded(int level){
		if (level == 0) {
			for(int  i = 0; i < 8; i++) {
				isPlayingTexts[i] = GameObject.Find("P" + (i+1)).GetComponent<Image>();
				isPlayingTexts[i].gameObject.SetActive(false);
			}
			if (counterText == null)
				counterText = GameObject.Find("Timer").GetComponent<Text>();
			if(joinUpSound == null)
				joinUpSound = GameObject.Find("JoinUp").GetComponent<AudioSource>();
			if(beep == null)
				beep = GameObject.Find("Beep").GetComponent<AudioSource>();
		}
	}

	void Awake(){
		if (s_instance == null) {
			s_instance = this;
			if (networkE) {
				transmit = new OSCTransmitter("192.168.1.255", 9999);
			}
			DontDestroyOnLoad(gameObject);
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Start() {
		inputSystem = InputSystem.s_instance;
		Application.LoadLevel("Config");

		for(int  i = 0; i < 8; i++) {
			isPlayingTexts[i] = GameObject.Find("P" + (i+1)).GetComponent<Image>();
			isPlayingTexts[i].gameObject.SetActive(false);
		}
		if (counterText == null)
			counterText = GameObject.Find("Timer").GetComponent<Text>();
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
					}
				}
			}
		}
	}

	public void SerialInputRecieved(int[] message) {  
//		ArrayList args = message.Values;
//		playerEvent d;
		float pedalIntensity = (float)message [0];
		float wheelIntensity = (float)message [1];
		int player = (int)message [2];
		float tempWheelIntensity;

//		//check if OSC is giving an outlyer, if it is, then give an alternate, adequate value.
//		if ((Mathf.Abs(d.wheelIntensity - previousWheelIntensity [d.pid]) > wheelGlitchBuffer) && formerWheelIntensity != previousWheelIntensity) {
//			tempWheelIntensity = previousWheelIntensity [d.pid];
//		} else {
//			tempWheelIntensity = d.wheelIntensity;
//		}

		inputSystem.AddInput(pedalIntensity, player, InputSystem.PlayerInput.Type.Pedal);
		inputSystem.AddInput(wheelIntensity, player, InputSystem.PlayerInput.Type.Wheel);

		float pedalNormalized = inputSystem.players [player].pedal.GetRunningAverageNormalized();
		float wheelNormalized = inputSystem.players [player].wheel.GetRunningAverageNormalized();

		switch (currentState) {
			case States.playing:
				if (canControlCars) {
					PlayerManager.s_instance.SendOSCDataToCar(player, pedalNormalized, wheelNormalized);
//					formerWheelIntensity [player] = previousWheelIntensity [player];
//					previousWheelIntensity [player] = tempWheelIntensity;
				}
				break;
			case States.startScreen:
				if (pedalNormalized < .5f && playerBools [player] == false) {
							
					playerBools [player] = true;
					isPlayingTexts [player].gameObject.SetActive(true);
					if (!isCountingDown) {
						StartCoroutine("CountDown");
						isCountingDown = true;
						joinUpSound.Play();
					} else {
						counter = 10;
						joinUpSound.Play();
					}
				}
				break;
			case States.config:
				break;
		}

	}
}
