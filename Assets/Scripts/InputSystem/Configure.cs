using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Configure : MonoBehaviour {

	int currentPlayer = 0;

	InputSystem.PlayerInput.Type currentType = InputSystem.PlayerInput.Type.Pedal;
	bool logMinimum = true;
	
	public float waitTime = 3f;

	InputSystem inputSystem;

	public Text[] infoText;

	void Start () {
		inputSystem = InputSystem.s_instance;
		inputSystem.ResetValues();
		if (inputSystem == null) {
			Debug.LogError("Can't find component InputSystem", this);
			enabled = false;
			return;
		}
		ShowInstructions();
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Space)) {
			if(currentType == InputSystem.PlayerInput.Type.Pedal) {
				if(logMinimum) {
					inputSystem.LogMinimum(currentPlayer, InputSystem.PlayerInput.Type.Pedal);
				} else {
					inputSystem.LogMaximum(currentPlayer, InputSystem.PlayerInput.Type.Pedal);
				}
			} else if(currentType == InputSystem.PlayerInput.Type.Wheel) {
				if(logMinimum) {
					inputSystem.LogMinimum(currentPlayer, InputSystem.PlayerInput.Type.Wheel);
				} else {
					inputSystem.LogMaximum(currentPlayer, InputSystem.PlayerInput.Type.Wheel);
				}
			}
		}
		if(Input.GetKeyUp(KeyCode.Space)) {
			if(currentType == InputSystem.PlayerInput.Type.Pedal) {
				if(logMinimum) {
					logMinimum = false;
				} else {
					logMinimum = true;
					currentType = InputSystem.PlayerInput.Type.Wheel;
				}
			} else if(currentType == InputSystem.PlayerInput.Type.Wheel) {
				if(logMinimum) {
					logMinimum = false;
				} else {
					logMinimum = true;
					currentType = InputSystem.PlayerInput.Type.Pedal;
						
					currentPlayer++;
					if(currentPlayer > InputSystem.NUM_PLAYERS - 1) {
						Application.LoadLevel("Intro");
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
			if(currentType == InputSystem.PlayerInput.Type.Pedal)
				text += "down";
			else
				text += "right";
		else
			if(currentType == InputSystem.PlayerInput.Type.Pedal)
				text += "up";
			else
				text += "left";

		for (int i = 0; i < infoText.Length; i++) {
			infoText[i].text = text;
		}
	}
}
