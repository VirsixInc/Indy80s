#define LOG_SERIAL

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
	public static States currentState = States.config;
	public bool[] playerBools = {false,false,false,false,false,false,false,false};
	public int counter = 10;
	bool isCountingDown = false;
	public Image[] isPlayingTexts;
	public Text counterText;
//	private OSCTransmitter transmit;
//	bool networkE = true;
//	float[] previousWheelIntensity = {0,0,0,0,0,0,0,0};
//	float[] formerWheelIntensity = {0,0,0,0,0,0,0,0};
//	[Range (0, .5f)]
//	public float wheelGlitchBuffer = .6f;
	public bool canControlCars = true;
//	public Animation introCameraMove;
	public AudioSource joinUpSound, beep;

	InputSystem inputSystem;

#if LOG_SERIAL
	string[] serialInfo = new string[8];
	bool showDebugStr = true;
#endif

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
			currentState = States.startScreen;
		}
	}

	void Awake(){
		if (s_instance == null) {
			s_instance = this;
//			if (networkE) {
//				transmit = new OSCTransmitter("192.168.1.255", 9999);
//			}
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

#if LOG_SERIAL
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			showDebugStr = !showDebugStr;
		}

		if (Input.GetKeyDown(KeyCode.S)) {
			if(Application.loadedLevelName == "Config") {
				Application.LoadLevel("Intro");
			} else {
				for(int i = 0; i < InputSystem.NUM_PLAYERS; i++) {
					playerBools [i] = true;
					isPlayingTexts [i].gameObject.SetActive(true);
					if (!isCountingDown) {
						StartCoroutine("CountDown");
						isCountingDown = true;
						joinUpSound.Play();
					} else {
						counter = 10;
						joinUpSound.Play();
					}
				}
			}
		}
	}
#endif
	
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
		float wheelIntensity = (float)message [0];
		float pedalIntensity = (float)message [1];
		int player = (int)message [2];
//		float tempWheelIntensity;

//		//check if OSC is giving an outlyer, if it is, then give an alternate, adequate value.
//		if ((Mathf.Abs(d.wheelIntensity - previousWheelIntensity [d.pid]) > wheelGlitchBuffer) && formerWheelIntensity != previousWheelIntensity) {
//			tempWheelIntensity = previousWheelIntensity [d.pid];
//		} else {
//			tempWheelIntensity = d.wheelIntensity;
//		}

		inputSystem.AddInput(pedalIntensity, player, InputSystem.PlayerInput.Type.Pedal);
		inputSystem.AddInput(wheelIntensity, player, InputSystem.PlayerInput.Type.Wheel);

		float pedalNormalized = inputSystem.players [player].pedal.GetRunningAverageNormalized();
		float wheelNormalized = -(inputSystem.players [player].wheel.GetRunningAverageNormalized() * 2f - 1f);

//		print("player: " + player + " pedal: " + (int)pedalNormalized + " wheel: " + (int)wheelNormalized);

#if LOG_SERIAL
		serialInfo[player] = "Player " + player + "\n" + 
			"  Pedal: \n" +
				"    Norm: " + pedalNormalized.ToString("F2") + " Cur: " + pedalIntensity + " Min: " + inputSystem.players[player].pedal.min + " Max: " + inputSystem.players[player].pedal.max + 
			"\n  Wheel: \n" +
				"    Norm: " + wheelNormalized.ToString("F2") + " Cur: " + wheelIntensity + " Min: " + inputSystem.players[player].wheel.min + " Max: " + inputSystem.players[player].wheel.max;
#endif

//		if(player == 0)
//			print ("player " + player + " pedal " + pedalNormalized + " wheel " + wheelNormalized);

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

#if LOG_SERIAL
	void OnGUI() {
		if(showDebugStr) {
			string debugStr = "";
			for(int i = 0; i < InputSystem.NUM_PLAYERS; i++) {
				debugStr += serialInfo[i] + "\n\n";
			}
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			print (GUI.skin.box.alignment.ToString());
			GUI.Box (new Rect (0f, 0f, 300f, 600f), debugStr, GUI.skin.box);
		}
	}
#endif
}
