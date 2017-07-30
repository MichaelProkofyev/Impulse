using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static float mSize = .5f;
    public static char symbol;
    public static bool ready = true;
    public static int numSyms = 5;
    public static int transmitCount; // Сколько символов передал

    public bool debug;

    [SerializeField, Range(0, 1)] public float maxSize = .5f;
    public char Symbol;
    public bool Ready;
    public int numSymbols = 5;

    public int recieveCount; // Сколько символов получил
    public float globalSpeedMultiply = .1f; // Скорость
    public float globalSpeed = .1f; // Скорость
    [Space]
    public int symbolsInTime = 5;
    [Space]
    public int zeroPrefabsCount = 1;
    public int onePrefabsCount = 12;
    public static int nPrefabsCount = 1;
    public static int commaPrefabsCount = 1;
    public static int dotPrefabsCount = 1;

    public int pnPrefabsCount = 1;
    public int pcommaPrefabsCount = 1;
    public int pdotPrefabsCount = 1;

    [Space]
    public float perlinSpeedRotation = .1f;

    [Space]
    public float duranceMin = 1;
    public float duranceMax = 1.5f;

    [Space]
    public float globalSpeedTimerSpeed;



    //    private List<string> symbolsSeq = new List<string>();
    //    private List<int> symbolsSeqDot;

    public static void SetLaserValues(char bit, char parentChar)
    {
        symbol = bit;
        // print("Parent Char'" + parentChar + "'");

        string prefabName;
        GameObject obj;
        switch (parentChar)
        {
            case '\n':
                print("N");
                prefabName = "n" + (int)Random.Range(0, nPrefabsCount);
                obj = GameObject.Instantiate((GameObject)Resources.Load(prefabName));
                break;
            case ',':
                prefabName = "comma" + (int)Random.Range(0, commaPrefabsCount);
                obj = GameObject.Instantiate((GameObject)Resources.Load(prefabName));
                print("COMMA");
                break;
            case '.':
                prefabName = "dot" + (int)Random.Range(0, dotPrefabsCount);
                obj = GameObject.Instantiate((GameObject)Resources.Load(prefabName));
                print("DOT");
                break;



        }
    }


    void Start()
    {
        //string prefabName;
        //prefabName = "n" + (int)Random.Range(0, nPrefabsCount);
        //GameObject.Instantiate((GameObject)Resources.Load(prefabName));

#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
    }


    void Update()
    {



        nPrefabsCount = pnPrefabsCount;
        commaPrefabsCount = pcommaPrefabsCount;
        dotPrefabsCount = pdotPrefabsCount;


        if (debug)
        {
            symbol = Symbol;
            ready = Ready;
            numSyms = numSymbols;
            // mSize = maxSize;
        }
        else
        {
            Symbol = symbol;
            //maxSize = mSize;
            Ready = ready;
        }

        mSize = maxSize;


        GameObject[] pointsObject = GameObject.FindGameObjectsWithTag("LaserPoints");
        numSyms = pointsObject.Length;
        if (pointsObject.Length < symbolsInTime)
        {
            getPrefab();
            ready = true;
        }
        else
            ready = false;






        //globalSpeed = Perlin.Noise(Time.fixedTime * globalSpeedTimerSpeed);

    }


    void getPrefab() {
        // Emulate 0, 1
        //symbol = Random.Range(0, 2);
        string name;
        int prefabCount;
        bool selectZero = true;//Random.value > 0.5f;
        if (selectZero)
        {
            name = "Zero";
            prefabCount = zeroPrefabsCount;
        }
        else
        {
            name = "One";
            prefabCount = onePrefabsCount;
        }



        string prefabName = name + (int)Random.Range(1, zeroPrefabsCount);
        GameObject obj = GameObject.Instantiate((GameObject)Resources.Load(prefabName));



        if (name == "Zero")
        {
            obj.GetComponent<createCircle>().angleOffset = Random.Range(0, 360);
            //obj.GetComponent<createCircle>().speed = globalSpeed * Random.Range(duranceMin, duranceMax);
            obj.GetComponent<createCircle>().speed = globalSpeed;
        }
        else
        {
            //obj.GetComponent<createLine>().angleOffset = Random.Range(0, 360);
            obj.GetComponent<createLine>().angleOffset = Perlin.Noise(Time.fixedTime * perlinSpeedRotation) * 360;
            obj.GetComponent<createLine>().speed = globalSpeed;
        }




    }
}
