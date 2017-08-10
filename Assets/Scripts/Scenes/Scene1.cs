using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : SingletonComponent<Scene1> {

    public float[] dotsBr = new float[4];


    public float[] circlesBr = new float[7];
    public Vector3[] circlesRotationSpeed = new Vector3[7];
    public float[] circlesRadius = new float[7] { 1, 1, 1, 1, 0.2f , 0.2f, 0.2f};
    public float[] circlesWobble = new float[7];
    public float[] circlesPointsMultiplier = new float[7] { 1, 1, 1, 1 ,1,1,1};




    public bool sendData = false;

	public void InitScene() {
        circlesBr[0] = 0.001f;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            sendData = !sendData;
        }
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
               center: Vector2.zero,
               radius: circlesRadius[circleIdx]
            );
        }

    }


    public void OSCDotData(int laserIdx, int patternID, float brightness) {
        if (patternID < 4) {
            dotsBr[patternID] = brightness;
            OnValidate();
        } {
            Laser.Instance.AddDotData(
               laserIdx: 1,
               patternID: patternID,
               brightness: (ushort)(Mathf.Clamp01(brightness) * CONST.LASER_MAX_VALUE),
               wobble: 0,
               speed: 0f,
               showTrace: false,
               stickToPattern: PATTERN.CIRCLE
           );
        }
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
