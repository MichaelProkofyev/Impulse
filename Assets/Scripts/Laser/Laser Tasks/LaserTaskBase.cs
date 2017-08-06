using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LaserTaskBase {


    public LASERPATTERN type = LASERPATTERN.NONE;
    public ushort brightness;

    public Vector3 rotation = Vector3.zero;
    public Vector3 rotation_speed = Vector3.zero;

    public float wobble = 0f;

    public float dashLength;
    public int pointsCount;

    public Vector2[] currentPoints;

    public Vector2 startPoint;
    

    public LaserTaskBase(Vector2 newStartPoint, ushort brightness = CONST.LASER_MAX_VALUE, float dashLength = 0, float shake = 0) {
        this.startPoint = newStartPoint;
        this.brightness = brightness;
        this.dashLength = 0;
        this.wobble = shake;
    }

    public Vector2[] NextPoints(float deltaTime)
    {
        rotation += rotation_speed * deltaTime;
        //Debug.Log(rotation_speed);
        Vector2[] patternPoints = CalculatePatternPoints(deltaTime);
        //SHAKE THE POINTS
        for (int pointIdx = 0; pointIdx < patternPoints.Length; pointIdx++)
        {
            patternPoints[pointIdx] += CONST.RRange2(-(Laser.Instance.global_wobble + wobble), Laser.Instance.global_wobble + wobble);
        }

        currentPoints = CONST.RotatePoints(patternPoints, rotation);
        return currentPoints;
    }

    abstract public Vector2[] CalculatePatternPoints(float deltaTime);
}