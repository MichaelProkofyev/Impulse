using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : LaserTaskBase
{
    public Vector2 currentPoint;
    public Vector2 direction;
    public float speed = 0;
    public bool showTrace = true;
    public PATTERN stickToPattern = 0;

    public int size = 4;

    public Dot(Vector2 startPoint) : base()
    {
        this.type = PATTERN.DOT;
        this.pointsCount = Laser.Instance.dotPoints;
        this.startPoint = startPoint;
        this.currentPoint = startPoint;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {   
        currentPoint += direction * deltaTime * speed;
//        Debug.Log(startPoint);
        Vector2[] points;
        if (showTrace) {
            points = new Vector2[pointsCount];
            for (int pIdx = 0; pIdx < pointsCount; pIdx++) {

                Vector2 newPoint = Vector2.Lerp(startPoint, currentPoint, (float)pIdx / pointsCount);
                points[pIdx] = newPoint;
                // Debug.Log("X: " + newPoint.x + " Y: " + newPoint.y);
            }
        }else {
            points = new Vector2[2] { currentPoint, currentPoint };
            //Debug.Log(currentPoint);
        }
       // Debug.Log("STARTT: " + startPoint + " CURRENT " + currentPoint);
        return points;
    }
}