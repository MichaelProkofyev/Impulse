using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LaserTaskBase {


    public PATTERN type = PATTERN.NONE;
    public ushort brightness = 0;

    public Vector3 rotation = Vector3.zero;
    public Vector3 rotation_speed = Vector3.zero;

    public float wobble = 0f;

    public float dashLength = 0;
    public int pointsCount;

    public Vector2[] currentPoints;

    public Vector2 startPoint = Vector2.zero;    

    public LaserTaskBase() {
    }

    public Vector2[] NextPoints(float deltaTime)
    {
        rotation += rotation_speed * deltaTime;
        //Debug.Log(rotation_speed);
        Vector2[] patternPoints = CalculatePatternPoints(deltaTime);
        for (int pointIdx = 0; pointIdx < patternPoints.Length; pointIdx++)
        {
            //SHAKE THE POINTS
            patternPoints[pointIdx] += CONST.RRange2(-(Laser.Instance.global_wobble + wobble), Laser.Instance.global_wobble + wobble);
        }

        Vector2[] rotatedPoints = CONST.RotatePoints(patternPoints, rotation);

        currentPoints = rotatedPoints;
        return currentPoints;
    }

    abstract public Vector2[] CalculatePatternPoints(float deltaTime);
}