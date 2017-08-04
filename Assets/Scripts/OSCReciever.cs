using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpOSC;

public class OSCReciever : MonoBehaviour {

    UDPListener oscListener;

    static void HandleLaserMessage(int laserIdx, List<object> args) {
        LASERPATTERN patternType = (LASERPATTERN)args[0];
        int newPatternID = (int)args[1];
        float brightnessFraction = Mathf.Clamp01((float)args[2]);
        ushort brightness = (ushort)(brightnessFraction * 65535);


        switch (patternType) {
            case LASERPATTERN.DOT:

                break;
            case LASERPATTERN.CIRCLE:
                float speedX = Mathf.Clamp01((float)args[3]);
                float speedY = Mathf.Clamp01((float)args[4]);
                float speedZ = Mathf.Clamp01((float)args[5]);
                Laser.Instance.AddCircleData(newPatternID, brightness: brightness, rotSpeed: new Vector3(speedX, speedY, speedZ));
                break;
            default:
                Debug.LogWarning("Recieved unknown laser pattern: " + patternType);
                break;
        }
    }

    static void HandleLEDMessage(int ledIdx, List<object> args) {
        float brightnessFraction = Mathf.Clamp01((float)args[0]);
        LEDController.Instance.ledValues[ledIdx] = (byte)(brightnessFraction * 255);
    }

    static void HandleDMXMessage(int dmxIdx, List<object> args) {
        float brightnessFraction = Mathf.Clamp01((float)args[1]);
        byte brightness = (byte)(brightnessFraction * 255);
        DMXController.Instance.SetValue(dmxIdx, DMXController.Instance.channelToChange, brightness);
    }


    HandleOscPacket OscCallback = delegate (OscPacket packet)
    {
        OscMessage messageReceived = (OscMessage)packet;
        List<object> args = messageReceived.Arguments;

        switch (messageReceived.Address){
            //LED
            case "/led1":
                HandleLEDMessage(0, args);
                break;
            case "/led2":
                HandleLEDMessage(1, args);
                break;
            case "/led3":
                HandleLEDMessage(2, args);
                break;
            case "/led4":
                HandleLEDMessage(3, args);
                break;
            case "/led5":
                HandleLEDMessage(4, args);
                break;
            case "/led6":
                HandleLEDMessage(5, args);
                break;
            case "/led7":
                HandleLEDMessage(6, args);
                break;
            case "/led8":
                HandleLEDMessage(7, args);
                break;
            case "/led9":
                HandleLEDMessage(8, args);
                break;
            case "/led10":
                HandleLEDMessage(9, args);
                break;
            //LASER
            case "/laser1":
                HandleLaserMessage(0, args);
                break;
            //DMX
            case "/dmx1":
                HandleDMXMessage(0, args);
                break;
            case "/dmx2":
                HandleDMXMessage(1, args);
                break;
            case "/dmx3":
                HandleDMXMessage(2, args);
                break;
            case "/dmx4":
                HandleDMXMessage(3, args);
                break;
            default:
                Debug.LogWarning("Unknown OSC recieved with addr: " + messageReceived.Address);
                break;
        }
    };

    // Use this for initialization
    void Start () {
        oscListener = new UDPListener(10000, OscCallback);
    }

    void OnApplicationQuit()
    {
        oscListener.Close();
    }

}