using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createCircle : MonoBehaviour {
    

    public AnimationCurve sizeCurve;
    public float speed = 0.01f;
    public float size;


    public float angleOffset;



    [Range (0, 1)] public float radius = 1f;
    [Range (0, 200)] public int pointCount = 200;

    [Range (0, 40)] public int dotLength = 10;
    [Range (0, 40)] public int dotInterval = 5;
    [Range (0, 40)] public int dotOffset = 10;

    public Color col = Color.red;
    [Range (0, 1)] public float minСhatter;
    [Range (0, 1)] public float maxСhatter = 1;
    private float colorMultipy = 1;

    private List<RCPoint> points;
    private float timer;



	
	void Update () {



        radius = Mathf.Abs(sizeCurve.Evaluate(timer)) < 1 ? Mathf.Abs(sizeCurve.Evaluate(timer)) : 1;


        points = new List<RCPoint>();

        if (minСhatter > 0 || maxСhatter < 1)
        {
            colorMultipy = Random.Range(minСhatter, maxСhatter);
        }
        else
            colorMultipy = 1;


        uint i;
        for (i = 0; i < pointCount; i++) {
            RCPoint point = new RCPoint();

            float phi = (float)i * Mathf.PI * 2.0f / (float)pointCount +  + angleOffset * Mathf.Deg2Rad;

            Vector2 pointTransform = new Vector3((Mathf.Sin(phi) * radius * 32767.5f - 0.5f), (Mathf.Cos(phi) * radius * 32767.5f - 0.5f));


            point.x = (short)(pointTransform.x * Global.mSize);
            point.y = (short)(pointTransform.y * Global.mSize);

            /* Это зачем? для плавного перелива доработать?? */
//            point.red = (i + dotOffset)%dotLength < dotInterval &&  dotLength > 0? (ushort)(col.r * 65535) : (ushort) 0;
//            point.green = (i + dotOffset)%dotLength < dotInterval &&  dotLength > 0 ? (ushort)(col.g * 65535) : (ushort) 0;
//            point.blue = (i + dotOffset)%dotLength < dotInterval &&  dotLength > 0 ? (ushort)(col.b * 65535) : (ushort) 0;


            point.red = (ushort)(col.r * 65535 * colorMultipy);
            point.green = (ushort)(col.g * 65535 * colorMultipy);
            point.blue = (ushort)(col.b * 65535 * colorMultipy);


            point.intensity = RayComposerDraw.intensity;
            point.user1 = RayComposerDraw.user1;
            point.user2 = RayComposerDraw.user2;

            points.Add(point);

        }

        GetComponent<laserPoints>().points = points;

        if (timer < sizeCurve.keys[sizeCurve.keys.Length - 1].time)
            timer += speed;
        else
            Finish();
            //Debug.Log("curve Finish");
		
	}


    void Finish()
    {
        Global.numSyms--;
        Destroy(this.gameObject);
    }
}
