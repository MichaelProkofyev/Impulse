using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2 : SingletonComponent<Scene1> {
    public float[] circlesBr = new float[3];
    public Vector2[] circlesCurrentPos = new Vector2[3] { Vector2.zero, Vector2.zero, Vector2.zero};
    public Vector2[] circlesTargetPos = new Vector2[3] { Vector2.up, Vector2.zero, Vector2.down};
    public Vector3[] circlesRotationSpeed = new Vector3[3];
    public float[] circlesRadius = new float[3] { 0.2f , 0.2f, 0.2f};
    public float[] circlesWobble = new float[3];
    public float[] circlesPointsMultiplier = new float[3] {1,1,1};


    public Vector2[] hoverTargetPositions = new Vector2[3];
    public float hoverAmplitude = 1;

    public bool sendData = false;

    IEnumerator ShiftCirclesToStage2() {
        float progress = 0f;
        float speed = 5f;
        Vector2[] startPostions = circlesCurrentPos;
        while (progress < 1f) {
            progress += Time.deltaTime * speed;
            for (int circleIdx = 0; circleIdx < startPostions.Length; circleIdx++) {
                circlesCurrentPos[circleIdx] = Vector2.Lerp(startPostions[circleIdx], circlesTargetPos[circleIdx], progress);
            }
            OnValidate();
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(HoverCircles());
    }

    IEnumerator HoverCircles() {
        
        float speed = 1f;
        while (true) {
            float progress = 0f;
            Vector2[] startPostions = circlesCurrentPos;
            hoverTargetPositions = new Vector2[startPostions.Length];
            for (int i = 0; i < hoverTargetPositions.Length; i++) {
                hoverTargetPositions[i].x = startPostions[i].x + Random.Range(-hoverAmplitude, hoverAmplitude);
                hoverTargetPositions[i].y = startPostions[i].y;
            }
            while (progress < 1f) {
                progress += Time.deltaTime * speed;
                for (int circleIdx = 0; circleIdx < startPostions.Length; circleIdx++) {
                    circlesCurrentPos[circleIdx] = Vector2.Lerp(startPostions[circleIdx], hoverTargetPositions[circleIdx], progress);
                }
                OnValidate();
                yield return new WaitForEndOfFrame();
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.Space)) {
            sendData = !sendData;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            StartCoroutine(ShiftCirclesToStage2());
        }
    }

    void OnValidate()
    {

        if (!sendData) return;

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
               center: circlesCurrentPos[circleIdx],
               radius: circlesRadius[circleIdx]
            );
        }

    }

    public void OSCCircleData(int laserIdx, int patternID, float brightness) {
        circlesBr[patternID] = brightness;
        OnValidate();
    }
}
