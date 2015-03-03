//#define LOG_SERIAL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public enum State {
	Intro, LevelSelect, Main, Config 
};

public class GameManager : MonoBehaviour {

//#if LOG_SERIAL
	public bool showDebugStr = true;
	string[] serialInfo = new string[8];
//#endif
	public bool configured;

	public static GameManager s_instance;
	public static State currentState = State.Intro;
	public bool[] playersJoined = {false,false,false,false,false,false,false,false};
	public int counter = 10;
	bool isCountingDown = false;
	public Image[] isPlayingTexts;
	public Text counterText;
	public bool canControlCars = true;
	public AudioSource joinUpSound, beep;
	InputSystem inputSystem;

	void OnLevelWasLoaded(int level) {
		if (level == 0) {
			for (int  i = 0; i < 8; i++) {
				isPlayingTexts [i] = GameObject.Find("P" + (i + 1)).GetComponent<Image>();
				isPlayingTexts [i].gameObject.SetActive(false);
			}
			if (counterText == null)
				counterText = GameObject.Find("Timer").GetComponent<Text>();
			if (joinUpSound == null)
				joinUpSound = GameObject.Find("JoinUp").GetComponent<AudioSource>();
			if (beep == null)
				beep = GameObject.Find("Beep").GetComponent<AudioSource>();
			currentState = State.Intro;
		} else if (level == 1) {
			currentState = State.Main;
			for (int i = 0; i < playersJoined.Length; i++) {
				if(playersJoined[i]) {
					PlayerManager.s_instance.Addplayer(i);
				}
			}
		}
	}

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Start() {
		inputSystem = InputSystem.s_instance;

		for (int  i = 0; i < 8; i++) {
			isPlayingTexts [i] = GameObject.Find("P" + (i + 1)).GetComponent<Image>();
			isPlayingTexts [i].gameObject.SetActive(false);
		}
		if (counterText == null)
			counterText = GameObject.Find("Timer").GetComponent<Text>();

		if (!inputSystem.inputLoaded) {
			currentState = State.Config;
			Application.LoadLevel("Config");
		} else {
			currentState = State.Intro;
		}
	}
	
//#if LOG_SERIAL
	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			showDebugStr = !showDebugStr;
		}

		if (Input.GetKeyDown(KeyCode.X)) {
			currentState = State.Config;
			StopAllCoroutines();
			Application.LoadLevel("Config");
		}

		if (Input.GetKeyDown(KeyCode.S)) {
			if (Application.loadedLevelName == "Config") {
				Application.LoadLevel("Intro");
			} else {
				for (int i = 0; i < InputSystem.NUM_PLAYERS; i++) {
					playersJoined [i] = true;
					isPlayingTexts [i].gameObject.SetActive(true);
					if (!isCountingDown) {
						StartCoroutine("CountDown");
						isCountingDown = true;
						joinUpSound.Play();
					} else {
						counter = 5;
						joinUpSound.Play();
					}
				}
			}
		}
	}
//#endif
	
	IEnumerator CountDown() {
		while (counter > 0) {
			yield return new WaitForSeconds(.65f);
			counter--;
			counterText.text = counter.ToString();
			beep.Play();
		}
		Application.LoadLevel(1);
	}

	public void SerialInputRecieved(int[] message) { 
		float wheelIntensity = (float)message [0];
		float pedalIntensity = (float)message [1];
		int player = (int)message [2];

		inputSystem.AddInput(pedalIntensity, player, InputSystem.PlayerInput.Type.Pedal);
		inputSystem.AddInput(wheelIntensity, player, InputSystem.PlayerInput.Type.Wheel);

		float pedalNormalized = inputSystem.players [player].pedal.GetRunningAverageNormalized();
		float wheelNormalized = -(inputSystem.players [player].wheel.GetRunningAverageNormalized() * 2f - 1f);
//#if LOG_SERIAL
		serialInfo [player] = "Player " + player + "\n"
			+ "  Pedal: \n"
				+ "    Norm: " + pedalNormalized.ToString("F2") + " Cur: " + pedalIntensity + " Min: " + inputSystem.players [player].pedal.min.ToString("F0") + " Max: " + inputSystem.players [player].pedal.max.ToString("F0")
			+ "\n  Wheel: \n"
				+ "    Norm: " + wheelNormalized.ToString("F2") + " Cur: " + wheelIntensity + " Min: " + inputSystem.players [player].wheel.min.ToString("F0") + " Max: " + inputSystem.players [player].wheel.max.ToString("F0");
//#endif

		if (!inputSystem.inputAvailable) {
			return;
		}

		switch (currentState) {
			case State.Main:
				if (canControlCars) {
					if (!playersJoined [player])
						return;
					PlayerManager.s_instance.SendOSCDataToCar(player, pedalNormalized, wheelNormalized);
				}
				break;
			case State.Intro:
				if (pedalNormalized < .5f && playersJoined [player] == false) {
					playersJoined [player] = true;
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
			case State.Config:
				break;
		}
	}

	void OnGUI() {
		if (showDebugStr) {
			string debugStr = "";
			for (int i = 0; i < InputSystem.NUM_PLAYERS; i++) {
				debugStr += serialInfo [i] + "\n\n";
			}
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUI.Box(new Rect(0f, 0f, 300f, 900f), debugStr, GUI.skin.box);
		}
	}
}
