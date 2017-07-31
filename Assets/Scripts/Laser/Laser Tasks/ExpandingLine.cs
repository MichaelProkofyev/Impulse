using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingLine : LaserTaskBase
{

    Vector2 endPoint;

    public ExpandingLine(Vector2 newStartPoint, float newSpeed = 5, int newCyclesCount = 0) : base(newStartPoint, newSpeed, newCyclesCount)
    {
        endPoint = startPoint + UnityEngine.Random.insideUnitCircle / 2f;
    }

    public override Vector2[] NextPointsCalculations(int pointsCount)
    {
        Vector2[] points = new Vector2[pointsCount];
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
            Vector2 newLineEndPoint = Vector2.Lerp(startPoint, endPoint, progress);
            Vector2 newPoint = Vector2.Lerp(startPoint, newLineEndPoint, (float)pIdx / pointsCount);
            newPoint.x *= Laser.Instance.multx;
            newPoint.y *= Laser.Instance.multy;
            points[pIdx] = newPoint;
        }


        return points;
    }
}
