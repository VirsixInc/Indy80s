using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour {

	bool gameStarted = false;

	public float gameStartTime = 8f;

	bool[] players = {false, false, false, false, false, false, false, false};

	public Image[] playerText;

	public Text instructionsText;
	public Text timerText;

	void Start () {
		for(int i = 0; i < 8; i++) {
			playerText[i].enabled = false;
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			players[0] = true;
			playerText[0].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			players[1] = true;
			playerText[1].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			players[2] = true;
			playerText[2].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			players[3] = true;
			playerText[3].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			players[4] = true;
			playerText[4].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)) {
			players[5] = true;
			playerText[5].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)) {
			players[6] = true;
			playerText[6].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha8)) {
			players[7] = true;
			playerText[7].enabled = true;
			if(!gameStarted) {
				gameStarted = true;
				StartCoroutine("StartTimer");
			}
		}
	}

	IEnumerator StartTimer() {
		timerText.enabled = true;
		instructionsText.text = "Starting in: ";
		float timer = gameStartTime;
		while(timer > 0f) {
			timer -= Time.deltaTime;
			if(gameStartTime < 0f)
				gameStartTime = 0f;
			timerText.text = Mathf.CeilToInt(timer).ToString();
			yield return null;
		}
		Application.LoadLevel("Main");
	}
}
