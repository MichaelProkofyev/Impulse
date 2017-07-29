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

		public float currentValue = 0;
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


		public override void OnEnable()
		{
			base.OnEnable();

			if(transformToMove == null){
				Transform hostTransform = GetComponent<Transform>();
				if(hostTransform != null) transformToMove = hostTransform;
			}
		}


		public override void OnOSCMessageReceived(UniOSCEventArgs args)
		{
			if(transformToMove == null) return;
			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1)return;

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

			

			currentValue = (float)msg.Data[0];
			// print(msg.Address);
			// print(msg.Data[0]);
			return;

			float x = transformToMove.transform.position.x;
			float y =  transformToMove.transform.position.y;
			float z = transformToMove.transform.position.z;

			switch (movementMode) {

			case Mode.Screen:

				y = Screen.height * (float)msg.Data[0];
				
				if(msg.Data.Count >= 2){
					x = Screen.width* (float)msg.Data[1];
				}
				
				pos = new Vector3(x,y,Camera.main.nearClipPlane+nearClipPlaneOffset);
				transformToMove.transform.position = Camera.main.ScreenToWorldPoint(pos);

				break;

			case Mode.Relative:
				z = 0f;
				y =  (float)msg.Data[0];
				if(msg.Data.Count >= 2){
					x =  (float)msg.Data[1];
				}
				if(msg.Data.Count >= 3){
					z =  (float)msg.Data[2];
				}

				pos = new Vector3(x,y,z);
				transformToMove.transform.position += pos; 
				break;

			}


		}

	}

}