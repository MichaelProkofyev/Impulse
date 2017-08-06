using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSCsharp.Data;
using UniOSC;

public class OSCReciever : UniOSCEventTarget
{

    static void HandleLaserMessage(IList<object> args) {
        int laserIdx = (int)args[0];
        LASERPATTERN patternType = (LASERPATTERN)args[1];
        int newPatternID = (int)args[2];
        ushort brightness = (ushort)(Mathf.Clamp01((float)args[3]) * CONST.LASER_MAX_VALUE);
        float wobbleMultiplier = (float)args[4];

        Vector3 rotationSpeed = Vector3.zero;
        float pointsMultiplier = 1f;

        switch (patternType) {
            case LASERPATTERN.DOT:
                float speed = (float)args[5];
                float showTrace = (float)args[6];
                float stickToPattern = (float)args[7];

                // print("BRIGHTNESS " +  brightness );
                Laser.Instance.AddDotData (
                        patternID: newPatternID,
                        brightness: brightness,
                        wobble: wobbleMultiplier,
                        speed: speed,
                        showTrace: showTrace == 1f,
                        stickToPattern: (LASERPATTERN)stickToPattern
                      );
                break;
            case LASERPATTERN.CIRCLE:
                rotationSpeed = new Vector3((float)args[5], (float)args[6], (float)args[7]);
                pointsMultiplier = (float)args[8];
                Vector2 center = new Vector2((float)args[9], (float)args[10]);

                Laser.Instance.AddCircleData(
                    patternID: newPatternID,
                    brightness: brightness,
                    wobble: wobbleMultiplier,
                    rotation_speed: rotationSpeed,
                    pointsMultiplier: pointsMultiplier,
                    center: center
                    );
                break;
            case LASERPATTERN.SQUARE:
                rotationSpeed = new Vector3((float)args[5], (float)args[6], (float)args[7]);
                pointsMultiplier = (float)args[8];
                Laser.Instance.AddSquareData(
                        patternID: newPatternID,
                        brightness: brightness,
                        rotation_speed: rotationSpeed,
                        wobble: wobbleMultiplier,
                        pointsMultiplier: pointsMultiplier
                    );
                break;
            default:
                Debug.LogWarning("Recieved unknown laser pattern: " + patternType);
                break;
        }
    }

    static void AddTestSquare(int patternID, IList<object> args)
    {
        float brightnessFraction = Mathf.Clamp01((float)args[0]);
        ushort brightness = (ushort)(brightnessFraction * 65535);

        // print("BRIGHTNESS " +  brightness );
        //   Laser.Instance.AddDotData(patternID: patternID, brightness: brightness, speed: 2f);
        Laser.Instance.AddSquareData(patternID: patternID, rotation_speed: Vector3.zero, brightness: brightness);
    }

    

    static void HandleLEDMessage(int ledIdx, IList<object> args) {
        float brightnessFraction = Mathf.Clamp01((float)args[0]);
        LEDController.Instance.ledValues[ledIdx] = (byte)(brightnessFraction * 255);
    }

    static void HandleDMXMessage(int dmxIdx, IList<object> args) {
        float brightnessFraction = Mathf.Clamp01((float)args[1]);
        byte brightness = (byte)(brightnessFraction * 255);
        DMXController.Instance.SetValue(dmxIdx, DMXController.Instance.channelToChange, brightness);
    }


    public override void OnOSCMessageReceived(UniOSCEventArgs main_args)
    {

        OscMessage msg = (OscMessage)main_args.Packet;
        var args = msg.Data;
        switch (msg.Address){
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
            case "/laser":
                HandleLaserMessage(args);
                break;
            case "/laser_SetRGB":
                Laser.Instance.rgb = new Vector3((float)args[0], (float)args[1], (float)args[2]);
                break;
            case "/laser_wobble":
                Laser.Instance.global_wobble = .02f * (float)args[0];
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
                Debug.LogWarning("Unknown OSC recieved with addr: " + msg.Address);
                break;
        }
    }


}