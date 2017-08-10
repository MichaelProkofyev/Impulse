using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : LaserTaskBase {

	public float radius = 1;
	List<Vector2> anchrors = new List<Vector2>();

    public float pointsMultiplier = 1f;

    public Circle() : base() {
        this.type = PATTERN.CIRCLE;
        this.pointsCount = Laser.Instance.circlePoints;
    }

	public override Vector2[] CalculatePatternPoints(float deltaTime) {
        pointsCount = Mathf.CeilToInt(Laser.Instance.circlePoints * pointsMultiplier);
        Vector2[] points = new Vector2[pointsCount];
		for (int pIdx = 0; pIdx < pointsCount; pIdx++) {
			Vector2 newPoint;
			float phi = (float)pIdx * Mathf.PI * 2.0f / (float)pointsCount;
			newPoint.x = Mathf.Sin(phi) * radius + startPoint.x;
			newPoint.y = Mathf.Cos(phi) * radius + startPoint.y;
			
			float progress = (float)pIdx/(float)pointsCount;
			float anchorProgressStep =  1.0f / Laser.Instance.circleAnchors;
			for (int i = 0; i < Laser.Instance.circleAnchors; i++) {
				if (anchrors.Count < i + 1) {
					if (anchorProgressStep * i < progress && progress < anchorProgressStep * (i + 1)) {
						anchrors.Add(newPoint);
					}
				}
			}
			points[pIdx] = newPoint;
		}

		

		List<Vector2> pointsWithAnchors = new List<Vector2>();

        for (int i = 0; i < points.Length; i++) {
            Vector2 point = points[i];
            pointsWithAnchors.Add(point);
            for (int aIDx = 0; aIDx < anchrors.Count; aIDx++) {
                if(point.x == anchrors[aIDx].x && point.y == anchrors[aIDx].y) {
                    for (int j = 0; j < Laser.Instance.additionalPointsAtAnchorCIRCLE; j++) {
                        pointsWithAnchors.Add(point);    
                    }       
                }
            }
        }

        // points[pointsCount] = points[0];
        pointsWithAnchors.Add(points[0]);

        points = pointsWithAnchors.ToArray();
		anchrors.Clear();
        return points;
	}
}