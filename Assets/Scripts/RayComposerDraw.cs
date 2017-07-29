using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;


public class RayComposerDraw : MonoBehaviour {
    public float upperRandLimit;
    public float mSize;
    public bool randomDots;
    public int dotsCount;
    public uint speed = 20000; /* sampling rate */
    public uint minPoints = 200;
    public uint pointsInterval = 500;
    public uint startAnchors = 4;
    public uint endAnchors = 4;
    public uint startBlackPoints = 4;
    public uint endBlackPoints = 4;

    [Space]
    [SerializeField, Range (0, 65535)] public static ushort red = 65535;
    [SerializeField, Range (0, 65535)] public static ushort green = 65535;
    [SerializeField, Range (0, 65535)] public static ushort blue = 65535;
    [Range (0, 65535)] public static ushort intensity = 65535;
    [Range (0, 65535)] public static ushort user1 = 65535;
    [Range (0, 65535)] public static ushort user2 = 65535;

    public GameObject[] pointsObject;

    [Space][Space]
    public bool drawTestCircle;
    public float testCircleradius = 1;

    [SerializeField, Range (0, 65535)] public ushort tRed = 65535;
    [SerializeField, Range (0, 65535)] public ushort tGreen = 65535;
    [SerializeField, Range (0, 65535)] public ushort tBlue = 65535;
    [Range (0, 65535)] public ushort tIntensity = 65535;
    [Range (0, 65535)] public ushort tUser1 = 65535;
    [Range (0, 65535)] public ushort tUser2 = 65535;



    int ret; /* return value */
    int count; /* device count */
    int handle; /* device handle */
    StringBuilder deviceIdString = new StringBuilder(256);  /* device id string */

    public static List<RCPoint> points; /* points to draw */
    uint pointCount;

    [Space]
    public short correctionX;
    public short correctionY;
    public float heigthCorrection = 1;


	void Start () {
         
        ret = RayComposer.RCInit();

        if(ret < 0){
            Debug.Log("Error initialising Library! Exit."); return;
        }
        if(ret < RayComposerConstants.RCAPI_VERSION){
            Debug.Log("API Version too old. Please use a newer dll/lib."); return;
        }
            

        /* Detect & enumerate the connected devices. */
        count = RayComposer.RCEnumerateDevices();
        if(count < 0){
            Debug.Log("Error enumerating devices! Exit."); return;
        }
        if(count == 0){
            //Debug.Log("count  " + count); 
            Debug.Log("No devices found. Exit.\n"); return;
        }

        /* List the devices found. */
        Debug.Log("Found " + count + " device(s):\n");
        for(uint i = 0; i < count; i++){
                   

            ret = RayComposer.RCDeviceID(i, deviceIdString, (uint)deviceIdString.Capacity);

            if(ret < 0){
                Debug.Log("Error reading device id! Exit.\n"); return;
            }
            Debug.Log(i + " " + deviceIdString + "\n");
        }


        /* Demo laser output */
        /* Select first device in list. */

        ret = RayComposer.RCDeviceID(0, deviceIdString, (uint)deviceIdString.Capacity);

        if(ret < 0){
            Debug.Log("Error reading device id! Exit.\n");
        }

        Debug.Log("Opening device: " + deviceIdString + "\n");

        handle = RayComposer.RCOpenDevice(deviceIdString);


        if(handle < 0){
            Debug.Log("Error opening device: " + handle + " Exit.\n"); return;
        }

        Debug.Log("Starting laser.\n");
 
        ret = RayComposer.RCStartOutput(handle);
        if(ret < (int)RCReturnCode.RCOk){
            Debug.Log("Error starting laser output: " + ret + " Exit.\n"); return;
        }

	}


	

	void Update () {

        points = new List<RCPoint>();

        if(randomDots) {
            // for (int dotIdx = 0; dotIdx < dotsCount; dotIdx++) {
            //     pointCount = 10;
            //     for (uint i = 0; i < pointCount; i++)
            //     {
            //         RCPoint point = new RCPoint();
            //         Vector2 randCenter = new Vector2(Random.Range(0, 16383), Random.Range(0, 16383));
            //         float phi = (float)i * Mathf.PI * 2.0f / (float)pointCount;
            //         point.x = (short)(((Mathf.Sin(phi) * 32767.5 - 0.5 + randCenter.x) * testCircleradius * mSize) + correctionX);
            //         point.y = (short)(((Mathf.Cos(phi) * 32767.5 - 0.5 + randCenter.y) * testCircleradius * mSize) + correctionY);
            //         point.red = tRed;
            //         point.green = tGreen;
            //         point.blue = tBlue;
            //         point.intensity = tIntensity;
            //         point.user1 = tUser1;
            //         point.user2 = tUser2;

            //         points.Add(point);

            //     }    
            // }
            for (int dotIdx = 0; dotIdx < dotsCount; dotIdx++) {
                pointCount = 10;
                for (uint i = 0; i < pointCount; i++)
                {
                    RCPoint point = new RCPoint();
                    Vector2 randCenter = new Vector2(Random.Range(0, upperRandLimit), Random.Range(0, upperRandLimit));
                    float phi = (float)i * Mathf.PI * 2.0f / (float)pointCount;
                    point.x = (short)(((Mathf.Sin(phi) * 32767.5 - 0.5 ) * testCircleradius * mSize ) + correctionX + randCenter.x);
                    point.y = (short)(((Mathf.Cos(phi) * 32767.5 - 0.5) * testCircleradius * mSize ) + correctionY + randCenter.y);

                    point.red = tRed;
                    point.green = tGreen;
                    point.blue = tBlue;
                    point.intensity = tIntensity;
                    point.user1 = tUser1;
                    point.user2 = tUser2;

                    points.Add(point);

                }    
            }
            
        }else if(drawTestCircle)
        {

            /* Your TODO: create fancy laser graphics here */

            pointCount = 200;
            uint i;
            for (i = 0; i < pointCount; i++)
            {
                RCPoint point = new RCPoint();

                float phi = (float)i * Mathf.PI * 2.0f / (float)pointCount;
                point.x = (short)(((Mathf.Sin(phi) * 32767.5 - 0.5) * testCircleradius * mSize) + correctionX);
                point.y = (short)(((Mathf.Cos(phi) * 32767.5 - 0.5) * testCircleradius * mSize) + correctionY);
                point.red = tRed;
                point.green = tGreen;
                point.blue = tBlue;
                point.intensity = tIntensity;
                point.user1 = tUser1;
                point.user2 = tUser2;

                points.Add(point);

            }
        }
        else
        {
            
            pointsObject = GameObject.FindGameObjectsWithTag("LaserPoints");


            for (int i = 0; i < pointsObject.Length; i++)
            {

                List<RCPoint> objectPoints = new List<RCPoint>();
                objectPoints = pointsObject[i].GetComponent<laserPoints>().points;




                if (objectPoints.Count > 0)
                {

                        
                    //Position Correction
                    for (int p = 0; p < objectPoints.Count; p++)
                    {
                        RCPoint point = new RCPoint();
                        point.x = (short)(objectPoints[p].x + correctionX);
                        point.y = (short)((objectPoints[p].y + correctionY) * heigthCorrection);
                        point.red = objectPoints[p].red;
                        point.green = objectPoints[p].green;
                        point.blue = objectPoints[p].blue;
                        point.intensity = 0;
                        point.user1 = 0;
                        point.user2 = 0;
                        objectPoints[p] = point;
//                        objectPoints[p].x = (short)(objectPoints[p].x + correctionX);
//                        objectPoints[p].y = (short)(objectPoints[p].y + correctionY);

 
                    }



                    for (int b = 0; b < startBlackPoints; b++) // Put Start black Points
                        {
                            RCPoint point = new RCPoint();
                            point.x = objectPoints[0].x;
                            point.y = objectPoints[0].y;
                            point.red = 0;
                            point.green = 0;
                            point.blue = 0;
                            point.intensity = 0;
                            point.user1 = 0;
                            point.user2 = 0;
                            points.Add(point);
                        }



                    for (int p = 0; p < objectPoints.Count; p++)
                    {
                        if (p == 0) // Put start color Points
                        {
                            for (int b = 0; b < startAnchors; b++)
                            {
                                points.Add(objectPoints[p]);
                            }
                        }

                        points.Add(objectPoints[p]);
                        if(p > 0)
                            Debug.DrawLine(new Vector2(objectPoints[p - 1].x, objectPoints[p - 1].y)/32767.5f, new Vector2(objectPoints[p].x, objectPoints[p].y)/32767.5f, new Color(objectPoints[p].red / 65535f, objectPoints[p].green / 65535f, objectPoints[p].blue / 65535f));
                        //print(objectPoints[p].x);

                        if (p == objectPoints.Count-1)  // Put End color Points
                        {
                            for (int b = 0; b < endAnchors; b++)
                            {
                                points.Add(objectPoints[p]);
                            }
                        }

  
                            

                    }


                    for (int b = 0; b < endBlackPoints; b++) // Put End black Points
                    {
                        RCPoint point = new RCPoint();
                        point.x = objectPoints[objectPoints.Count-1].x;
                        point.y = objectPoints[objectPoints.Count-1].y;
                        point.red = 0;
                        point.green = 0;
                        point.blue = 0;
                        point.intensity = 0;
                        point.user1 = 0;
                        point.user2 = 0;
                        points.Add(point);
                    }
                }





            }
        }



//        for (int i = 0; i < points.Count; i++)
//        {
//            print(points[i].x);
//            print(points[i].y);
//            print(points[i].red);
//            print(points[i].green);
//            print(points[i].blue);
//
//        }
//
//        print("---");

        checkPointsCount();
            

            /* wait for free buffer; second parameter is timeout
     *   0 = poll number of free buffers only, return immediately
     * < 0 = wait forever until buffer becomes free
     * > 0 = wait the number of miliseconds or until a buffer becomes free */
        ret = RayComposer.RCWaitForReady(handle, -1);
        if (ret < (int)RCReturnCode.RCOk)
        {
            Debug.Log("\nError waiting for free buffer: " + ret + " Exit.\n");
            return;
        }
        ret = RayComposer.RCWriteFrame(handle, points.ToArray(), (uint)points.Count, speed, 0);
        if (ret < (int)RCReturnCode.RCOk)
        {
            Debug.Log("\nError writing frame to device: " + ret + " Exit.\n");
            return;
        }
             

//                for(int i = 0; i < points.Count; i++) {
//                    print(points[i].x);
//                    print(points[i].y);
//                    print(points[i].red);
//          
//                }
//        
//                print("----");
		
	}


    void optimizePoints() {
        if (points.Count > 0)
        {
            
        }
    }

    void checkPointsCount()
    {
        // Check if number of points less than minimum Points number. If less than we add empty
        if (points.Count < (int) minPoints)
        {
            for (int i = points.Count; i < minPoints; i++)
            {
                RCPoint point = new RCPoint();

                point.x = points.Count > 0 ? points[points.Count - 1].x : (short) 0;
                point.y = points.Count > 0 ? points[points.Count - 1].y : (short) 0;
                point.red = 0;
                point.green = 0;
                point.blue = 0;
                point.intensity = 0;
                point.user1 = 0;
                point.user2 = 0;

                points.Add(point);
            }
        }
    }

    void OnApplicationQuit() {
        if (count > 0) {
            Debug.Log("Stoping laser.\n");
            ret = RayComposer.RCStopOutput(handle);
            if (ret < (int)RCReturnCode.RCOk)
            {
                Debug.Log("Error stoping laser output: " + ret + "Exit.\n");
                return;
            }

            Debug.Log("Closing device.\n");
            ret = RayComposer.RCCloseDevice(handle);
            if (ret < (int)RCReturnCode.RCOk)
            {
                Debug.Log("Error closing device: " + ret + " Exit.\n");
                return;
            }


            ret = RayComposer.RCExit();
            if (ret < (int)RCReturnCode.RCOk)
            {
                Debug.Log("Error closing Library: " + ret + " Exit.\n");
                return;
            }
        }
    }
}