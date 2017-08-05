﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : LaserTaskBase
{

    new const LASERPATTERN type  = LASERPATTERN.DOT;
	int pointsCount = CONST.pointsPerPattern[type];

    Vector2 endPoint;
    public float speed = 0;
    float movementProgress = 0;

    public Dot(Vector2 newStartPoint, Vector2 endPoint, ushort brightness = CONST.LASER_MAX_VALUE, float newSpeed = 5) : base(newStartPoint, brightness)
    {
        System.Random rand = new System.Random();
        this.endPoint = startPoint + endPoint;;
      //  print("START POINT: " + startPoint);
    }

    public override Vector2[] CalculatePatternPoints(float deltaTime)
    {   
        movementProgress = movementProgress + deltaTime * speed * 5f;
        endPoint += UnityEngine.Random.insideUnitCircle *0.01f;
        startPoint += UnityEngine.Random.insideUnitCircle *0.01f;
        // Debug.Log(movementProgress);
        Vector2[] points = new Vector2[pointsCount];
        for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
            Vector2 newLineEndPoint = Vector2.LerpUnclamped(startPoint, endPoint, movementProgress);
            Vector2 newPoint = Vector2.Lerp(startPoint, newLineEndPoint, (float)pIdx / pointsCount);
            newPoint.x *= Laser.Instance.multx;
            newPoint.y *= Laser.Instance.multy;
            points[pIdx] = newPoint;
            // Debug.Log("X: " + newPoint.x + " Y: " + newPoint.y);
        }
        return points;
    }
}