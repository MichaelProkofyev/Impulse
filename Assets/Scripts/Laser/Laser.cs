using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    //List<LaserTaskBase> patterns = new List<LaserTaskBase>();
    int pointsPerShape = 50;

    public float sizeMultiplier = 1;

    public float multy;
    public float multx;

    public List<List<RCPoint>> currentPoints = new List<List<RCPoint>>(); 
    Dictionary<int, LaserTaskBase> currentPatterns = new Dictionary<int, LaserTaskBase>();


    // public void AddPattern(int patternID, float brightnessFraction) {
    //     if(currentPatterns.ContainsKey(patternID)) {
    //         LaserTaskBase pattern = currentPatterns[patternID];
    //         pattern.brightnessFraction = brightnessFraction;
    //     }else if(!Mathf.Approximately(brightnessFraction,0)) { //NO PATTERN FOR ID, CREATE IT
    //         print("ADDED PATTERN");
    //         float line_duration = 10f;//Random.Range(.1f, .5f);
    //         // ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle / 2f, line_duration, 1);    
    //         // expLineTask.brightnessFraction = brightnessFraction;
    //         // Circle circleTask = new Circle(pointsPerShape, Random.insideUnitCircle / 2f, line_duration, 1);    
    //         // currentPatterns.Add(patternID, circleTask);
    //         //patterns.Add(expLineTask);
    //     }
    // }

    public void AddCircleData(int patternID, float brightnessFraction, Vector3 rotation_speed_fraction) {
        Circle circlePattern;
        if(currentPatterns.ContainsKey(patternID)) {
            circlePattern = (Circle)currentPatterns[patternID];
        }else { //NO PATTERN FOR ID, CREATE IT
            circlePattern = new Circle(Vector2.zero); 
            currentPatterns.Add(patternID, circlePattern);
        }
        circlePattern.brightnessFraction = brightnessFraction;
        circlePattern.rotation_speed = rotation_speed_fraction * Const.circle_max_rotation_speed;
    }


	// Use this for initialization
	void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        // for (int cIdx = 0; cIdx < 5; cIdx++) {
        //     AddCircleData(cIdx, 0.1f * cIdx,  Vector3.one * 10 * cIdx);
        // }

        //  LaserTaskBase lineTask = new LineTask(new Vector3(0f, -2f, 0f), 4f, 0);
        // LaserTaskBase sinTask = new SinTask(Vector3.zero, 4f, 0);


        // ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle/2f, .1f, 1);
        // patterns.Add(expLineTask);

        // StartCoroutine(AddLinesRegularly());
    }

    // IEnumerator AddLinesRegularly() {
    //     while(true) {
    //         yield return new WaitForSeconds(.7f);
    //         float line_duration = Random.Range(1, 2);
    //         ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle / 2f, line_duration, 1);
    //         patterns.Add(expLineTask);
    //     }
    // }
	
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            // ExpandingLine expLineTask = new ExpandingLine(Random.insideUnitCircle / 2f, .1f, 1);
            // Circle cirlceTask = new Circle(Vector2.zero, 5, 0);
            // patterns.Add(expLineTask);
        }
        currentPoints.Clear();


        foreach(KeyValuePair<int, LaserTaskBase> pattern in currentPatterns) {
            List<RCPoint> shapePoints = new List<RCPoint>();
            Vector2[] pointsPositions = pattern.Value.NextPoints(pointsPerShape);

            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++) {
                RCPoint newPoint;
                newPoint.x = (short)(pointsPositions[pIdx].x * 32767 * sizeMultiplier);
                newPoint.y = (short)(pointsPositions[pIdx].y * 32767 * sizeMultiplier);
                newPoint.red = (ushort)(65535 * pattern.Value.brightnessFraction);
                newPoint.green = 0;
                newPoint.blue = 0;

                newPoint.intensity = RayComposerDraw.intensity;
                newPoint.user1 = RayComposerDraw.user1;
                newPoint.user2 = RayComposerDraw.user2;


                shapePoints.Add(newPoint);
            }
            currentPoints.Add(shapePoints);

            if (pattern.Value.brightnessFraction <= 0) {
                currentPatterns.Remove(pattern.Key);
                // print("DEATH");
            }

            // print("IDX " + pattern.Key +  "BRIGHTNESS: " + pattern.Value.brightnessFraction);

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
