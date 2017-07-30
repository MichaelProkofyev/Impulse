using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTask : LaserTaskBase {


    //Constructor that just uses the base class, nothing more
    public LineTask(Vector3 newStartPoint, float newSpeed = 5, int newCyclesCount = 0) : base(newStartPoint, newSpeed, newCyclesCount)
    {

    }

    public override Vector2[] NextPointsCalculations(int pointsCount)
    {
        Vector2[] points = new Vector2[pointsCount];
        for (int pIdx = 0; pIdx < pointsCount; pIdx++)
        {
            Vector2 newPoint;
            newPoint.x = ((float)pIdx / pointsCount);
            newPoint.y = .5f;
            points[pIdx] = newPoint;
        }
        return points;
    }

    public Vector3 NextPointCalculationsLegacy()
    {
        return startPoint;
    }
}