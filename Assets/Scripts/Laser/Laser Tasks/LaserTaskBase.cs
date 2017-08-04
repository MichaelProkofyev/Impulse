using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LaserTaskBase {

    public const LASERPATTERN type = LASERPATTERN.NONE;
    public ushort brightness;

    public Vector3 rotation = Vector3.zero;
    public Vector3 rotation_speed = Vector3.zero;

    public float dashLength;
    int pointsCount;

    protected Vector2 startPoint;
    

    public LaserTaskBase(Vector2 newStartPoint, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0) {
        this.startPoint = newStartPoint;
        this.brightness = brightness;
        this.dashLength = 0;
    }

    public Vector2[] NextPoints(float deltaTime)
    {
        rotation += rotation_speed * deltaTime;
        Vector2[] patternPoints = CalculatePatternPoints(deltaTime);
        Vector2[] rotatedPoints = CONST.RotatePoints(patternPoints, rotation);
        return rotatedPoints;
    }

    abstract public Vector2[] CalculatePatternPoints(float deltaTime);
}