using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingLine : LaserTaskBase
{

    new const LASERPATTERN type  = LASERPATTERN.DOT;
	int pointsCount = CONST.pointsPerPattern[type];

    Vector2 endPoint;

    public ExpandingLine(Vector2 newStartPoint, ushort brightness = CONST.LASER_MAX_VALUE, float newSpeed = 5) : base(newStartPoint, brightness)
    {
        endPoint = startPoint;// + UnityEngine.Random.insideUnitCircle / 2f;
      //  print("START POINT: " + startPoint);
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {
        //LINE IS NOT MAKING PROGRESS, THINK HOW TO FIX AND UPDATE
        Vector2[] points = new Vector2[pointsCount];
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
            Vector2 newLineEndPoint = Vector2.Lerp(startPoint, endPoint, 1);
            Vector2 newPoint = Vector2.Lerp(startPoint, newLineEndPoint, (float)pIdx / pointsCount);
            newPoint.x *= Laser.Instance.multx;
            newPoint.y *= Laser.Instance.multy;
            points[pIdx] = newPoint;
        }
        return points;
    }
}
