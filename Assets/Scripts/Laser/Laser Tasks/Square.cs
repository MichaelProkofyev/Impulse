﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : LaserTaskBase {

    float sideLength;
    int pointsCount = CONST.pointsPerPattern[type];
    new const LASERPATTERN type = LASERPATTERN.SQUARE;

    public Square(Vector2 newStartPoint, float sideLength = 1, ushort brightness = CONST.LASER_MAX_VALUE) : base(newStartPoint, brightness)
    {
        this.sideLength = sideLength;
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {
        //throw new NotImplementedException();
        Vector2[] points = new Vector2[pointsCount + 1];

        float interval = Mathf.Floor(pointsCount / 4.0f);
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
            if (pIdx < pointsCount / 4.0f) { //SIDE 1
                points[pIdx] = Vector2.Lerp(Vector2.zero, new Vector2(0, 1), (float)pIdx / interval);
            }
            else if(pIdx < (pointsCount / 4.0f) * 2f){ //SIDE 2
                float intervalPoints = pIdx - pointsCount / 4.0f;
                points[pIdx] = Vector2.Lerp(new Vector2(0, 1), new Vector2(1,1), (float)intervalPoints / interval);
            }
            else if (pIdx < (pointsCount / 4.0f) * 3f) { //SIDE 3
                float intervalPoints = pIdx - (pointsCount / 4.0f) * 2f;
                points[pIdx] = Vector2.Lerp(new Vector2(1, 1), new Vector2(1, 0), (float)intervalPoints / interval);
            }
            else { //SIDE 4
                float intervalPoints = pIdx - (pointsCount / 4.0f) * 3f;
                points[pIdx] = Vector2.Lerp(new Vector2(1, 0), Vector2.zero, (float)intervalPoints / interval);
            }
            //Debug.Log("IDX: " + pIdx + " X" + points[pIdx].x + " Y " + points[pIdx].y);

            //SCALE
            points[pIdx] *= sideLength;
        }
        points[pointsCount] = points[0];

        return points;
    }
}