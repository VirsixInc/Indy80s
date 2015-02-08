#define LOG_SERIAL

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

public enum States {startScreen, levelSelect, playing, config};

[XmlRoot("inputContainer")]
public class inputCont {
  [XmlArray("Players"),XmlArrayItem("Player")]
  public List<inputData> savedPlayers = new List<inputData>();
  public inputData[] players;
 
 	public void Save(string path){
 		var serializer = new XmlSerializer(typeof(inputCont));
 		using(var stream = new FileStream(path, FileMode.Create))
 		{
 			serializer.Serialize(stream, this);
 		}
 	}
 
 	public static inputCont Load(string path){
 		var serializer = new XmlSerializer(typeof(inputCont));
 		using(var stream = new FileStream(path, FileMode.Open))
 		{
 			return serializer.Deserialize(stream) as inputCont;
 		}
 	}
  public static inputCont LoadFromText(string text){
 		var serializer = new XmlSerializer(typeof(inputCont));
 		return serializer.Deserialize(new StringReader(text)) as inputCont;
 	}
}

public class inputData {
  [XmlAttribute("name")]
    public int id;
    public float minPed;
    public float maxPed;
    public float minWheel;
    public float maxWheel;
}
public class GameManager : MonoBehaviour {

  public bool debugMode;
#if LOG_SERIAL
	public bool showDebugStr = true;
	string[] serialInfo = new string[8];
#endif
  public bool configured;
   public TextAsset inputConfiguration;

	public static GameManager s_instance;
	public static States currentState = States.config;
	public bool[] playerBools = {false,false,false,false,false,false,false,false};
	public int counter = 10;
	bool isCountingDown = false;
	public Image[] isPlayingTexts;
	public Text counterText;
	public bool canControlCars = true;

	public AudioSource joinUpSound, beep;

	InputSystem inputSystem;
  inputCont thisInputCont;

  int rotTestInput;
  int pedTestInput;

  void saveAllVals(){
    thisInputCont = new inputCont();
    for(int i = 0; i<8; i++){
      inputData currInputData = new inputData();
      currInputData.id = i;
      currInputData.minPed = inputSystem.players[i].wheelData.min;
      currInputData.maxPed = inputSystem.players[i].wheelData.max;
      currInputData.minWheel = inputSystem.players[i].pedalData.min;
      currInputData.maxWheel = inputSystem.players[i].pedalData.max;
      thisInputCont.savedPlayers.Add(currInputData);
    }
   thisInputCont.Save(Path.Combine(Application.persistentDataPath, "configData.xml"));
   print(Application.persistentDataPath);
  print("SAVED XML");
  }

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
			DontDestroyOnLoad(gameObject);
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Start() {
		inputSystem = InputSystem.s_instance;
    if(debugMode){
      setDebugVals(0);
    }else{
    }
    Application.LoadLevel("Config");

		for(int  i = 0; i < 8; i++) {
			isPlayingTexts[i] = GameObject.Find("P" + (i+1)).GetComponent<Image>();
			isPlayingTexts[i].gameObject.SetActive(false);
		}
		if (counterText == null)
			counterText = GameObject.Find("Timer").GetComponent<Text>();
	}

  void setDebugVals(int id){
    inputSystem.players[id].wheelData.min = 0;
    inputSystem.players[id].wheelData.max = 500;
    inputSystem.players[id].pedalData.min = 0;
    inputSystem.players[id].pedalData.max = 500;
  }
#if LOG_SERIAL
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			showDebugStr = !showDebugStr;
		}
		if(Input.GetKeyDown(KeyCode.W)) {
      saveAllVals();
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
		currentState = States.playing;
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
    if(debugMode && message.Length != 4){
      return;
    }
		float wheelIntensity = (float)message [0];
		float pedalIntensity = (float)message [1];
		int player = (int)message [2];

		inputSystem.AddInput(pedalIntensity, player, InputSystem.PlayerInput.Type.Pedal);
		inputSystem.AddInput(wheelIntensity, player, InputSystem.PlayerInput.Type.Wheel);

		float pedalNormalized = inputSystem.players [player].pedal.GetRunningAverageNormalized();
		float wheelNormalized = -(inputSystem.players [player].wheel.GetRunningAverageNormalized() * 2f - 1f);
#if LOG_SERIAL
		serialInfo[player] = "Player " + player + "\n" + 
			"  Pedal: \n" +
				"    Norm: " + pedalNormalized.ToString("F2") + " Cur: " + pedalIntensity + " Min: " + inputSystem.players[player].pedal.min + " Max: " + inputSystem.players[player].pedal.max + 
			"\n  Wheel: \n" +
				"    Norm: " + wheelNormalized.ToString("F2") + " Cur: " + wheelIntensity + " Min: " + inputSystem.players[player].wheel.min + " Max: " + inputSystem.players[player].wheel.max;
#endif
		switch (currentState) {
			case States.playing:
				if (canControlCars) {
					PlayerManager.s_instance.SendOSCDataToCar(player, pedalNormalized, wheelNormalized);
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

	void OnGUI() {
    if(debugMode){
      pedTestInput = (int)(GUI.HorizontalSlider(new Rect(50, 50, 100, 30), pedTestInput, 0, 500));
      rotTestInput = (int)(GUI.HorizontalSlider(new Rect(50, 25, 100, 30), rotTestInput, 0, 500));

      int[] testArr = new int[4]; // length of 4 is a debug array
      testArr[0] = rotTestInput;
      testArr[1] = pedTestInput;
      testArr[2] = 0;
      testArr[3] = 0; // defines as debug

      SerialInputRecieved(testArr);

      

#if LOG_SERIAL
      if(showDebugStr) {
        string debugStr = "";
        for(int i = 0; i < InputSystem.NUM_PLAYERS; i++) {
          debugStr += serialInfo[i] + "\n\n";
        }
        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        GUI.Box (new Rect (0f, 0f, 300f, 600f), debugStr, GUI.skin.box);
      }
#endif
    }
	}
}
