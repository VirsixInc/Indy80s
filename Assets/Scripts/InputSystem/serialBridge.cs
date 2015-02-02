using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
 
public class serialBridge : MonoBehaviour {
  public string portOne;
  public string portTwo;
  public bool useSecond;

  private Thread thread;
  SerialPort stream;// = new SerialPort("/dev/tty.usbmodem1411", 115200); 
  SerialPort stream2;// = new SerialPort("/dev/tty.usbmodem1411", 115200); 

  int baudRate = 57600;
  int readTimeout = 20;

  private List<int[]> packetQueue = new List<int[]>();


  void Start () {
    try {
      stream = new SerialPort(portOne, baudRate);
      stream.Open(); //Open the Serial Stream.
      stream.ReadTimeout = readTimeout;
      if(useSecond){
        stream2 = new SerialPort(portTwo, baudRate);
        stream2.Open(); //Open the Serial Stream.
        stream2.ReadTimeout = readTimeout;
      }
      thread = new Thread (new ThreadStart (readSerial));
      thread.Start();
    } catch (Exception e) {
      Debug.Log (e.Message);
    }
  }
 
  public void Update (){
    lock (packetQueue) {
      foreach (int[] message in packetQueue) {
        BroadcastMessage ("SerialInputRecieved", message, SendMessageOptions.DontRequireReceiver);
        print(message[0] + "  " + message[1] + "  " + message[2]);
      }
      packetQueue.Clear ();
    }
  }
  private int[] parseIndyData(string stringToParse){
    string[] stringToSend = stringToParse.Split(',');
    int[] valsToSend = new int[3];
    for(int j = 0; j<stringToSend.Length;j++){
      valsToSend[j] = int.Parse(stringToSend[j]);
    }
    return valsToSend;
  }
  private void readSerial(){
    while(stream.IsOpen) {
      try{
        string[] lineToRead = stream.ReadLine().Split('|'); 
        string[] lineToRead2 = new string[3];
        if(useSecond){
          lineToRead2 = stream2.ReadLine().Split('|');
          stream2.BaseStream.Flush(); 
        }
        if (lineToRead != null) {
          lock (packetQueue) {
            for(int i = 0; i<lineToRead.Length;i++){
              packetQueue.Add(parseIndyData(lineToRead[i]));
            }
            if(useSecond){
              for(int i = 0; i<lineToRead2.Length;i++){
                packetQueue.Add(parseIndyData(lineToRead2[i]));
              }
            }
          }
        }
        stream.BaseStream.Flush(); 
      } catch (Exception e) { 
        Debug.Log (e.Message);
        Console.WriteLine (e.Message); 
      }
    }
  }

}
 
