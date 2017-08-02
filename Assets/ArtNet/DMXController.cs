using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtNet;


public class DMXController : SingletonComponent<DMXController> {

    public string channelToChange = "white";

    float mapTo_floor  = 0;
    float mapTo_ceiling  = 1;

    [Range(0, 255)]
    public int[] dmx_values;

    private int[] oldValue;
    private bool updateDMX;

    private short universe = 0;
    ArtNet.Engine   ArtEngine; 
    byte[]   DMXData;

    List<Dictionary<string, int>> DMX_Mapping = new List<Dictionary<string, int>>();

	void Start () {
        //value = new int[512];
        oldValue = new int[dmx_values.Length];
        initDmx();

        
        for (int dmxID = 0; dmxID < 4; dmxID++) {
            Dictionary<string, int> dmx_values_mapping = new Dictionary<string, int>() {
                {"brightness", dmxID * 10},
                {"red", dmxID * 10 + 1},
                {"green", dmxID * 10 + 2},
                {"blue", dmxID * 10 + 3},
                {"white", dmxID * 10 + 4},
                {"ultraviolet", dmxID * 10 + 6},
                {"strobe", dmxID * 10 + 7}

            };
            DMX_Mapping.Add(dmx_values_mapping);
        }

        for (int i = 0; i < 4; i++) {
            SetValue(i, "brightness", 255);
            // SetValue(i, "red", 255);
        }
	}

    public void SetValue(int dmx_id, string valueName, short new_value) {
        int general_index = DMX_Mapping[dmx_id][valueName];
        dmx_values[general_index] = new_value;
    }
	
	void FixedUpdate () {

        for (int i = 0; i < dmx_values.Length; i++)
        {
            if (oldValue[i] != dmx_values[i])
            {
                oldValue[i] = dmx_values[i];
                updateDMX = true;
            }                
        }



        if (updateDMX)
        {
            updateDMX = false;
            for (int i = 0; i < dmx_values.Length; i++)
            {
                DMXData[i] = (byte)dmx_values[i]; 
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
