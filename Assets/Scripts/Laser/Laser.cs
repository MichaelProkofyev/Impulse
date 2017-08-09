using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    public float sizeMultiplier = 1;

    public int fatness = 1;
    public float fatness_offset_multiplier = 1f;

    public Dictionary<PATTERN, int> additionalPointsAtAnchor = new Dictionary<PATTERN, int> () {
        {PATTERN.DOT, 0},
        {PATTERN.CIRCLE, 5},
        {PATTERN.SQUARE, 50}
    };
    public Dictionary<PATTERN, int> additionalAnchors = new Dictionary<PATTERN, int> () {
        {PATTERN.DOT, 0},
        {PATTERN.CIRCLE, 5},
        {PATTERN.SQUARE, 4}
    };

    public ushort cut_x = 32767;
    public ushort cut_y = 32767;
    public Vector3 rgb = Vector3.right;
    public int patternsCount = 0;

    public float global_wobble = 0f;

    public List<List<RCPoint>> points = new List<List<RCPoint>>(); 
    Dictionary<int, LaserTaskBase> patterns = new Dictionary<int, LaserTaskBase>();


    public void AddCircleData(int patternID, Vector3 rotation_speed, Vector2 center, float wobble = 0, ushort brightness = CONST.LASER_MAX_VALUE, float pointsMultiplier = 1f, float dashLength = 0, float radius = 1) {
        Circle circlePattern;
        if(patterns.ContainsKey(patternID)) {
            circlePattern = (Circle)patterns[patternID];
        }else { //NO PATTERN FOR ID, CREATE IT
            circlePattern = new Circle(); 
            patterns.Add(patternID, circlePattern);
        }
        circlePattern.radius = 1;
        circlePattern.brightness = brightness;
        circlePattern.dashLength = dashLength;
        circlePattern.rotation_speed = rotation_speed;
        circlePattern.wobble = wobble;
        circlePattern.pointsMultiplier = pointsMultiplier;
        circlePattern.startPoint = center;
    }

    public void AddSquareData(int patternID, Vector3 rotation_speed, Vector2 center, float sideLength = 1f, float pointsMultiplier = 1f, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0, float wobble = 0)
    {
        Square squarePattern;
        if (patterns.ContainsKey(patternID)) {
            squarePattern = (Square)patterns[patternID];
        }
        else { //NO PATTERN FOR ID, CREATE IT
            squarePattern = new Square();
            patterns.Add(patternID, squarePattern);
        }
        squarePattern.sideLength = sideLength;
        squarePattern.dashLength = dashLength;
        squarePattern.brightness = brightness;
        squarePattern.pointsMultiplier = pointsMultiplier;
        squarePattern.wobble = wobble;
        squarePattern.rotation_speed = rotation_speed;
        squarePattern.startPoint = center;
        //print(rotation_speed);
    }

    public void AddDotData(int patternID, ushort brightness = CONST.LASER_MAX_VALUE, float wobble = 0, float speed  = 0, bool showTrace = true, PATTERN stickToPattern = PATTERN.NONE)
    {
        Dot dotPattern;
        Vector2 startPoint = Vector2.zero;
        if (patterns.ContainsKey(patternID)) {
            dotPattern = (Dot)patterns[patternID];
        }
        else { //NO PATTERN FOR ID, CREATE IT
            if (stickToPattern != PATTERN.NONE){
                foreach (int patternKey in patterns.Keys)
                {
                    PATTERN type = patterns[patternKey].type;
                    if (stickToPattern == type) {

                        //NEED MULTITHREAD LOCK
                        startPoint = patterns[patternKey].currentPoints[CONST.RRangeInt(0, patterns[patternKey].currentPoints.Length)];
                        print("CLINGED TO");
                        break;
                    }
                }
            }else {

                switch (patternID)
                {
                    case 0:
                        startPoint = new Vector2(-1, 0);
                        break;
                    case 1:
                        startPoint = new Vector2(1, 0);
                        break;
                    case 2:
                        startPoint = new Vector2(0, -1);
                        break;
                    case 3:
                        startPoint = new Vector2(0, 1);
                        break;
                    default:
                        startPoint = CONST.RRange2(-1, 1);
                        break;
                }
            }
            dotPattern = new Dot(startPoint);
            dotPattern.direction = CONST.RRange2(-1, 1);
            dotPattern.showTrace = showTrace;
            dotPattern.stickToPattern = stickToPattern;
            patterns.Add(patternID, dotPattern);
        }
        dotPattern.brightness = brightness;
        dotPattern.speed = speed;
        dotPattern.wobble = wobble;
    }

    public List<List<RCPoint>> UpdatePatterns()
    {
        points.Clear();

        List<int> finishedPatternIDs = new List<int>();;
        foreach (int patternID in patterns.Keys)
        {
            // print(patternID);
            List<RCPoint> shapePoints = new List<RCPoint>();
            Vector2[] pointsPositions = patterns[patternID].NextPoints(Time.deltaTime);

            //DASH THINGS
            bool isDashingColor = true;
            float currDashLength = 0f;

            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++)
            {
                RCPoint newPoint;
                newPoint.x = (short)(Mathf.Clamp(pointsPositions[pIdx].x * 32767 * sizeMultiplier, -cut_x, cut_x));
                newPoint.y = (short)(Mathf.Clamp(pointsPositions[pIdx].y * 32767 * sizeMultiplier, -cut_y, cut_y));
                if (isDashingColor)
                {

                    newPoint.red = (ushort)(patterns[patternID].brightness * rgb.x);
                    newPoint.green = (ushort)(patterns[patternID].brightness * rgb.y);
                    newPoint.blue = (ushort)(patterns[patternID].brightness * rgb.z);
                }
                else
                {
                    newPoint.red = 0;
                    newPoint.green = 0;
                    newPoint.blue = 0;
                }

                if (patterns[patternID].dashLength != 0)
                {
                    currDashLength += 1.0f / pointsPositions.Length;

                    if (patterns[patternID].dashLength <= currDashLength) //END OF DASH 
                    {
                        isDashingColor = !isDashingColor;
                        currDashLength = 0f;
                    }
                }

                newPoint.intensity = (ushort)(patterns[patternID].brightness);
                newPoint.user1 = RayComposerDraw.user1;
                newPoint.user2 = RayComposerDraw.user2;


                shapePoints.Add(newPoint);
                // print("X " + newPoint.x + " Y " + newPoint.y);
            }
            points.Add(shapePoints);

            bool shouldDestroyPattern = patterns[patternID].brightness == 0;
            if (shouldDestroyPattern)
            {
                finishedPatternIDs.Add(patternID);
            }

        }

        //REMOVE PATTERNS THAT ARE FINISHED
        for (int i = 0; i < finishedPatternIDs.Count; i++)
        {
            patterns.Remove(finishedPatternIDs[i]);
            //print("DELETING " + i);
        }

        patternsCount = patterns.Count;


        if (patterns.Count == 0)
        {
            points.Clear();
            RCPoint newPoint;
            newPoint.intensity = RayComposerDraw.intensity;
            newPoint.user1 = RayComposerDraw.user1;
            newPoint.user2 = RayComposerDraw.user2;
            newPoint.x = 0;
            newPoint.y = 0;
            newPoint.red = 0;
            newPoint.green = 0;
            newPoint.blue = 0;
            List<RCPoint> emptyList = new List<RCPoint>();
            emptyList.Add(newPoint);
            points.Add(emptyList);
        }

        return points;
    }

    public void ClearPatterns() {
        patterns.Clear();
    }

    void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif
    }
}
