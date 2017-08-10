using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neuron;
public class CirclesTest : MonoBehaviour {

	public int numberOfCircles = 0;
	public float radius =1f;

	public Vector3 rotationSpeed = Vector3.zero;

    public Vector2[] circlesPositions = new Vector2[17];


    public Neuron.NeuronTransformsInstance skeleton;


    public float transformMultiplierX = 1f;
    public float transformMultiplierY = 1f;
    public Vector3 transformClampMin;
    public Vector3 transformClampMax;

    public Vector2[] skeletonPoints = new Vector2[17];

    public Vector2 pointsOffset;
    public Vector2 pointsRotation;

    public Vector2[] linesConnections = new Vector2[4]; 


    // Use this for initialization
    void Start () {
        StartCoroutine(ChangeLines());

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.D)) {
            StartCoroutine(ChangeLines());
        }

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

        //ADD LINES



        for (int i = 0; i < 1; i++)
        {
            Laser.Instance.AddLineData(
                laserIdx: 1,
                patternID: i,
                startPoint: circlesPositions[(int)linesConnections[0].x],
                endPoint: circlesPositions[(int)linesConnections[0].y],
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
            yield return new WaitForSeconds(.1f);
        }
    }

    Vector2 transformPostion(Vector3 position) {
        position.x *= transformMultiplierX;
        position.y *= transformMultiplierY;

        Vector2 result = new Vector2(Mathf.Clamp(position.x, transformClampMin.x, transformClampMax.x), Mathf.Clamp(position.y, transformClampMin.y, transformClampMax.y ));
        result += pointsOffset;
        return CONST.RotatePoints(new Vector2[] { result }, pointsRotation)[0];
    }
}
