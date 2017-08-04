using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;

public class LEDController : SingletonComponent<LEDController> {

    int baudRate = 115200;
    [Range (0, 255)] public byte[] ledValues = new byte[CONST.LED_COUNT];

    private byte[] oldControls = new byte[CONST.LED_COUNT];
    private bool updateOutput;
    private bool serialOpened;
    private SerialPort arduinoSerial;     // each port's actual Serial port



    void Start () {
        serialOpen(CONST.LED_SERIAL_PORT, baudRate);
        updateOutput = true;
    }

    IEnumerator SwitchLight(){ 
        while(true) {
            yield return new WaitForSeconds(CONST.LED_MIN_UPDATE_TIME);
            for (int i = 0; i < ledValues.Length; i++) {
                    // controls[i] = isZero ? 0 : 255;
                    //controls[i] = (int)(oscReciever.ledValues[i] * 255);
            }
        }
    }

    void Update () {
        if (serialOpened) {
            for (int i = 0; i < CONST.LED_COUNT; i++) {
                if (oldControls[i] != ledValues[i] && !updateOutput) {
                    oldControls[i] = ledValues[i];
                    updateOutput = true;
                }
            }
            if (updateOutput) {
                // print("updating");
                serialUpdate(ledValues);
                updateOutput = false;
            }
        }

    }


    void OnApplicationQuit()
    {
        if (serialOpened)
        {
            serialUpdate(new byte[CONST.LED_COUNT]);
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
			print("Serial port " + CONST.LED_SERIAL_PORT + " does not exist or is non-functional");
			return;
		}
    }

    void serialUpdate(byte[] values)
    {
		// convert the LED image to raw data
		byte[] data = new byte[CONST.LED_COUNT + 1];

		data[0] = (byte)'*';  // first Teensy is the frame sync master
		for (int i = 0; i < CONST.LED_COUNT; i++)
		{
            // print("updating i " + values[i]);
			data[i + 1] = (byte)values[i];

		}

		// send the raw data to the arduino
		arduinoSerial.Write(data, 0, data.Length);
    }
}
