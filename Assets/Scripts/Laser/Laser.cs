using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    LineRenderer laserLine;

    List<LaserTaskBase> tasks = new List<LaserTaskBase>();
    int pointsCount = 100;

    public float sizeMultiplier = 1;

    public float multy;
    public float multx;

    public List<List<RCPoint>> currentPoints = new List<List<RCPoint>>(); 



	// Use this for initialization
	void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        laserLine = GetComponent<LineRenderer>();

        //  LaserTaskBase lineTask = new LineTask(new Vector3(0f, -2f, 0f), 4f, 0);
        // LaserTaskBase sinTask = new SinTask(Vector3.zero, 4f, 0);
        ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle/2f, .1f, 1);
        tasks.Add(expLineTask);
    }
	
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle / 2f, .1f, 1);
            tasks.Add(expLineTask);
        }
        currentPoints.Clear();
        for (int taskIdx = 0; taskIdx < tasks.Count; taskIdx++) {
            List<RCPoint> shapePoints = new List<RCPoint>();
            Vector2[] pointsPositions = tasks[taskIdx].NextPoints(pointsCount);

            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++) {
                RCPoint newPoint;
                newPoint.x = (short)(pointsPositions[pIdx].x * 32767 * sizeMultiplier);
                newPoint.y = (short)(pointsPositions[pIdx].y * 32767 * sizeMultiplier);
                newPoint.red = 65535;
                newPoint.green = 0;
                newPoint.blue = 0;

                newPoint.intensity = RayComposerDraw.intensity;
                newPoint.user1 = RayComposerDraw.user1;
                newPoint.user2 = RayComposerDraw.user2;


                shapePoints.Add(newPoint);
            }
            currentPoints.Add(shapePoints);

            if (tasks[taskIdx].isFinished) {
                tasks.RemoveAt(taskIdx);

                ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle / 2f, Random.Range(.1f, 2), 1);
                tasks.Add(expLineTask);
            }


        }
    }

    // Update is called once per frame
    //void UpdateLegacy () {
    //    if (currTaskIdx >= tasks.Count) return;  //If no tasks left - do nothing


    //    Vector3 startPosition = transform.position;
    //    Vector3 endPosition = tasks[currTaskIdx].NextPoints(); // startPosition + Random.insideUnitSphere * 5f;



    //    laserLine.SetPosition(0, startPosition);


    //    Vector3 direction = endPosition - startPosition;

    //    float raycastDistance = Vector3.Distance(endPosition, startPosition);
    //    laserLine.positionCount = 2;
    //    laserLine.SetPosition(1, endPosition);


    //    //Move to the next task, if needed
        //if(tasks[currTaskIdx].isFinished) {
        //    currTaskIdx++;
        //}
    //}
}
