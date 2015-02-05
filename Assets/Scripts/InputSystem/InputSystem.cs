using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour {
	
	public const int NUM_PLAYERS = 8;
	
	public class PlayerInput {
		public enum Type {
			Wheel, Pedal
		}
		
		public InputData wheelData, pedalData;
		public PlayerInput() {
			wheelData = new InputData();
			wheelData.min = 9999f;
			wheelData.max = -9999f;
			wheelData.values = new Queue<float>();

			pedalData = new InputData();
			pedalData.min = 9999f;
			pedalData.max = -9999f;
			pedalData.values = new Queue<float>();
		}
		
		public InputData wheel {
			get {
				return wheelData;
			}
		}
		
		public InputData pedal {
			get {
				return pedalData;
			}
		}
	}
	
	public class InputData {
		public float min, max;
		public Queue<float> values;
		
		public float GetRunningAverage() {
			float average = 0f;
			if (values.Count == 0)
				return average;

			foreach(float value in values) {
				average += value;
			}

			average /= (float)values.Count;
			
			return average;
		}

		public float GetRunningAverageNormalized() {
			float average = Mathf.Clamp(GetRunningAverage(), min, max);
			float normalized = (average - min) / (max - min);

			return normalized;
		}
		
		public void AddValue(float value) {
			if (values.Count > 10)
				values.Dequeue();
			values.Enqueue(value);
		}
	}
	
//	public enum State {
//		Configure, Normal
//	}
//	
//	public State state = State.Normal;
	
	public PlayerInput[] players;

	public static InputSystem s_instance;

	void Awake () {
		if(s_instance == null) {
			s_instance = this;
		} else {
			enabled = false;
			return;
//			Destroy(gameObject);
		}
		players = new PlayerInput[NUM_PLAYERS];
		
		for (int i = 0; i < NUM_PLAYERS; i++) {
			players[i] = new PlayerInput();
		}
	}

	public void AddInput(float value, int player, PlayerInput.Type type) {
		if (type == PlayerInput.Type.Pedal) {
			players[player].pedal.AddValue(value);
		} else if (type == PlayerInput.Type.Wheel) {
			players[player].wheel.AddValue(value);
		}
	}

	public void LogMinimum(int player, InputSystem.PlayerInput.Type type) {
		if (type == PlayerInput.Type.Pedal) {
			float average = players[player].pedal.GetRunningAverage();
			if(average < players[player].pedal.min)
				players[player].pedal.min = average;
			print ("player " + player + type.ToString() + " " + players[player].pedal.min);
		} else if (type == PlayerInput.Type.Wheel) {
			float average = players[player].wheel.GetRunningAverage();
			if(average < players[player].wheel.min)
				players[player].wheel.min = average;
			print ("player " + player + type.ToString() + " " + players[player].wheel.min);
		}
	}

	public void LogMaximum(int player, InputSystem.PlayerInput.Type type) {
		if (type == PlayerInput.Type.Pedal) {
			float average = players[player].pedal.GetRunningAverage();
			if(average > players[player].pedal.max)
				players[player].pedal.max = average;
			print ("player " + player + type.ToString() + " " + players[player].pedal.max);
		} else if (type == PlayerInput.Type.Wheel) {
			float average = players[player].wheel.GetRunningAverage();
			if(average > players[player].wheel.max)
				players[player].wheel.max = average;
			print ("player " + player + type.ToString() + " " + players[player].wheel.max);
		}
	}
}
