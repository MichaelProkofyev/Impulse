using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LaserTaskBase {

    public const LASERPATTERN type = LASERPATTERN.NONE;
    public ushort brightness = 65535;

    int pointsCount = Const.pointsPerPattern[type];

    protected Vector2 startPoint;
    

    public LaserTaskBase(Vector2 newStartPoint) {
        this.startPoint = newStartPoint;
    }

    abstract public Vector2[] NextPoints(float deltaTime);
}
