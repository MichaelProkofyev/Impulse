using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : LaserTaskBase
{

    new const LASERPATTERN type  = LASERPATTERN.DOT;
	int pointsCount = CONST.pointsPerPattern[type];

    Vector2 currentPoint;
    Vector2 direction;
    public float speed = 0;

    public Dot(Vector2 newStartPoint, Vector2 direction, ushort brightness = CONST.LASER_MAX_VALUE, float newSpeed = 5) : base(newStartPoint, brightness)
    {
        this.currentPoint = startPoint;
        this.direction = direction;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {   
        currentPoint += direction * deltaTime * speed;
        currentPoint += UnityEngine.Random.insideUnitCircle *0.01f;
        startPoint += UnityEngine.Random.insideUnitCircle *0.01f;
        //Debug.Log("START: " + startPoint + " CURR: " + currentPoint);
        //Debug.Log(brightness);
        Vector2[] points = new Vector2[pointsCount];
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
            Vector2 newPoint = Vector2.Lerp(startPoint, currentPoint, (float)pIdx / pointsCount);
            newPoint.x *= Laser.Instance.multx;
            newPoint.y *= Laser.Instance.multy;
            points[pIdx] = newPoint;
            // Debug.Log("X: " + newPoint.x + " Y: " + newPoint.y);
        }
        return points;
    }
}
