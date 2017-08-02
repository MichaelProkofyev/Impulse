using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pattern_t
{
	NONE,
	CIRCLE,
	LINE,
}




public static class Const {

	public static Dictionary<Pattern_t, int> pointsPerPattern = new Dictionary<Pattern_t, int>() {
		{Pattern_t.NONE, 100},
		{Pattern_t.CIRCLE, 30},
		{Pattern_t.LINE, 20},
	};	

	public static float circle_max_rotation_speed = 1f;
}
