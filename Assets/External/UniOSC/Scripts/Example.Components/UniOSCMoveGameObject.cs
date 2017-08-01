/*
* UniOSC
* Copyright © 2014-2015 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using OSCsharp.Data;


namespace UniOSC{

	/// <summary>
	/// Moves a GameObject in normalized coordinates (ScreenToWorldPoint)
	/// </summary>
	[AddComponentMenu("UniOSC/MoveGameObject")]
	public class UniOSCMoveGameObject :  UniOSCEventTarget {

		public float[] ledValues = new float[10];

		[HideInInspector]
		public Transform transformToMove;
		public float nearClipPlaneOffset = 1;
		public enum Mode{Screen,Relative}
		public Mode movementMode;
		//movementModeProp = serializedObject.FindProperty ("movementMode");

		private Vector3 pos;

		void Awake(){

		}


		public override void OnOSCMessageReceived(UniOSCEventArgs args)
		{
			if(transformToMove == null) return;
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
			if(msg.Address == "/laser1") {
				if((int)msg.Data[0] == 1)  {
					int patternID = (int)msg.Data[1];
					float patternGain = Mathf.Clamp01((float)msg.Data[2]);
					Laser.Instance.AddPattern(patternID, patternGain);
				}
			}

			// print(msg.Address);
			// print(msg.Data[0]);
		}

	}

}