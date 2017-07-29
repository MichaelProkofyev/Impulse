using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtNet;


public class testArtNet : MonoBehaviour {


    [Range(0, 255)]
    public int[] value;

    private int[] oldValue;
    private bool updateDMX;

    private short universe = 0;
    ArtNet.Engine   ArtEngine; 
    byte[]   DMXData;


	void Start () {
        //value = new int[512];
        oldValue = new int[value.Length];
        initDmx();
		
	}
	
	void FixedUpdate () {

        for (int i = 0; i < value.Length; i++)
        {
            if (oldValue[i] != value[i])
            {
                oldValue[i] = value[i];
                updateDMX = true;
            }                
        }



        if (updateDMX)
        {
            updateDMX = false;
            for (int i = 0; i < value.Length; i++)
            {
                DMXData[i] = (byte)value[i]; 
            }
        }
        ArtEngine.SendDMX (universe, DMXData, DMXData.Length);		
	}


    void initDmx ( )
    {
        DMXData = new byte[512];
        ArtEngine = new ArtNet.Engine("Open DMX Ethernet", "192.168.1.100");
        ArtEngine.Start ();
    }

    void OnApplicationQuit ( )
    {   
        //set all light to black on quit application
        byte[] setBlack = new byte[512];
        ArtEngine.SendDMX (universe, setBlack, setBlack.Length);
    }


}
