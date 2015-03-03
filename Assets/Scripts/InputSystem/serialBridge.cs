using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
 
public class SerialBridge : MonoBehaviour {
	[HideInInspector]
	public string portOne, portTwo;

	[HideInInspector]
	public bool useFirst, useSecond;

	private Thread thread;
	SerialPort stream;// = new SerialPort("/dev/tty.usbmodem1411", 115200); 
	SerialPort stream2;// = new SerialPort("/dev/tty.usbmodem1411", 115200); 

	int baudRate = 57600;
	int readTimeout = 200;
	private List<int[]> packetQueue = new List<int[]>();

	[HideInInspector]
	public int port1Index = 0; // For SerialBridgeEditor. I cri evertym.
	[HideInInspector]
	public int port2Index = 0;

	bool cleanStream = true;

	private bool connected = false;
	
	void Start() {
		try {
			if(useFirst) {
				if(portOne == "None")
					return;
				stream = new SerialPort(portOne, baudRate);
				stream.Open(); //Open the Serial Stream.
				stream.ReadTimeout = readTimeout;

			}
			if (useSecond) {
				if(portTwo == "None")
					return;
				stream2 = new SerialPort(portTwo, baudRate);
				stream2.Open(); //Open the Serial Stream.
				stream2.ReadTimeout = readTimeout;
			}
			if(useFirst || useSecond) {
				connected = true;
				thread = new Thread(new ThreadStart(readSerial));
				thread.Start();
			}
		} catch (Exception e) {
			Debug.Log(e.Message);
		}
	}
 
	public void Update() {
		lock (packetQueue) {
			foreach (int[] message in packetQueue) {
				BroadcastMessage("SerialInputRecieved", message);
			}
			packetQueue.Clear();
		}
	}

	private int[] parseIndyData(string stringToParse) {
		string[] stringToSend = stringToParse.Split(',');
		int[] valsToSend = new int[3];
		for (int j = 0; j<stringToSend.Length; j++) {
			valsToSend [j] = int.Parse(stringToSend [j]);
		}
		return valsToSend;
	}

	private void readSerial() {
		while (connected) {
			try {
				if(useFirst) {
					string[] lineToRead = stream.ReadLine().Split('|');
					if (lineToRead != null) {
						lock (packetQueue) {
							for (int i = 0; i < lineToRead.Length; i++) {
								packetQueue.Add(parseIndyData(lineToRead [i]));
							}

						}
					}
					stream.BaseStream.Flush(); 
				}
				if (useSecond) {
					string[] lineToRead2 = stream2.ReadLine().Split('|');
					if (lineToRead2 != null) {
						lock (packetQueue) {
							for (int i = 0; i < lineToRead2.Length; i++) {
								packetQueue.Add(parseIndyData(lineToRead2 [i]));
							}
						}
					}
					stream2.BaseStream.Flush(); 
				}
			} catch (Exception e) { 
				Debug.Log(e.Message);
			}
			if(cleanStream) {
				lock(packetQueue) {
					packetQueue.Clear();
				}
				cleanStream = false;
			}
		}
	}

	public void OnApplicationQuit () {
		if (stream != null) {
			stream.Close ();
		}
		if (stream2 != null) {
			stream2.Close ();
		}
		if (thread != null) {
		    if(thread.IsAlive) {
				connected = false;
				thread.Join();
			}
		}
		
		stream = null;
		stream2 = null;
	}
}
