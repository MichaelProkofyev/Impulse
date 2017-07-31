using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinTask : LaserTaskBase {


    //Constructor that just uses the base class, nothing more
    public SinTask(Vector3 newStartPoint, float newSpeed = 5, int newCyclesCount = 0) : base(newStartPoint, newSpeed, newCyclesCount)
    {

    }

    public override Vector2[] NextPointsCalculations(int pointsCount)
    {
        Vector2[] points = new Vector2[pointsCount];
        //float maxy = -Mathf.Infinity;
        //float miny = Mathf.Infinity;
        for (int pIdx = 0; pIdx < pointsCount; pIdx++)
        {
            Vector2 newPoint;
            newPoint.x = (((float)pIdx / pointsCount) + startPoint.x) * Laser.Instance.multx;
            newPoint.y = (Mathf.Sin(newPoint.x + progress) + startPoint.y) * Laser.Instance.multy;
            //if (newPoint.y > maxy) maxy = newPoint.y;
            //if (newPoint.y < miny) miny = newPoint.y;

            points[pIdx] = newPoint;
        }
        //Debug.Log("MAXY " + maxy + " MINY " + miny);
        return points;
    }
}