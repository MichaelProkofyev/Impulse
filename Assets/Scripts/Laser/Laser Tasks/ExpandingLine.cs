using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingLine : LaserTaskBase
{

    new const LASERPATTERN type  = LASERPATTERN.CIRCLE;
	int pointsCount = Const.pointsPerPattern[type];

    Vector2 endPoint;

    public ExpandingLine(Vector2 newStartPoint, float newSpeed = 5) : base(newStartPoint)
    {
        endPoint = startPoint;// + UnityEngine.Random.insideUnitCircle / 2f;
      //  print("START POINT: " + startPoint);
    }

    public override Vector2[] NextPoints(float deltaTime)
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
