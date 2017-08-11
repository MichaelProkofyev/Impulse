using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neuron;
using System.Linq;
public class CirclesTest : SingletonComponent<CirclesTest> {


	public float radius = 1f;
	public Vector3 rotationSpeed = Vector3.zero;
    public Vector2[] circlesPositions = new Vector2[17];

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

    public int numberOfLines = 15;


    // Use this for initialization
    void Start () {
//        StartCoroutine(ChangeLines());

    }

    // Update is called once per frame
    void Update () {


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

        Vector2 lArmPos = transformPostion(skeletonPoints[0]);
        Vector2 rArmPos = transformPostion(skeletonPoints[1]);

        Vector2 lFootTransform = transformPostion(skeletonPoints[15]);
        Vector2 rFootTransform = transformPostion(skeletonPoints[16]);


        for (int i = 0; i < circlesPositions.Length; i++)
        {
            circlesPositions[i] = transformPostion(skeletonPoints[i]);
        }

        //ADD CIRCLES
	    for (int i = 0; i < circlesPositions.Length; i++) {
            if (i == 0 || i == 1 || i == 13 || i == 14 || i ==15 || i == 16)
            {
                break;
		        Laser.Instance.AddCircleData(
                laserIdx: 1,
                patternID: i,
                brightness: CONST.LASER_MAX_VALUE,
                wobble: 0,
                rotation_speed: rotationSpeed,
                pointsMultiplier: 1,
                center: circlesPositions[i],//Vector2.one - new Vector2(0.1f * i, 0.1f * i),
                radius: radius);   
            }
	    }

        //for (int i = 0; i < circlesPositions.Length; i++)
        //{
        //    Laser.Instance.AddDotData(
        //    laserIdx: 1,
        //    patternID: i,
        //    brightness: CONST.LASER_MAX_VALUE,
        //    wobble: 0,
        //    center: circlesPositions[i],//Vector2.one - new Vector2(0.1f * i, 0.1f * i),
        //    radius: radius);
        //}

        //ADD LINES



        for (int i = 0; i < numberOfLines; i++) {
            Laser.Instance.AddLineData(
                laserIdx: 1,
                patternID: i,
                startPoint: circlesPositions[(int)linesConnections[i].x],
                endPoint: circlesPositions[(int)linesConnections[i].y],
                brightness: CONST.LASER_MAX_VALUE,
                wobble: 0
                );
        }
    }

    IEnumerator ChangeLines() {
        Vector2[] points = new Vector2[4] { new Vector2(-0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0, .5f), new Vector2(0, -.5f) };
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
        position.x *= transformMultiplierX;
        position.y *= transformMultiplierY;

        Vector2 result = new Vector2(Mathf.Clamp(position.x, transformClampMin.x, transformClampMax.x), Mathf.Clamp(position.y, transformClampMin.y, transformClampMax.y ));
        result += pointsOffset;
        return CONST.RotatePoints(new Vector2[] { result }, pointsRotation)[0];
    }
}
