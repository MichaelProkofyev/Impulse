using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : LaserTaskBase {

    public float sideLength = 1;
    public float pointsMultiplier = 1f;

    //Vector2[] anchors = new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(1, 1), new Vector2(-1, 1) };

    public Square() : base()
    {
        this.type = PATTERN.SQUARE;
        this.pointsCount = Laser.Instance.squarePoints;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {
        pointsCount = Mathf.CeilToInt(Laser.Instance.squarePoints * pointsMultiplier);
        //throw new NotImplementedException();
        //Vector2[] points = new Vector2[pointsCount];
        List<Vector2> points = new List<Vector2>();
        int usedAnchorsCount = 0;
        float interval = Mathf.Floor(pointsCount / 4.0f);
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {

            if (pIdx < pointsCount / 4.0f) { //SIDE 1
                if (usedAnchorsCount == 0) {
                    for (int j = 0; j < Laser.Instance.additionalPointsAtAnchorSQUARE; j++) {
                        points.Add(new Vector2(-1, -1));    
                    }
                    usedAnchorsCount ++;
                }

                Vector2 newPoint = Vector2.Lerp(new Vector2(-1, -1), new Vector2(1, -1), (float)pIdx / interval);
                points.Add(newPoint);
                
            }
            else if(pIdx < (pointsCount / 4.0f) * 2f){ //SIDE 2
                if (usedAnchorsCount == 1) {
                    for (int j = 0; j < Laser.Instance.additionalPointsAtAnchorSQUARE; j++) {
                        points.Add(new Vector2(1, -1));    
                    }
                    usedAnchorsCount ++;
                }
                float intervalPoints = pIdx - pointsCount / 4.0f;
                Vector2 newPoint = Vector2.Lerp(new Vector2(1, -1), new Vector2(1,1), (float)intervalPoints / interval);
                points.Add(newPoint);
            }
            else if (pIdx < (pointsCount / 4.0f) * 3f) { //SIDE 3
                if (usedAnchorsCount == 2) {
                    for (int j = 0; j < Laser.Instance.additionalPointsAtAnchorSQUARE; j++) {
                        points.Add(new Vector2(1, 1));    
                    }
                    usedAnchorsCount ++;
                }
                float intervalPoints = pIdx - (pointsCount / 4.0f) * 2f;
                Vector2 newPoint = Vector2.Lerp(new Vector2(1, 1), new Vector2(-1, 1), (float)intervalPoints / interval);
                points.Add(newPoint);
            }
            else { //SIDE 4
                if (usedAnchorsCount == 3) {
                    for (int j = 0; j < Laser.Instance.additionalPointsAtAnchorSQUARE; j++) {
                        points.Add(new Vector2(-1, 1));    
                    }
                    usedAnchorsCount ++;
                }
                float intervalPoints = pIdx - (pointsCount / 4.0f) * 3f;
                Vector2 newPoint = Vector2.Lerp(new Vector2(-1, 1), new Vector2(-1, -1), (float)intervalPoints / interval);
                points.Add(newPoint);
            }

            //SCALE
            points[pIdx] *= sideLength;

            //CENTER OFFSET
            points[pIdx] += startPoint;
        }
        points.Add(points[0]);
        return points.ToArray();
    }
}