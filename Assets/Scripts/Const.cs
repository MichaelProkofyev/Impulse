using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LASERPATTERN
{
	NONE,
    DOT,
	CIRCLE,
    SQUARE
}


public static class Const {

    //LED
    public static string LED_SERIAL_PORT = "/dev/tty.usbmodem1421";
    public static int LED_COUNT = 10;
    public static float LED_MIN_UPDATE_TIME = 0.017f;

    //LASER
	public static Dictionary<LASERPATTERN, int> pointsPerPattern = new Dictionary<LASERPATTERN, int>() {
		{LASERPATTERN.NONE, 100},
		{LASERPATTERN.DOT, 20},
		{LASERPATTERN.CIRCLE, 30},
        {LASERPATTERN.SQUARE, 30},
    };	
	public static float circle_max_rotation_speed = 1f;
    public static float square_max_rotation_speed = 1f;

    public static Vector2[] RotatePoints(Vector2[] points, Vector3 rotation)
    {
        Quaternion new_rotation = Quaternion.Euler(rotation);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, new_rotation, Vector3.one * 1.0f.Remap(0f, 1f, 0, 2f));

        for (int pIdx = 0; pIdx < points.Length; pIdx++)
        {
            Vector3 d_point = new Vector3(points[pIdx].x, points[pIdx].y, 0);
            Vector3 transformedPoint = m.MultiplyPoint3x4(d_point);
            points[pIdx] = transformedPoint;
        }
        return points;
    }
}


public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}