using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : MonoBehaviour {

    public float[] dotsBr = new float[4];


    public float[] circlesBr = new float[4];
    public Vector3[] circlesRotationSpeed = new Vector3[4];
    public float[] circlesRadius = new float[4] { 1, 1, 1, 1 };
    public float[] circlesWobble = new float[4];
    public float[] circlesPointsMultiplier = new float[4] { 1, 1, 1, 1 };




    public bool sendData = false;

	// Use this for initialization
	void Start () {
		
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
}
