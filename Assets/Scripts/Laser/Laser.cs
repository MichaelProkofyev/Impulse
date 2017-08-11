using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    //public float sizeMultiplier = 1;
    public Vector2 sizeMultiplier = Vector2.one;

    public int additionalPointsAtAnchorCIRCLE, additionalPointsAtAnchorSQUARE;
    public int circleAnchors = 5;

    public ushort cut_x;
    public ushort cut_y;
    private ushort original_cut_x, original_cut_y;
    public Vector3 rgb = Vector3.right;
    public int dotsCount, circlesCount, squaresCount, linesCount;
    public float global_wobble = 0f;


    public int dotPoints = 2;
    public int circlePoints = 70;
    public int squarePoints = 50;
    public int linePoints = 2;

    public List<List<RCPoint>> points1 = new List<List<RCPoint>>(); 

    Dictionary<PATTERN, Dictionary<int, LaserTaskBase>> patterns = new Dictionary<PATTERN, Dictionary<int, LaserTaskBase>>()
    {
        { PATTERN.DOT, new Dictionary<int, LaserTaskBase>() },
        { PATTERN.CIRCLE, new Dictionary<int, LaserTaskBase>() },
        { PATTERN.SQUARE, new Dictionary<int, LaserTaskBase>() },
        { PATTERN.LINE, new Dictionary<int, LaserTaskBase>() }

    };

    public void AddDotData(int laserIdx, int patternID, ushort brightness = CONST.LASER_MAX_VALUE, float wobble = 0, float speed = 0, bool showTrace = true, PATTERN stickToPattern = PATTERN.NONE)
    {
        Dot dotPattern;
        Vector2 startPoint = Vector2.zero;
        if (patterns[PATTERN.DOT].ContainsKey(patternID))
        {
            dotPattern = (Dot)patterns[PATTERN.DOT][patternID];
        }
        else
        { //NO PATTERN FOR ID, CREATE IT
            switch (patternID)
            {
                case 0:
                    startPoint = new Vector2(-.5f, 0);
                    break;
                case 1:
                    startPoint = new Vector2(.5f, 0);
                    break;
                case 2:
                    startPoint = new Vector2(0, -.5f);
                    break;
                case 3:
                    startPoint = new Vector2(0, .5f);
                    break;
                default:
                    if (stickToPattern != PATTERN.NONE) {
                        foreach (int patternKey in patterns[stickToPattern].Keys)
                        {
                            //NEED MULTITHREAD LOCK
                            LaserTaskBase taskStickTo = patterns[stickToPattern][patternKey];
                            startPoint = taskStickTo.currentPoints[CONST.RRangeInt(0, taskStickTo.currentPoints.Length)];
                            break;
                        }
                        if (startPoint == Vector2.zero) startPoint = CONST.RRange2(-.5f, .5f);
                    }else {
                        startPoint = CONST.RRange2(-.5f, .5f);
                    }
                    break;
            }
            
            
            dotPattern = new Dot(startPoint);
            dotPattern.direction = CONST.RRange2(-1, 1);
            dotPattern.showTrace = showTrace;
            dotPattern.stickToPattern = stickToPattern;
            patterns[PATTERN.DOT].Add(patternID, dotPattern);
        }
        dotPattern.brightness = brightness;
        dotPattern.speed = speed;
        dotPattern.wobble = wobble;
    }

    public void AddCircleData(int laserIdx, int patternID, Vector3 rotation_speed, Vector2 center, float wobble = 0, ushort brightness = CONST.LASER_MAX_VALUE, float pointsMultiplier = 1f, float dashLength = 0, float radius = 1) {
        Circle circlePattern;
        if(patterns[PATTERN.CIRCLE].ContainsKey(patternID)) {
            circlePattern = (Circle)patterns[PATTERN.CIRCLE][patternID];
        }else { //NO PATTERN FOR ID, CREATE IT
            circlePattern = new Circle();
            patterns[PATTERN.CIRCLE].Add(patternID, circlePattern);
        }
        
        circlePattern.brightness = brightness;
        circlePattern.dashLength = dashLength;
        circlePattern.rotation_speed = rotation_speed;
        circlePattern.wobble = wobble;
        circlePattern.pointsMultiplier = pointsMultiplier;
        circlePattern.startPoint = center;
        circlePattern.radius = radius;
    }

    public void AddSquareData(int laserIdx, int patternID, Vector3 rotation_speed, Vector2 center, float sideLength = 1f, float pointsMultiplier = 1f, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0, float wobble = 0)
    {
        Square squarePattern;
        if (patterns[PATTERN.SQUARE].ContainsKey(patternID)) {
            squarePattern = (Square)patterns[PATTERN.SQUARE][patternID];
        }
        else { //NO PATTERN FOR ID, CREATE IT
            squarePattern = new Square();
            patterns[PATTERN.SQUARE].Add(patternID, squarePattern);
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

    public void AddLineData(int laserIdx, int patternID, Vector2 startPoint, Vector2 endPoint, ushort brightness = CONST.LASER_MAX_VALUE, float wobble = 0) {
        Line linePattern;
        if (patterns[PATTERN.LINE].ContainsKey(patternID))
        {
            linePattern = (Line)patterns[PATTERN.LINE][patternID];
        }
        else
        { //NO PATTERN FOR ID, CREATE IT
            linePattern = new Line(startPoint, endPoint);
            patterns[PATTERN.LINE].Add(patternID, linePattern);
        }
        linePattern.startPoint = startPoint;
        linePattern.endPoint = endPoint;
        linePattern.brightness = brightness;
        linePattern.wobble = wobble;
    }


    public List<List<RCPoint>> UpdatePatterns1()
    {
        points1.Clear();

        Dictionary<PATTERN, List<int>> finishedPatternIDs = new Dictionary<PATTERN, List<int>>() {
            { PATTERN.DOT, new List<int>() },
            { PATTERN.CIRCLE, new List<int>() },
            { PATTERN.SQUARE, new List<int>() },
            { PATTERN.LINE, new List<int>() },
        };

        foreach (PATTERN patternType in patterns.Keys) {
            foreach (int patternID in patterns[patternType].Keys)
            {
                // print(patternID);
                List<RCPoint> shapePoints = new List<RCPoint>();
                Vector2[] pointsPositions = patterns[patternType][patternID].NextPoints(Time.deltaTime);

                //DASH THINGS
                bool isDashingColor = true;
                float currDashLength = 0f;

                for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++)
                {
                    RCPoint newPoint;
                    newPoint.x = (short)(Mathf.Clamp(pointsPositions[pIdx].x * 32767 * sizeMultiplier.x, -cut_x, cut_x));
                    newPoint.y = (short)(Mathf.Clamp(pointsPositions[pIdx].y * 32767 * sizeMultiplier.y, -cut_y, cut_y));

                    //print("X " + newPoint.x + " Y " + newPoint.y);

                    if (isDashingColor)
                    {

                        newPoint.red = (ushort)(patterns[patternType][patternID].brightness * rgb.x);
                        newPoint.green = (ushort)(patterns[patternType][patternID].brightness * rgb.y);
                        newPoint.blue = (ushort)(patterns[patternType][patternID].brightness * rgb.z);
                    }
                    else
                    {
                        newPoint.red = 0;
                        newPoint.green = 0;
                        newPoint.blue = 0;
                    }

                    if (patterns[patternType][patternID].dashLength != 0)
                    {
                        currDashLength += 1.0f / pointsPositions.Length;

                        if (patterns[patternType][patternID].dashLength <= currDashLength) //END OF DASH 
                        {
                            isDashingColor = !isDashingColor;
                            currDashLength = 0f;
                        }
                    }

                    newPoint.intensity = (ushort)(patterns[patternType][patternID].brightness);
                    newPoint.user1 = RayComposerDraw.user1;
                    newPoint.user2 = RayComposerDraw.user2;

                    //print(newPoint.intensity);

                    shapePoints.Add(newPoint);
                    // print("X " + newPoint.x + " Y " + newPoint.y);
                }
                points1.Add(shapePoints);

                bool shouldDestroyPattern = patterns[patternType][patternID].brightness == 0;
                if (shouldDestroyPattern)
                {
                    finishedPatternIDs[patternType].Add(patternID);
                }

            }
        }

        foreach (PATTERN patternType in patterns.Keys) {
            for (int i = 0; i < finishedPatternIDs[patternType].Count; i++) {
                patterns[patternType].Remove(finishedPatternIDs[patternType][i]);
            }
        }

        dotsCount = patterns[PATTERN.DOT].Count;
        circlesCount = patterns[PATTERN.CIRCLE].Count;
        squaresCount = patterns[PATTERN.SQUARE].Count;
        linesCount = patterns[PATTERN.LINE].Count;





        //if (patterns.Count == 0)
        //{
        //    points1.Clear();
        //    RCPoint newPoint;
        //    newPoint.intensity = RayComposerDraw.intensity;
        //    newPoint.user1 = RayComposerDraw.user1;
        //    newPoint.user2 = RayComposerDraw.user2;
        //    newPoint.x = 0;
        //    newPoint.y = 0;
        //    newPoint.red = 0;
        //    newPoint.green = 0;
        //    newPoint.blue = 0;
        //    List<RCPoint> emptyList = new List<RCPoint>();
        //    emptyList.Add(newPoint);
        //    points1.Add(emptyList);
        //}


        // foreach (var item in points1)
        // {
        //     for (int i = 0; i < item.Count; i++)
        //     {
        //         print(" X "  + item[i].x + " Y " + item[i].y);
        //     }

        // }


        return points1;
    }

    public void ClearPatterns() {
        foreach (PATTERN patternType in patterns.Keys)
        {
            patterns[patternType].Clear();
        }
    }

    public void ClearDots() {
        
        patterns[PATTERN.DOT].Clear();
        
    }

    void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        original_cut_x = cut_x;
        original_cut_y = cut_y;

        // Laser.Instance.AddCircleData(
        //             laserIdx: 1,
        //             patternID: 800,
        //             brightness: CONST.LASER_MAX_VALUE,
        //             wobble: 0,
        //             rotation_speed: Vector3.zero,
        //             pointsMultiplier: 1,
        //             center: Vector2.zero,
        //             radius: 1
        //             );
    }

    void Update() //COOL TRICKS DOWN THERE
    {
        if (Input.GetKey(KeyCode.Space)) {
            sizeMultiplier = CONST.RotatePoints(new Vector2[] { Random.insideUnitCircle }, Random.insideUnitSphere)[0] * Random.Range(-5f, 5f);
        } else if (Input.GetKey(KeyCode.C))
        {
            cut_x = (ushort)Random.Range(-30000, 30000);
            cut_y = (ushort)Random.Range(-30000, 30000);
        } else if (Input.GetKey(KeyCode.L)) {
            CirclesTest.Instance.RandomizeLines();
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            CirclesTest.Instance.ResetLines();
        }
        else
        {
            cut_x = original_cut_x;
            cut_y = original_cut_y;
            sizeMultiplier = Vector2.one *  .5f;
        }
    }
}
