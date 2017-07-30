using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class LaserTaskBase {

    protected Vector3 startPoint;
    protected int cyclesCount; //Number of times to repeat the pattern. 0 - infinity.
    protected float duration;

    protected float progress = 0f; //0..1
    protected int currCycleIdx = 0;

    public bool isFinished = false;
    

    public LaserTaskBase(Vector3 newStartPoint, float newSpeed = 5f, int newCyclesCount = 0) {
        this.startPoint = newStartPoint;
        this.cyclesCount = newCyclesCount;
        this.duration = newSpeed;
    }

    //Wrapper-method for REAL CALCULATIONS in NextPointCalculations method
    public Vector2[] NextPoints(int pointsCount) {
        if (progress >= 1f) { //End of the cycle
            currCycleIdx++;
            bool moveToNextCycle = cyclesCount == 0 || currCycleIdx < cyclesCount; //If set to infinite repeat OR cycles left
            if (moveToNextCycle) {
                progress = 0;
            } else {
                isFinished = true;
            }
        }
        Vector2[] nextPoints = NextPointsCalculations(pointsCount);
        progress += Time.deltaTime / duration;
        return nextPoints;
    }

    abstract public Vector2[] NextPointsCalculations(int pointsCount);
}
