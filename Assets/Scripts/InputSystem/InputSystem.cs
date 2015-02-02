using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour {
	
	public const int NUM_PLAYERS = 8;
	
	public class PlayerInput {
		public enum Type {
			Wheel, Pedal
		}
		
		InputData wheelData, pedalData;
		public PlayerInput() {
			wheelData = new InputData();
			wheelData.min = wheelData.max = 0f;
			wheelData.values = new Queue<float>();

			pedalData = new InputData();
			pedalData.min = pedalData.max = 0f;
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
			Queue<float>.Enumerator iter = values.GetEnumerator();
			do {
				average += iter.Current;
			} while (iter.MoveNext());
			
			average /= (float)values.Count;
			
			return average;
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
	
	PlayerInput[] players;

	public static InputSystem s_instance;

	void Awake () {
		if(s_instance == null) {
			s_instance = this;
		} else {
			Destroy(gameObject);
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
			players[player].pedal.min = players[player].pedal.GetRunningAverage();
		} else if (type == PlayerInput.Type.Wheel) {
			players[player].wheel.min = players[player].wheel.GetRunningAverage();
		}
	}

	public void LogMaximum(int player, InputSystem.PlayerInput.Type type) {
		if (type == PlayerInput.Type.Pedal) {
			players[player].pedal.max = players[player].pedal.GetRunningAverage();
		} else if (type == PlayerInput.Type.Wheel) {
			players[player].wheel.max = players[player].wheel.GetRunningAverage();
		}
	}
}
