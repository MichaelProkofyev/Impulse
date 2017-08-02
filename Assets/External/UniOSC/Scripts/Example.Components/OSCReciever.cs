using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using OSCsharp.Data;


namespace UniOSC{

	public class OSCReciever :  UniOSCEventTarget {

		public float[] ledValues = new float[10];


		public override void OnOSCMessageReceived(UniOSCEventArgs args)
		{
			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1)return;

			//
			//LED LIGHTS
			//
			if(msg.Address == "/led1") {
				ledValues[0] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led2") {
				ledValues[1] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led3") {
				ledValues[2] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led4") {
				ledValues[3] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led5") {
				ledValues[4] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led6") {
				ledValues[5] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led7") {
				ledValues[6] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led8") {
				ledValues[7] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led9") {
				ledValues[8] = (float)msg.Data[0];
			}else
			if(msg.Address == "/led10") {
				ledValues[9] = (float)msg.Data[0];
			}
			//
			//LASER
			//
			else if(msg.Address == "/laser1") {
				int patternID;
				float patternGain;
				switch ((int)msg.Data[0]) { 
					case 1: //PATTERN - DOTS
						patternID = (int)msg.Data[1];
						patternGain = Mathf.Clamp01((float)msg.Data[2]);
						// Laser.Instance.AddPattern(patternID, patternGain);
					break;
					case 2: //PATTERN - SPINNING CIRCLES
						patternID = (int)msg.Data[1];
						patternGain = Mathf.Clamp01((float)msg.Data[2]);
						float speed_x = Mathf.Clamp01((float)msg.Data[3]);
						float speed_y = Mathf.Clamp01((float)msg.Data[4]);
						float speed_z = Mathf.Clamp01((float)msg.Data[5]);
						Laser.Instance.AddCircleData(patternID, patternGain, new Vector3(speed_x, speed_y, speed_z));
					break;

				}
			} 
			//
			//DMX
			//
			else if(msg.Address == "/dmx1") {
				float recievedValue = Mathf.Clamp01((float)msg.Data[1]);
				// DMXController.Instance.SetValue(i, "brightness", 255);
				DMXController.Instance.SetValue(0, DMXController.Instance.channelToChange, (byte)(255 * recievedValue) );
			}
			else if(msg.Address == "/dmx2") {
				float recievedValue = Mathf.Clamp01((float)msg.Data[1]);
				for (int i = 0; i < 4; i++) {
         	   		// DMXController.Instance.SetValue(i, "brightness", 255);
            		DMXController.Instance.SetValue(1, DMXController.Instance.channelToChange, (byte)(255 * recievedValue) );
        		}
			}
			else if(msg.Address == "/dmx3") {
				float recievedValue = Mathf.Clamp01((float)msg.Data[1]);
				for (int i = 0; i < 4; i++) {
         	   		// DMXController.Instance.SetValue(i, "brightness", 255);
            		DMXController.Instance.SetValue(2, DMXController.Instance.channelToChange, (byte)(255 * recievedValue) );
        		}
			}
			else if(msg.Address == "/dmx4") {
				float recievedValue = Mathf.Clamp01((float)msg.Data[1]);
				for (int i = 0; i < 4; i++) {
         	   		// DMXController.Instance.SetValue(i, "brightness", 255);
            		DMXController.Instance.SetValue(3, DMXController.Instance.channelToChange, (byte)(255 * recievedValue) );
        		}
			}

			// print(msg.Address);
			// print(msg.Data[0]);
		}

	}

}