using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : LaserTaskBase
{
    public Vector2 endPoint;


    public Line(Vector2 startPoint, Vector2 endPoint) : base() {
        this.type = PATTERN.DOT;
        this.pointsCount = Laser.Instance.linePoints;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime) {
        Vector2[] points = new Vector2[Laser.Instance.linePoints];
        for (int pIdx = 0; pIdx < Laser.Instance.linePoints; pIdx++) {
            Vector2 newPoint = Vector2.Lerp(startPoint, endPoint, (float)pIdx / Laser.Instance.linePoints);
            points[pIdx] = newPoint;
            // Debug.Log("X: " + newPoint.x + " Y: " + newPoint.y);
        }

        return points;
    }
}