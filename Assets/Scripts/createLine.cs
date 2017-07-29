using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createLine : MonoBehaviour {


    public float speed = 0.01f;
    public float angleOffset;

    [Space]
    public AnimationCurve startPointDistanceCurve;
    public AnimationCurve startPointAngleCurve;
    private float startPointDistance;

    [Space]
    public AnimationCurve endPointDistanceCurve;
    public AnimationCurve endPointAngleCurve;
    private float endPointDistance;

    [Space]
    public Vector2 startPoint;
    public Vector2 endPoint;


    [Range (0, 200)] public int pointCount = 50;

    [Space]
    public Color col = Color.red;
    [Range (0, 1)] public float minСhatter;
    [Range (0, 1)] public float maxСhatter = 1;
    private float colorMultipy = 1;


    private List<RCPoint> points;
    private float timer;


	void Update () {

        points = new List<RCPoint>();

        RCPoint point1 = new RCPoint();

        if (minСhatter > 0 || maxСhatter < 1)
        {
            colorMultipy = Random.Range(minСhatter, maxСhatter);
        }
        else
            colorMultipy = 1;


        startPointDistance = startPointDistanceCurve.Evaluate(timer);

        if (startPointDistance > 1)
            startPointDistance = 1;
        if (startPointDistance < -1)
            startPointDistance = -1;

        startPoint = new Vector2(Mathf.Sin((startPointAngleCurve.Evaluate(timer) * 360 + angleOffset) * Mathf.Deg2Rad) * startPointDistance, Mathf.Cos((startPointAngleCurve.Evaluate(timer) * 360 + angleOffset) * Mathf.Deg2Rad) * startPointDistance);

        endPointDistance = endPointDistanceCurve.Evaluate(timer);
        if (endPointDistance > 1)
            endPointDistance = 1;
        if (endPointDistance < -1)
            endPointDistance = -1;

        endPoint = new Vector2(Mathf.Sin((endPointAngleCurve.Evaluate(timer) * 360 + angleOffset) * Mathf.Deg2Rad) * endPointDistance, Mathf.Cos((endPointAngleCurve.Evaluate(timer) * 360 + angleOffset) * Mathf.Deg2Rad) * endPointDistance);



        Vector2 fragment = (endPoint - startPoint) / pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            RCPoint point = new RCPoint();


            point.x = (short)(((startPoint.x + fragment.x * i) * 32767.5f - 0.5f) * Global.mSize);
            point.y = (short)(((startPoint.y + fragment.y * i) * 32767.5f - 0.5f) * Global.mSize);


            point.red = (ushort)(col.r * 65535 * colorMultipy);
            point.green = (ushort)(col.g * 65535 * colorMultipy);
            point.blue = (ushort)(col.b * 65535 * colorMultipy);


            point.intensity = RayComposerDraw.intensity;
            point.user1 = RayComposerDraw.user1;
            point.user2 = RayComposerDraw.user2;

            points.Add(point);

        }
            



        GetComponent<laserPoints>().points = points;

        float maxTimer;
        maxTimer = startPointDistanceCurve.keys[startPointDistanceCurve.keys.Length - 1].time;
        if (startPointAngleCurve.keys[startPointAngleCurve.keys.Length - 1].time > maxTimer)
            maxTimer = startPointAngleCurve.keys[startPointAngleCurve.keys.Length - 1].time;
        if (endPointDistanceCurve.keys[endPointDistanceCurve.keys.Length - 1].time > maxTimer)
            maxTimer = endPointDistanceCurve.keys[endPointDistanceCurve.keys.Length - 1].time;
        if (endPointAngleCurve.keys[endPointAngleCurve.keys.Length - 1].time > maxTimer)
            maxTimer = endPointAngleCurve.keys[endPointAngleCurve.keys.Length - 1].time;

        if (timer < maxTimer)
            timer += speed;
        else
        {
            Finish();
        }
		
	}

    void Finish()
    {
        Global.numSyms--;
        //Debug.Log("curve Finish");
        Destroy(this.gameObject);

    }
}
