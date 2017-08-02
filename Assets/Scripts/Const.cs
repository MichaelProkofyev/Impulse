using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LASERPATTERN
{
	NONE,
    DOT,
	CIRCLE,
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
	};	

	public static float circle_max_rotation_speed = 1f;
}
