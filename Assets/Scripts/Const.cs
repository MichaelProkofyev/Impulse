using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PATTERN
{
	NONE,
    DOT,
	CIRCLE,
    SQUARE,
    LINE
}

public enum SCENE
{
    NONE,
    CIRCLES_1,
    TAKEOFF_2,
    SQUARES_3,
    COCOON_5,
    KINECT
}

public static class CONST {

    static System.Random rand = new System.Random();


    //LED
    public static string LED_SERIAL_PORT = "/dev/tty.usbmodem1421";
    public static int LED_COUNT = 10;
    public static float LED_MIN_UPDATE_TIME = 0.017f;

    //LASER

    public const ushort LASER_MAX_VALUE = 65535;


    public static Vector2[] RotatePoints(Vector2[] points, Vector3 rotation)
    {
        Quaternion new_rotation = Quaternion.Euler(rotation);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, new_rotation, Vector3.one);
        //Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, new_rotation, Vector3.one * 1.0f.Remap(0f, 1f, 0, 2f));


        for (int pIdx = 0; pIdx < points.Length; pIdx++) {
            Vector3 d_point = new Vector3(points[pIdx].x, points[pIdx].y, 0);
            Vector3 transformedPoint = m.MultiplyPoint3x4(d_point);
            points[pIdx] = transformedPoint;
        }
        return points;
    }


    public static float RRange(float min, float max) {
        return (float)rand.NextDouble() * (max - min) + min;
    }

    public static int RRangeInt(int min, int max)
    {
        return rand.Next(min + max);
    }

    public static Vector2 RRange2(float min, float max)
    {
        float x = (float)rand.NextDouble() * (max - min) + min;
        float y = (float)rand.NextDouble() * (max - min) + min;
        return new Vector2(x, y);
    }
}

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}