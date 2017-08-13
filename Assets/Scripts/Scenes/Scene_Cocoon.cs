using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neuron;
using System.Linq;
public class Scene_Cocoon : SingletonComponent<Scene_Cocoon> {

    public bool sendUpdates = false;

    public float[] brightness = new float[10];
    public Vector2 offsetClamp = Vector2.zero;
    public Vector2[] circlesPositions = new Vector2[17];

    public Vector2 OSC_cocoonScale = Vector2.zero;

    public Vector2 OSC_cocoonOffset = Vector2.zero;
    public Vector2 OSC_offsetSpeed = Vector2.zero;

    public Neuron.NeuronTransformsInstance skeleton;


    public float transformMultiplierX = 1f;
    public float transformMultiplierY = 1f;
    public Vector3 transformClampMin, transformClampMax;

    public Vector2[] skeletonPoints = new Vector2[17];

    public Vector2 pointsOffset, pointsRotation;

    

    Vector2[] linesConnections = new Vector2[10] {
        new Vector2(0, 1),
        new Vector2(0, 13),
        new Vector2(0, 15),
        new Vector2(0, 16),
        new Vector2(1, 13),
        new Vector2(1, 15),
        new Vector2(1, 16),
        new Vector2(13, 15),
        new Vector2(13, 16),
        new Vector2(15, 16)
    };

    public float randomChangeSpeed = 1f;

    // Use this for initialization
    void Start () {
//        StartCoroutine(ChangeLines());

    }

    public void InitScene()
    {
        for (int i = 0; i < brightness.Length; i++) {
            brightness[i] = .1f;
        }
        sendUpdates = true;
    }

    public void DisableScene()
    {
        for (int i = 0; i < brightness.Length; i++)
        {
            brightness[i] = 0;
        }
        Laser.Instance.ClearLines();
        sendUpdates = false;
    }


    public Transform meshRoot;
    // Update is called once per frame
    void Update () {
        if (!sendUpdates) return;
       // meshRoot.position = new Vector3(0, meshRoot.position.y, 0);


        OSC_cocoonOffset +=  new Vector2(Mathf.Clamp(OSC_offsetSpeed.x, -offsetClamp.x, offsetClamp.x), Mathf.Clamp(OSC_offsetSpeed.y, -offsetClamp.y, offsetClamp.y)) * Time.deltaTime;

        skeletonPoints[0] = skeleton.lHandT.position;
        skeletonPoints[1] = skeleton.rHandT.position;
        skeletonPoints[2] = skeleton.lForeArmT.position;
        skeletonPoints[3] = skeleton.rForeArmT.position;
        skeletonPoints[4] = skeleton.lArmT.position;
        skeletonPoints[5] = skeleton.rArmT.position;
        skeletonPoints[6] = skeleton.lShoulderT.position;
        skeletonPoints[7] = skeleton.rShoulderT.position; 
        skeletonPoints[8] = skeleton.hips.position;
        skeletonPoints[9] = skeleton.spineT.position;
        skeletonPoints[10] = skeleton.headT.position;
        skeletonPoints[11] = skeleton.lUpLegT.position;
        skeletonPoints[12] = skeleton.rUpLegT.position;
        skeletonPoints[13] = skeleton.lLegT.position;
        skeletonPoints[14] = skeleton.rLegT.position;
        skeletonPoints[15] = skeleton.lFootT.position;
        skeletonPoints[16] = skeleton.rFootT.position;

        for (int i = 0; i < skeletonPoints.Length; i++) {
            skeletonPoints[i] -= (Vector2)meshRoot.position;
        }

        for (int i = 0; i < circlesPositions.Length; i++)
        {
            circlesPositions[i] = transformPostion(skeletonPoints[i]);
        }



        for (int i = 0; i < linesConnections.Length; i++) {
            Laser.Instance.AddLineData(
                laserIdx: 1,
                patternID: i,
                startPoint: circlesPositions[(int)linesConnections[i].x],
                endPoint: circlesPositions[(int)linesConnections[i].y],
                brightness:(ushort)(brightness[i] * CONST.LASER_MAX_VALUE),
                wobble: 0
                );
        }
    }

    IEnumerator ChangeLines() {
        while(true){
            for (int i = 0; i < linesConnections.Length; i++) {
                linesConnections[i].x = CONST.RRangeInt(0, circlesPositions.Length - 1);
                linesConnections[i].y = CONST.RRangeInt(0, circlesPositions.Length - 1);
            }
            yield return new WaitForSeconds(randomChangeSpeed);
        }
    }

    private int[] lastShuffle = new int[17] { 0, 1, 13, 15, 16, 0,0,0,0,0,0,0,0,0,0,0,0 };
    public void RandomizeLines()
    {
        int[] shuffle = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16 }.OrderBy(n => System.Guid.NewGuid()).ToArray();

        int firstChange = Random.Range(0, 5);
     //   int secondChange = Random.Range(0, 5);

        lastShuffle[firstChange] = shuffle[0];
     //   lastShuffle[secondChange] = shuffle[1];

        linesConnections = new Vector2[10] {
            new Vector2(lastShuffle[0], lastShuffle[1]),
            new Vector2(lastShuffle[0], lastShuffle[2]),
            new Vector2(lastShuffle[0], lastShuffle[3]),
            new Vector2(lastShuffle[0], lastShuffle[4]),
            new Vector2(lastShuffle[1], lastShuffle[2]),
            new Vector2(lastShuffle[1], lastShuffle[3]),
            new Vector2(lastShuffle[1], lastShuffle[4]),
            new Vector2(lastShuffle[2], lastShuffle[3]),
            new Vector2(lastShuffle[2], lastShuffle[4]),
            new Vector2(lastShuffle[3], lastShuffle[4])
        };
    }


    public void ResetLines() {
        linesConnections = new Vector2[10] {
            new Vector2(0, 1),
            new Vector2(0, 13),
            new Vector2(0, 15),
            new Vector2(0, 16),
            new Vector2(1, 13),
            new Vector2(1, 15),
            new Vector2(1, 16),
            new Vector2(13, 15),
            new Vector2(13, 16),
            new Vector2(15, 16)

        };
    }

    Vector2 transformPostion(Vector3 position) {
        position.x *= (transformMultiplierX + OSC_cocoonScale.x);
        position.y *= (transformMultiplierY +  OSC_cocoonScale.y);

        Vector2 result = position; //new Vector2(Mathf.Clamp(position.x, transformClampMin.x, transformClampMax.x), Mathf.Clamp(position.y, transformClampMin.y, transformClampMax.y ));
        result +=  pointsOffset + OSC_cocoonOffset;
        result = new Vector2(Mathf.Clamp(OSC_offsetSpeed.x, -offsetClamp.x, result.x), Mathf.Clamp(OSC_offsetSpeed.y, -offsetClamp.y, result.y));
        return CONST.RotatePoints(new Vector2[] { result }, pointsRotation)[0];
    }

    public void OSCLinesData(int patternID, float newBrightness)
    {
        if (patternID < 10)
        {
            brightness[patternID] = newBrightness;
        }
        else {
            Debug.LogWarning("Scene Cocoon, Unknown line ID: " + patternID);
        }
    }
}
