using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : LaserTaskBase {

	float radius;
	int pointsCount = CONST.pointsPerPattern[type];
	new const LASERPATTERN type  = LASERPATTERN.CIRCLE;

	public Circle(Vector2 center, float radius = 1, ushort brightness = CONST.LASER_MAX_VALUE) : base(center, brightness) {
		this.radius = radius;
    }

	public override Vector2[] CalculatePatternPoints(float deltaTime) {
        Vector2[] points = new Vector2[pointsCount + 1];
		for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
			Vector2 newPoint;

			float phi = (float)pIdx * Mathf.PI * 2.0f / (float)pointsCount;
			newPoint.x = Mathf.Sin(phi) * radius;
			newPoint.y = Mathf.Cos(phi) * radius;
			points[pIdx] = newPoint;
		}
		points[pointsCount] = points[0];
        return points;
	}
}