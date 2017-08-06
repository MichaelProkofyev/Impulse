using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : LaserTaskBase
{
    Vector2 currentPoint;
    Vector2 direction;
    public float speed = 0;
    public bool showTrace = true;
    public LASERPATTERN stickToPattern = 0;

    public int size = 4;

    public Dot(Vector2 newStartPoint, Vector2 direction, ushort brightness = CONST.LASER_MAX_VALUE, float newSpeed = 5) : base(newStartPoint, brightness)
    {
        this.type = LASERPATTERN.DOT;
        this.pointsCount = CONST.pointsPerPattern[type];
        this.currentPoint = startPoint;
        this.direction = direction;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {   
        currentPoint += direction * deltaTime * speed;
        Vector2[] points;
        if (showTrace) {
            points = new Vector2[ pointsCount];
            for (int pIdx = 0; pIdx < pointsCount; pIdx++) {

                Vector2 newPoint = Vector2.Lerp(startPoint, currentPoint, (float)pIdx / pointsCount);
                points[pIdx] = newPoint;
                // Debug.Log("X: " + newPoint.x + " Y: " + newPoint.y);
            }
        }else {
            points = new Vector2[2] { currentPoint, currentPoint };
            //Debug.Log(currentPoint);
        }
        return points;
    }
}