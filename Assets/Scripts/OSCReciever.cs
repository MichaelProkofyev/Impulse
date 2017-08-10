using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSCsharp.Data;
using UniOSC;

public class OSCReciever : UniOSCEventTarget
{
    static void Handle_SOUND_LaserMessage(IList<object> args) {
        int laserIdx = (int)args[0];
        PATTERN patternType = (PATTERN)args[1];
        int newPatternID = (int)args[2];
        ushort brightness = (ushort)(Mathf.Clamp01((float)args[3]) * CONST.LASER_MAX_VALUE);

        switch (patternType)
        {
            case PATTERN.DOT:
                Laser.Instance.AddDotData(
                        laserIdx: laserIdx,
                        patternID: newPatternID,
                        brightness: brightness,
                        wobble: 0,
                        speed: 0f,
                        showTrace: false,
                        stickToPattern: PATTERN.CIRCLE
                      );
                break;
            case PATTERN.CIRCLE:
                Laser.Instance.AddCircleData(
                            rotation_speed: Vector3.zero,
                            center: Vector2.zero,
                            laserIdx: laserIdx,
                            patternID: newPatternID,
                            brightness: brightness,
                            wobble: 0
                        );
                break;
            case PATTERN.SQUARE:
                break;
        }
    }

    static void HandleLaserMessage(IList<object> args) {
        int laserIdx = (int)args[0];
        PATTERN patternType = (PATTERN)args[1];
        int newPatternID = (int)args[2];
        ushort brightness = (ushort)(Mathf.Clamp01((float)args[3]) * CONST.LASER_MAX_VALUE);
        float wobbleMultiplier = (float)args[4];

        Vector3 rotationSpeed = Vector3.zero;
        float pointsMultiplier = 1f;
        Vector2 center = Vector2.zero;

        switch (patternType) {
            case PATTERN.DOT:
                float speed = (float)args[5];
                float showTrace = (float)args[6];
                float stickToPattern = (float)args[7];

                // print("BRIGHTNESS " +  brightness );
                Laser.Instance.AddDotData (
                        laserIdx: laserIdx,
                        patternID: newPatternID,
                        brightness: brightness,
                        wobble: wobbleMultiplier,
                        speed: speed,
                        showTrace: showTrace == 1f,
                        stickToPattern: (PATTERN)stickToPattern
                      );
                break;
            case PATTERN.CIRCLE:
                rotationSpeed = new Vector3((float)args[5], (float)args[6], (float)args[7]);
                pointsMultiplier = (float)args[8];
                center = new Vector2((float)args[9], (float)args[10]);
                float radius = (float)args[11];

                Laser.Instance.AddCircleData(
                    laserIdx: laserIdx,
                    patternID: newPatternID,
                    brightness: brightness,
                    wobble: wobbleMultiplier,
                    rotation_speed: rotationSpeed / 30f,
                    pointsMultiplier: pointsMultiplier,
                    center: center,
                    radius: radius
                    );


                break;
            case PATTERN.SQUARE:
                rotationSpeed = new Vector3((float)args[5], (float)args[6], (float)args[7]);
                pointsMultiplier = (float)args[8];
                center = new Vector2((float)args[9], (float)args[10]);

                Laser.Instance.AddSquareData(
                        laserIdx: laserIdx,
                        patternID: newPatternID,
                        brightness: brightness,
                        rotation_speed: rotationSpeed,
                        wobble: wobbleMultiplier,
                        pointsMultiplier: pointsMultiplier,
                        center: center
                    );
                break;
            default:
                Debug.LogWarning("Recieved unknown laser pattern: " + patternType);
                break;
        }
    }    

    static void HandleLEDMessage(int ledIdx, IList<object> args) {
        float brightnessFraction = Mathf.Clamp01((float)args[0]);
        LEDController.Instance.ledValues[ledIdx] = (byte)(brightnessFraction * 255);
    }

    static void HandleDMXMessage(IList<object> args) {
        int dmxIdx = (int)args[0];
        DMXCOLOR valueType = (DMXCOLOR)args[1];
        float brightnessFraction = Mathf.Clamp01((float)args[2]);
        byte brightness = (byte)(brightnessFraction * 255);
        DMXController.Instance.SetValue(dmxIdx, valueType, brightness);
    }

    public override void OnOSCMessageReceived(UniOSCEventArgs main_args)
    {

        OscMessage msg = (OscMessage)main_args.Packet;
        var args = msg.Data;
        switch (msg.Address){

            case "/scene":
                SceneController.Instance.CurrentScene = (SCENE)args[0];
                break;
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
            case "/laser_s":
                Handle_SOUND_LaserMessage(args);
                break;
            case "/laser_SetRGB":
                Laser.Instance.rgb = new Vector3((float)args[0], (float)args[1], (float)args[2]);
                break;
            case "/laser_wobble":
                Laser.Instance.global_wobble = .02f * (float)args[0];
                break;
            case "/laser_fatness":
                Laser.Instance.fatness = (int)((float)args[0]*4 + 1);
                break;
            case "/laser_fatness_offset_multiplier":
                Laser.Instance.fatness_offset_multiplier = (float)args[0];
                break;
            case "/laser_clear_patterns":
                Laser.Instance.ClearPatterns();
                break;
            //DMX
            case "/dmx":
                HandleDMXMessage(args);
                break;
            default:
                Debug.LogWarning("Unknown OSC recieved with addr: " + msg.Address);
                break;
        }
    }

}