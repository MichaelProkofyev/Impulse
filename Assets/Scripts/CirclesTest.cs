using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclesTest : MonoBehaviour {

	public int numberOfCircles = 0;
	public float radius =1f;

	public Vector3 rotationSpeed = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

			for (int i = 0; i < numberOfCircles; i++) {
				Laser.Instance.AddCircleData(
               laserIdx: 1,
               patternID: i,
               brightness: CONST.LASER_MAX_VALUE,
               wobble: 0,
               rotation_speed: rotationSpeed,
               pointsMultiplier: 1,
               center: Vector2.one - new Vector2(0.1f * i, 0.1f * i),
               radius: radius);
            
			}
		
	}
}
