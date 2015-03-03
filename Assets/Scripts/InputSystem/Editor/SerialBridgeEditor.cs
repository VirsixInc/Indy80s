using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System;
using System.Collections.Generic;
using Microsoft.Win32;

[CustomEditor(typeof(SerialBridge))]
public class SerialBridgeEditor : Editor {

	static string[] ports;// = new string[] {"None", "None2"};

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		serializedObject.Update();

		if (ports == null)
			ports = GetPorts();

		SerialBridge serialBridge = (SerialBridge)target;

		ports = GetPorts();
		
		for (int i = 0; i < ports.Length; i++) {
			ports[i] = ports[i].Replace('/', ' ');
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
		GUILayout.Label("Port One");
		serialBridge.useFirst = EditorGUILayout.Toggle(serialBridge.useFirst);
		EditorGUILayout.EndHorizontal();
		serialBridge.port1Index = EditorGUILayout.Popup(serialBridge.port1Index, ports);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
		GUILayout.Label("Port Two");
		serialBridge.useSecond = EditorGUILayout.Toggle(serialBridge.useSecond);
		EditorGUILayout.EndHorizontal();
		serialBridge.port2Index = EditorGUILayout.Popup(serialBridge.port2Index, ports);
		EditorGUILayout.EndHorizontal();


		for (int i = 0; i < ports.Length; i++) {
			ports[i] = ports[i].Replace(' ', '/');
		}

		serialBridge.portOne = ports [serialBridge.port1Index];
		serialBridge.portTwo = ports [serialBridge.port2Index];

		serializedObject.ApplyModifiedProperties();
	}

	string[] GetPorts() {
		PlatformID pID = Environment.OSVersion.Platform;
		List<string> serial_ports = new List<string> ();

		if (pID == PlatformID.Unix || pID == PlatformID.MacOSX || (int)pID == 128) {
			//OSX
			string[] ttys = Directory.GetFiles("/dev/", "tty.*");
			foreach (string dev in ttys) {
				if (dev != "/dev/tty" && dev.StartsWith("/dev/tty") && !dev.StartsWith("/dev/ttyC"))
					serial_ports.Add(dev);
			}
			//Linux (TEST ME)
			ttys = Directory.GetFiles("/dev/", "tty*");
			foreach (string dev in ttys) {
				if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB") || dev.StartsWith("/dev/ttyACM"))
					serial_ports.Add(dev);
			}
		} else {
			//Windows
			using (RegistryKey subkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM")) {
				if (subkey != null) {
					string[] names = subkey.GetValueNames();
					foreach (string value in names) {
						string port = subkey.GetValue(value, "").ToString();
						if (port != "")
							serial_ports.Add(port);
					}
				}
			}
		}
	
		if (serial_ports.Count == 0)
			serial_ports.Add("None");
		return serial_ports.ToArray();
	}
}
