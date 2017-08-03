using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : LaserTaskBase {

    float sideLength;
    int pointsCount = Const.pointsPerPattern[type];
    new const LASERPATTERN type = LASERPATTERN.SQUARE;

    public Square(Vector2 newStartPoint, float sideLength) : base(newStartPoint)
    {
        this.sideLength = sideLength;
    }

    public override Vector2[] NextPoints(float deltaTime)
    {
        //throw new NotImplementedException();
        Vector2[] points = new Vector2[pointsCount];

        for (int pIdx = 0; pIdx < pointsCount; pIdx++)
        {

        }

        return points;
    }
}
