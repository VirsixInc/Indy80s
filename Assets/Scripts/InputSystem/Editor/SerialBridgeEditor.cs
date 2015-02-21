using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO.Ports;
using System.Threading;

[CustomEditor(typeof(SerialBridge))]
public class SerialBridgeEditor : Editor {

	string[] ports;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		serializedObject.Update();

		if (ports == null)
			UpdatePorts();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
		EditorGUILayout.Toggle(true);
		GUILayout.Label("Port One");

		int portIndex = EditorGUILayout.Popup(0, ports);
		SerialBridge serialBridge = (SerialBridge)target;
		UpdatePorts();
		serialBridge.portOne = ports [portIndex];

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	void UpdatePorts() {
		ports = SerialPort.GetPortNames();
		if (ports.Length == 0)
			ports = new string[1] {"None"};
	}
}
