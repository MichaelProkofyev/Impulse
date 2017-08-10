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
        {PATTERN.SQUARE, 30}
    };
    public Dictionary<PATTERN, int> additionalAnchors = new Dictionary<PATTERN, int> () {
        {PATTERN.DOT, 0},
        {PATTERN.CIRCLE, 5},
        {PATTERN.SQUARE, 4}
    };

    public ushort cut_x = 32767;
    public ushort cut_y = 32767;
    public Vector3 rgb = Vector3.right;
    public int dotsCount = 0;
    public int circlesCount = 0;
    public int squaresCount = 0;
    public float global_wobble = 0f;

    public List<List<RCPoint>> points1 = new List<List<RCPoint>>(); 

    Dictionary<PATTERN, Dictionary<int, LaserTaskBase>> patterns = new Dictionary<PATTERN, Dictionary<int, LaserTaskBase>>()
    {
        { PATTERN.DOT, new Dictionary<int, LaserTaskBase>() },
        { PATTERN.CIRCLE, new Dictionary<int, LaserTaskBase>() },
        { PATTERN.SQUARE, new Dictionary<int, LaserTaskBase>() }

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
            if (stickToPattern != PATTERN.NONE)
            {
                foreach (int patternKey in patterns[stickToPattern].Keys)
                {
                    //NEED MULTITHREAD LOCK
                    startPoint = patterns[stickToPattern][patternKey].currentPoints[CONST.RRangeInt(0, patterns[stickToPattern][patternKey].currentPoints.Length)];
                    break;
                }
            }
            else
            {

                switch (patternID)
                {
                    case 10:
                        startPoint = new Vector2(-.5f, 0);
                        break;
                    case 11:
                        startPoint = new Vector2(.5f, 0);
                        break;
                    case 12:
                        startPoint = new Vector2(0, -.5f);
                        break;
                    case 13:
                        startPoint = new Vector2(0, .5f);
                        break;
                    default:
                        startPoint = CONST.RRange2(-.5f, .5f);
                        break;
                }
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
        circlePattern.radius = 1;
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



    public List<List<RCPoint>> UpdatePatterns1()
    {
        points1.Clear();

        Dictionary<PATTERN, List<int>> finishedPatternIDs = new Dictionary<PATTERN, List<int>>() {
            { PATTERN.DOT, new List<int>() },
            { PATTERN.CIRCLE, new List<int>() },
            { PATTERN.SQUARE, new List<int>() }
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
                    newPoint.x = (short)(Mathf.Clamp(pointsPositions[pIdx].x * 32767 * sizeMultiplier, -cut_x, cut_x));
                    newPoint.y = (short)(Mathf.Clamp(pointsPositions[pIdx].y * 32767 * sizeMultiplier, -cut_y, cut_y));
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

    void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

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
}
