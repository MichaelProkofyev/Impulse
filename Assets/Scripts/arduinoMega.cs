using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;

public class arduinoMega : MonoBehaviour {

    public UniOSC.UniOSCMoveGameObject oscReciever;

    string serialPort = "/dev/tty.usbmodem1421";
    public int baudRate = 115200;
    public int OUT_CH = 8;
    [Range (0, 255)] public int[] controls;

    private int[] oldControls;
    private bool updateOutput;
    private bool serialOpened;
    private SerialPort arduinoSerial;     // each port's actual Serial port



    void Start () {

        controls = new int[OUT_CH];
        oldControls = new int[OUT_CH];

        for (int i = 0; i < OUT_CH; i++) {
            controls[i] = new int();
            oldControls[i] = new int();
        }

        serialOpen(serialPort, baudRate);

        // for (int i = 0; i < controls.Length; i++) {
        //     controls[i] = 255;
        // }
        // serialUpdate(controls);
        updateOutput = true;
    }

    bool isZero = false;
    public float delay = 0.1f;
    IEnumerator SwitchLight(){ 
        while(true) {
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < controls.Length; i++) {
                    // controls[i] = isZero ? 0 : 255;
                    controls[i] = (int)(oscReciever.ledValues[i] * 255);
            }
            isZero = !isZero;   
        }
    }

    void Update () {

        if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(SwitchLight());
        }

        

        if (serialOpened) {
            for (int i = 0; i < OUT_CH; i++) {
                if (oldControls[i] != controls[i] && !updateOutput) {
                    oldControls[i] = controls[i];
                    updateOutput = true;
                }
            }
            if (updateOutput) {
                // print("updating");
                serialUpdate(controls);
                updateOutput = false;
            }
        }

    }


    void OnApplicationQuit()
    {
        if (serialOpened)
        {
            serialUpdate(new int[OUT_CH]);
            arduinoSerial.Close();
        }
    }

    void serialOpen(string sPort, int bRate) {
		try {
			arduinoSerial = new SerialPort(sPort, bRate);
			arduinoSerial.Open();
			serialOpened = true;
		}
		catch {
			print("Serial port " + serialPort + " does not exist or is non-functional");
			return;
		}
    }

    void serialUpdate(int[] values)
    {
		// convert the LED image to raw data
		byte[] data = new byte[OUT_CH + 1];

		data[0] = (byte)'*';  // first Teensy is the frame sync master
		for (int i = 0; i < OUT_CH; i++)
		{
            // print("updating i " + values[i]);
			data[i + 1] = (byte)values[i];

		}

		// send the raw data to the arduino
		arduinoSerial.Write(data, 0, data.Length);
    }
}
