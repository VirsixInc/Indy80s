using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Configure : MonoBehaviour {

	int currentPlayer = 0;

	InputSystem.PlayerInput.Type currentType = InputSystem.PlayerInput.Type.Pedal;
	bool logMinimum = true;

	float timer = 0f;

	public float waitTime = 3f;

	InputSystem inputSystem;

	public Text[] infoText;

	void Start () {
		inputSystem = InputSystem.s_instance;
		if (inputSystem == null) {
			Debug.LogError("Can't find component InputSystem", this);
			enabled = false;
			return;
		}
		ShowInstructions();
	}
	
	void Update () {
//		timer += Time.deltaTime;

//		if (timer > waitTime) {
//			timer = 0f;
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(currentType == InputSystem.PlayerInput.Type.Pedal) {
				if(logMinimum) {
					inputSystem.LogMinimum(currentPlayer, InputSystem.PlayerInput.Type.Pedal);
					logMinimum = false;
				} else {
					inputSystem.LogMaximum(currentPlayer, InputSystem.PlayerInput.Type.Pedal);
					logMinimum = true;
					currentType = InputSystem.PlayerInput.Type.Wheel;
				}
			} else if(currentType == InputSystem.PlayerInput.Type.Wheel) {
				if(logMinimum) {
					inputSystem.LogMinimum(currentPlayer, InputSystem.PlayerInput.Type.Wheel);
					logMinimum = false;
				} else {
					inputSystem.LogMaximum(currentPlayer, InputSystem.PlayerInput.Type.Wheel);
					logMinimum = true;
					currentType = InputSystem.PlayerInput.Type.Pedal;

					currentPlayer++;
					if(currentPlayer > InputSystem.NUM_PLAYERS - 1) {
						Application.LoadLevel("Intro");
//						inputSystem.state = InputSystem.State.Normal;
						enabled = false;
						return;
					}
				}
			}
			ShowInstructions();
		}
	}

	void ShowInstructions() {
		string text = "Player " + (currentPlayer + 1) + " " + currentType.ToString() + " ";

		if(logMinimum)
			text += "min";
		else
			text += "max";

		for (int i = 0; i < infoText.Length; i++) {
			infoText[i].text = text;
		}
	}
}
