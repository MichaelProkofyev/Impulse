using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : LaserTaskBase {

	public Vector3 rotation = Vector3.zero;
    public Vector3 rotation_speed = Vector3.zero;
	float radius;

	int pointsCount = Const.pointsPerPattern[type];
	new const LASERPATTERN type  = LASERPATTERN.CIRCLE;

	public Circle(Vector2 newStartPoint, float radius = 1) : base(newStartPoint) {
		this.radius = radius;
    }

	public override Vector2[] NextPoints(float deltaTime) {
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
		return Const.RotatePoints(points, rotation);
	}
}