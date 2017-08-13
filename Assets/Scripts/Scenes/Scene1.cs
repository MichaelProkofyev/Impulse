using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : SingletonComponent<Scene1> {

    public enum SUBSCENE {
         NONE_0,
         FOUR_DOTS_1,
         DOTS_IN_CIRCLE_2,
         CIRCLES_3,
         CIRCLES_FOCUS_4
    }

    public SUBSCENE currentSubscene = SUBSCENE.NONE_0;

    public float[] dotsBr = new float[4];


    public float[] circlesBr = new float[7];
    public Vector3[] circlesRotationSpeed = new Vector3[7];
    public float[] circlesRadius = new float[7] { 1, 1, 1, 1, 0.2f , 0.2f, 0.2f};
    public float[] circlesWobble = new float[7];
    public float[] circlesPointsMultiplier = new float[7] { 1, 1, 1, 1 ,1,1,1};
    public Vector2[] circlesCenter = new Vector2[7];

    public bool sendData = false;

	public void InitScene(SUBSCENE subscene) {
        for (int i = 0; i < dotsBr.Length; i++)
        {
            dotsBr[i] = 0;
        }
        switch (subscene)
        {
            case SUBSCENE.NONE_0:
                for (int i = 0; i < circlesBr.Length; i++) {
                    circlesBr[i] = 0;
                }
                //sendData = false;
                break;
            case SUBSCENE.FOUR_DOTS_1:
                for (int i = 0; i < circlesBr.Length; i++) {
                    circlesBr[i] = 0;
                }

                sendData = true;
                break;
            case SUBSCENE.DOTS_IN_CIRCLE_2:
                Laser.Instance.ClearDots();
                for (int i = 0; i < circlesBr.Length; i++)
                {
                    circlesBr[i] = 0;
                }
                circlesBr[0] = 0.01f;
                break;
            case SUBSCENE.CIRCLES_3:
                for (int i = 0; i < circlesBr.Length; i++) {
                    circlesBr[i] = 0;
                }
                circlesBr[0] = 1f;
                circlesBr[1] = 1f;
                circlesBr[2] = 1f;
                circlesBr[3] = 1f;
                
                break;
            case SUBSCENE.CIRCLES_FOCUS_4:
                for (int i = 0; i < circlesBr.Length; i++) {
                    circlesBr[i] = 0;
                }
                circlesBr[4] = 1f;
                circlesBr[5] = 1f;
                circlesBr[6] = 1f;
                break;
            default:
                Debug.LogWarning("UNKNOWN SUBSCENE");
                break;
        }
        currentSubscene = subscene;

        OnValidate();
    }
	
	// Update is called once per frame
	void Update () {        
    }

    void OnValidate()
    {

        if (!sendData) return;

        //DOTS
        for (int dotIdx = 0; dotIdx < dotsBr.Length; dotIdx++)
        {
            ushort brightness = (ushort)(Mathf.Clamp01(dotsBr[dotIdx]) * CONST.LASER_MAX_VALUE);
            Laser.Instance.AddDotData(
               laserIdx: 1,
               patternID: dotIdx,
               brightness: brightness,
               wobble: 0,
               speed: 0f,
               showTrace: false,
               stickToPattern: PATTERN.CIRCLE
           );
        }

        //CIRCLES
        for (int circleIdx = 0; circleIdx < circlesBr.Length; circleIdx++)
        {
            ushort brightness = (ushort)(Mathf.Clamp01(circlesBr[circleIdx]) * CONST.LASER_MAX_VALUE);

            Laser.Instance.AddCircleData(
               laserIdx: 1,
               patternID: circleIdx,
               brightness: brightness,
               wobble: circlesWobble[circleIdx],
               rotation_speed: circlesRotationSpeed[circleIdx],
               pointsMultiplier: circlesPointsMultiplier[circleIdx],
               center: circlesCenter[circleIdx],
               radius: circlesRadius[circleIdx]
            );
        }

    }


    public void OSCDotData(int laserIdx, int patternID, float brightness) {
        //if (patternID < 4) {
        //    dotsBr[patternID] = brightness;
        //    OnValidate();
        //} {
            Laser.Instance.AddDotData(
               laserIdx: 1,
               patternID: patternID,
               brightness: (ushort)(Mathf.Clamp01(brightness) * CONST.LASER_MAX_VALUE),
               wobble: 0,
               speed: 0f,
               showTrace: false,
               stickToPattern: PATTERN.CIRCLE
           );
        //}
    }

    public void OSCCircleData(int laserIdx, int patternID, float brightness) {
        if (patternID < 7) {
            circlesBr[patternID] = brightness;
            OnValidate();
        }else {
            Debug.LogWarning("Scene 1, Unknown circleID: " + patternID);
        }
    }
}
