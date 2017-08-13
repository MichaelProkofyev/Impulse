using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3 : SingletonComponent<Scene3> {



    public float[] squaresBr = new float[3];
    public Vector3[] squaresRotationSpeed = new Vector3[3];
    public float[] squaresWobble = new float[3];
    public float[] squaresPointsMultiplier = new float[3] { 1, 1, 1};
    public Vector2[] squaresCenter = new Vector2[3];
    public float[] sideLength = new float[3] { 1, 1, 1 };
    public float[] dashLengths = new float[3];
    public bool sendData = false;

   // public Vector3[] rotaions = new Vector3[3];


	public void InitScene() {
        for (int i = 0; i < squaresBr.Length; i++) {
            squaresBr[i] = 1f;
        }
        sendData = true;
        OnValidate();
    }

    public void DisableScene() {
        for (int i = 0; i < squaresBr.Length; i++)
        {
            squaresBr[i] = 0f;
        }
        Laser.Instance.ClearSquares();
    }
	
	// Update is called once per frame
	void Update () {        
    }

    void OnValidate()
    {

        if (!sendData) return;


        //SQUARES
        for (int squareIdx = 0; squareIdx < squaresBr.Length; squareIdx++)
        {
            ushort brightness = (ushort)(Mathf.Clamp01(squaresBr[squareIdx]) * CONST.LASER_MAX_VALUE);

            Laser.Instance.AddSquareData(
                laserIdx: 1,
                patternID: squareIdx,
                rotation_speed: squaresRotationSpeed[squareIdx],
                center: squaresCenter[squareIdx],
                sideLength: sideLength[squareIdx],
                pointsMultiplier: squaresPointsMultiplier[squareIdx],
                brightness: brightness,
                dashLength: dashLengths[squareIdx],
                wobble: squaresWobble[squareIdx]
                );
        }
    }

    public void OSCSquaresData(int patternID, float brightness) {
        if (patternID < 3) {
            squaresBr[patternID] = brightness;
            OnValidate();
        }else {
            Debug.LogWarning("Scene 3, Unknown squareID: " + patternID);
        }
    }
}
