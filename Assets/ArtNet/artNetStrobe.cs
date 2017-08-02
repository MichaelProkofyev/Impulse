using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class artNetStrobe : MonoBehaviour {

    [Range(0, 255)] public int[] strobe;
    public int[] strobeInterval;
    public int[] strobePause;
    public int[] fadeIn;
    public int[] fadeOut;

    private int[] value;
    private int[] timer;
    private bool[] isStrobe;



	void Start () {

        value = GetComponent<DMXController>().dmx_values;
        timer = new int[value.Length];
        isStrobe = new bool[value.Length];	
	}

	void Update () {

        for (int v = 0; v < value.Length; v++)
        {

            if (!isStrobe[v] && timer[v] > strobeInterval[v])
            {
                if (fadeIn[v] > 0 && value[v] < strobe[v])
                {
                    value[v] += fadeIn[v];
                }
                else
                {

                    isStrobe[v] = true;
                    timer[v] = 0;
                    if (strobe[v] > 0)
                    {
                        value[v] = strobe[v];
                    }
                }
            }
            else if (isStrobe[v] && timer[v] > strobePause[v])
            {
                if (fadeOut[v] > 0 && value[v] > 0)
                {
                    value[v] -= fadeOut[v];
                }
                else
                {

                    isStrobe[v] = false;
                    timer[v] = 0;
                    value[v] = 0;
                }
                if (value[v] < 0)
                    value[v] = 0;
            }

           
            timer[v]++;
		
        }

        GetComponent<DMXController>().dmx_values = value;
    }
}
