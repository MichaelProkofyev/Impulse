using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectController : SingletonComponent<KinectController> {

    public float[] brightness = new float[3] { 0, 0, 0 };
    public Vector2[] positions = new Vector2[3] { Vector2.zero, Vector2.zero, Vector2.zero };
    public float[] wobble = new float[3] { 0, 0, 0 };
    public Vector2[] rotationSpeeds = new Vector2[3] { Vector2.zero, Vector2.zero, Vector2.zero };
    public float[] radiuses = new float[3] { 0,0,0 };

    public bool sendData = false;

    //TIME
    Vector2[] numberAnchorPoints = new Vector2[] {
       // new Vector2()
    };


    // Use this for initialization
    void Start () {
		
	}

    public void InitScene()
    {
        for (int i = 0; i < brightness.Length; i++)
        {
            brightness[i] = 1;
        }
        sendData = true;
    }

    public void DisableScene()
    {
        for (int i = 0; i < brightness.Length; i++)
        {
            brightness[i] = 0;
        }
        Laser.Instance.ClearCircles();
        sendData = false;
    }


    // Update is called once per frame
    void Update () {
		
	}

    void OnValidate() {
        if (!sendData) return;
        
        for (int i = 0; i < brightness.Length; i++) {
            Laser.Instance.AddCircleData(
                laserIdx: 1,
                patternID: i,
                brightness: (ushort)(brightness[i] * CONST.LASER_MAX_VALUE),
                wobble: wobble[i],
                rotation_speed: rotationSpeeds[i],
                pointsMultiplier: 1,
                center: positions[i],
                radius: radiuses[i]);
        }
        
    }

        public void AddKinectData(int id, float x, float y, float width, float height, float noise, float newBrightness) {
            brightness[id] = newBrightness;
            positions[id] = new Vector2(x, y);
            wobble[id] = noise;
            rotationSpeeds[id] = Vector2.right * height;
            radiuses[id] = width;
            OnValidate();
    }
}
