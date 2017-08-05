using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : SingletonComponent<Laser> {

    public float sizeMultiplier = 1;
    public float multy = 1;
    public float multx = 1;


    public ushort cut_x = 32767;
    public ushort cut_y = 32767;


    public List<List<RCPoint>> points = new List<List<RCPoint>>(); 
    Dictionary<int, LaserTaskBase> patterns = new Dictionary<int, LaserTaskBase>();


    public void AddCircleData(int patternID, Vector3 rotSpeed, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0, float radius = 1) {
        Circle circlePattern;
        if(patterns.ContainsKey(patternID)) {
            circlePattern = (Circle)patterns[patternID];
        }else { //NO PATTERN FOR ID, CREATE IT
            circlePattern = new Circle(center: Vector2.zero, radius: 1f); 
            patterns.Add(patternID, circlePattern);
        }
        circlePattern.brightness = brightness;
        circlePattern.dashLength = dashLength;
        circlePattern.rotation_speed = rotSpeed * CONST.circle_max_rotation_speed;
    }

    public void AddSquareData(int patternID, Vector3 rotation_speed_fraction, float sideLength = 1f, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0)
    {
        Square squarePattern;
        if (patterns.ContainsKey(patternID)) {
            squarePattern = (Square)patterns[patternID];
        }
        else { //NO PATTERN FOR ID, CREATE IT
            squarePattern = new Square(Vector2.zero, sideLength);
            patterns.Add(patternID, squarePattern);
        }
        squarePattern.dashLength = dashLength;
        squarePattern.brightness = brightness;
        squarePattern.rotation_speed = rotation_speed_fraction * CONST.square_max_rotation_speed;
    }

    public void AddDotData(int patternID, ushort brightness = CONST.LASER_MAX_VALUE, float speed  = 0)
    {
        Dot dotPattern;
        if (patterns.ContainsKey(patternID)) {
            dotPattern = (Dot)patterns[patternID];
        }
        else { //NO PATTERN FOR ID, CREATE IT
            System.Random rand = new System.Random();
            Vector2 endPoint = new Vector2((float)rand.NextDouble() - 1f, (float)rand.NextDouble() - 1f );
            dotPattern = new Dot(new Vector2((float)rand.NextDouble(), (float)rand.NextDouble() ), endPoint);
            patterns.Add(patternID, dotPattern);
        }
        dotPattern.brightness = brightness;
        dotPattern.speed = speed;
    }

    void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif

        //DEBUG
        // for (int cIdx = 0; cIdx < 5; cIdx++) {
        // //    AddCircleData(cIdx, brightness: (ushort)(0.1f * cIdx * 65500), rotSpeed: Random.insideUnitSphere * 10 * cIdx, radius: 1f);
        // AddSquareData(cIdx, rotation_speed_fraction: Random.insideUnitSphere*50, dashLength: 0);
        // }

        // AddCircleData(patternID: 1, rotSpeed: new Vector3(50,0, 0), dashLength: 0.05f);
        // AddSquareData(1, rotation_speed_fraction:new Vector3(0,0, 0), dashLength: 0.05f);
    }
	
    void Update() {
        points.Clear();

        List<int> finishedPatternIDs = new List<int>();
        foreach (int patternID in patterns.Keys){
            List<RCPoint> shapePoints = new List<RCPoint>();
            Vector2[] pointsPositions = patterns[patternID].NextPoints(Time.deltaTime);

            //DASH THINGS
            bool isDashingColor = true;
            float currDashLength = 0f;

            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++)
            {
                RCPoint newPoint;
                newPoint.x = (short)( Mathf.Clamp(pointsPositions[pIdx].x * 32767 * sizeMultiplier, -cut_x, cut_x) );
                newPoint.y = (short)(Mathf.Clamp(pointsPositions[pIdx].y * 32767 * sizeMultiplier, -cut_y, cut_y));

                if(isDashingColor){
                    newPoint.red = patterns[patternID].brightness;
                    newPoint.green = patterns[patternID].brightness;
                    newPoint.blue = patterns[patternID].brightness;
                }
                else{
                    newPoint.red = 0;
                    newPoint.green = 0;
                    newPoint.blue = 0;
                }

                if(patterns[patternID].dashLength != 0)
                {
                    currDashLength += 1.0f / pointsPositions.Length;

                    if (patterns[patternID].dashLength <= currDashLength) //END OF DASH 
                    {
                        isDashingColor = !isDashingColor;
                        currDashLength = 0f;
                    }
                }


                newPoint.intensity = RayComposerDraw.intensity;
                newPoint.user1 = RayComposerDraw.user1;
                newPoint.user2 = RayComposerDraw.user2;


                shapePoints.Add(newPoint);
            }
            points.Add(shapePoints);

            bool shouldDestroyPattern = patterns[patternID].brightness == 0;
            if (shouldDestroyPattern) {
                finishedPatternIDs.Add(patternID);
            }
        }

        //REMOVE PATTERNS THAT ARE FINISHED
        for (int i = 0; i < finishedPatternIDs.Count; i++) {
            patterns.Remove(finishedPatternIDs[i]);
        }

        if(patterns.Count == 0) {
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
    }
}
