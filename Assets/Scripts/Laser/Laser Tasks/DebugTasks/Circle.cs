using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : LaserTaskBase {

	public Vector3 rotation = Vector3.zero;
    public Vector3 rotation_speed = Vector3.one;
	float radius;

	new static Pattern_t type  = Pattern_t.CIRCLE;
	int pointsCount = Const.pointsPerPattern[type];

	public Circle(Vector2 newStartPoint, int newCyclesCount = 0, float radius = 1) : base(newStartPoint, newCyclesCount) {
		this.radius = radius;
    }

	public override Vector2[] NextPointsCalculations(float deltaTime) {
		//ROTATION
		rotation += rotation_speed * deltaTime;

        Vector2[] points = new Vector2[pointsCount + 1];
		for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
			Vector2 newPoint;

			float phi = (float)pIdx * Mathf.PI * 2.0f / (float)pointsCount;
			newPoint.x = Mathf.Sin(phi) * radius;
			newPoint.y = Mathf.Cos(phi) * radius;
			points[pIdx] = newPoint;
		}
		points[pointsCount] = points[0];
		return Rotate(points);
	}

	Vector2[] Rotate(Vector2[] points) {
        Quaternion new_rotation = Quaternion.Euler(rotation);
        Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, new_rotation, Vector3.one * mathScale(0f, 1f, 0, 2f, 1));

    	for (int pIdx = 0; pIdx < points.Length; pIdx++) { 
			Vector3 d_point = new Vector3(points[pIdx].x, points[pIdx].y, 0);
			Vector3 transformedPoint = m.MultiplyPoint3x4(d_point);
    		points[pIdx] = transformedPoint;
		}
        return points;
	}

	public float mathScale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return(NewValue);
    }
}
