using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    LineRenderer laserLine;

    List<LaserTaskBase> tasks = new List<LaserTaskBase>();
    int pointsCount = 100;
    public int tasksCount = 0;

    public float sizeMultiplier = 1;

    public float multy;
    public float multx;

    public List<RCPoint> currentPoints = new List<RCPoint>(); 


	// Use this for initialization
	void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        laserLine = GetComponent<LineRenderer>();

      //  LaserTaskBase lineTask = new LineTask(new Vector3(0f, -2f, 0f), 4f, 0);
        LaserTaskBase sinTask = new SinTask(Vector3.zero, 4f, 0);
        tasksCount++;
        tasks.Add(sinTask);
    }
	
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)) {
            LaserTaskBase sinTask = new SinTask(Vector3.one * Random.value, 4f, 0);
            tasks.Add(sinTask);
            tasksCount++;
        }
        currentPoints.Clear();
        for (int taskIdx = 0; taskIdx < tasks.Count; taskIdx++) {
            Vector2[] pointsPositions = tasks[taskIdx].NextPoints(pointsCount);

            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++) {
                RCPoint newPoint;
                newPoint.x = (short)(pointsPositions[pIdx].x * 32767 * sizeMultiplier);
                newPoint.y = (short)(pointsPositions[pIdx].y * 32767 * sizeMultiplier);
                newPoint.red = 65535;
                newPoint.green = 65535;
                newPoint.blue = 65535;

                newPoint.intensity = RayComposerDraw.intensity;
                newPoint.user1 = RayComposerDraw.user1;
                newPoint.user2 = RayComposerDraw.user2;


                currentPoints.Add(newPoint);
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
    //    if(tasks[currTaskIdx].isFinished) {
    //        currTaskIdx++;
    //    }
    //}
}
