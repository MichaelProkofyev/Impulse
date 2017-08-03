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


    public void AddCircleData(int patternID, ushort brightness, Vector3 rotation_speed_fraction) {
        Circle circlePattern;
        if(patterns.ContainsKey(patternID)) {
            circlePattern = (Circle)patterns[patternID];
        }else { //NO PATTERN FOR ID, CREATE IT
            circlePattern = new Circle(Vector2.zero); 
            patterns.Add(patternID, circlePattern);
        }
        circlePattern.brightness = brightness;
        circlePattern.rotation_speed = rotation_speed_fraction * Const.circle_max_rotation_speed;
    }

	void Start () {
        #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        //DEBUG
        for (int cIdx = 0; cIdx < 5; cIdx++) {
            AddCircleData(cIdx, (ushort)(0.1f * cIdx * 65500), Vector3.one * 10 * cIdx);
        }
    }
	
    void Update() {
        points.Clear();

        List<int> finishedPatternIDs = new List<int>();
        foreach (int patternID in patterns.Keys){
            List<RCPoint> shapePoints = new List<RCPoint>();
            Vector2[] pointsPositions = patterns[patternID].NextPoints(Time.deltaTime);


            for (int pIdx = 0; pIdx < pointsPositions.Length; pIdx++)
            {
                RCPoint newPoint;
                newPoint.x = (short)( Mathf.Clamp(pointsPositions[pIdx].x * 32767 * sizeMultiplier, -cut_x, cut_x) );
                newPoint.y = (short)(Mathf.Clamp(pointsPositions[pIdx].y * 32767 * sizeMultiplier, -cut_y, cut_y));
                newPoint.red = patterns[patternID].brightness;
                newPoint.green = 0;
                newPoint.blue = 0;

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
    }
}
