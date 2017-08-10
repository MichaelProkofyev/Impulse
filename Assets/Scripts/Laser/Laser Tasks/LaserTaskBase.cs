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


        Vector2[] fatPoints = new Vector2[rotatedPoints.Length * Laser.Instance.fatness];
        //ADDITIONAL POINTS FOR CLARITY
        for (int pointIdx = 0; pointIdx < rotatedPoints.Length; pointIdx++) {
            for (int fIdx = 0; fIdx < Laser.Instance.fatness; fIdx++)
            {
                Vector2 point = rotatedPoints[pointIdx];
                Vector2 offset = Vector2.zero;
                switch (fIdx)
                {
                    case 1:
                        offset -= Vector2.right ;
                        break;
                    case 2:
                        offset -= Vector2.up;
                        break;
                    case 3:
                        point += Vector2.up;
                        break;
                    case 4:
                        point += Vector2.right;
                        break;

                }
                offset *= 0.001f * Laser.Instance.fatness_offset_multiplier;
                fatPoints[(pointIdx * Laser.Instance.fatness) + fIdx] = point + offset;
            }
        }

        currentPoints = fatPoints;
        return currentPoints;
    }

    abstract public Vector2[] CalculatePatternPoints(float deltaTime);
}