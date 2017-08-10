using System.Runtime.InteropServices;

public partial class RayComposerConstants
{

    /// RCAPI_VERSION -> 0x0100
    public const int RCAPI_VERSION = 256;
}

public enum RCReturnCode
{

    /// RCOk -> 0
    RCOk = 0,

    /// RCErrorNotInitialised -> -1
    RCErrorNotInitialised = -1,

    /// RCErrorNotEnumerated -> -2
    RCErrorNotEnumerated = -2,

    /// RCErrorInvalidHandle -> -3
    RCErrorInvalidHandle = -3,

    /// RCErrorNotStarted -> -4
    RCErrorNotStarted = -4,

    /// RCErrorIO -> -5
    RCErrorIO = -5,

    /// RCErrorParameterOutOfRange -> -6
    RCErrorParameterOutOfRange = -6,

    /// RCErrorParameterInvalid -> -7
    RCErrorParameterInvalid = -7,
}

public enum RCUniverseDirection
{

    /// RCOutput -> 0
    RCOutput = 0,

    /// RCInput -> 1
    RCInput = 1,
}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct RCPoint
{

    /// short
    public short x;

    /// short
    public short y;

    /// unsigned short
    public ushort red;

    /// unsigned short
    public ushort green;

    /// unsigned short
    public ushort blue;

    /// unsigned short
    public ushort intensity;

    /// unsigned short
    public ushort user1;

    /// unsigned short
    public ushort user2;
}

public partial class RayComposer
{
    const string winStringDLL =  "rcdev64";
    const string macStringDLL = "RayComposer1.6";
    //const string winStringDLL = (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor) ? winStringDLL : macStringDLL;


    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCInit")]
    public static extern int RCInit();


    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCExit")]
    public static extern int RCExit();


    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCEnumerateDevices")]
    public static extern int RCEnumerateDevices();


    /// Return Type: int
    ///index: unsigned int
    ///deviceId: char*
    ///maxLength: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCDeviceID")]
    public static extern int RCDeviceID(uint index, System.Text.StringBuilder deviceId, uint maxLength);


    /// Return Type: int
    ///deviceId: char*
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCOpenDevice")]
    public static extern int RCOpenDevice(System.Text.StringBuilder deviceId);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCCloseDevice")]
    public static extern int RCCloseDevice(int handle);


    /// Return Type: int
    ///handle: int
    ///deviceLabel: char*
    ///maxLength: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCDeviceLabel")]
    public static extern int RCDeviceLabel(int handle, System.IntPtr deviceLabel, uint maxLength);


    /// Return Type: int
    ///handle: int
    ///deviceLabel: char*
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCSetDeviceLabel")]
    public static extern int RCSetDeviceLabel(int handle, System.IntPtr deviceLabel);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCStartOutput")]
    public static extern int RCStartOutput(int handle);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCStopOutput")]
    public static extern int RCStopOutput(int handle);


    /// Return Type: int
    ///handle: int
    ///timeout: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCWaitForReady")]
    public static extern int RCWaitForReady(int handle, int timeout);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCMaxSpeed")]
    public static extern int RCMaxSpeed(int handle);


    /// Return Type: int
    ///handle: int
    ///points: RCPoint*
    ///count: unsigned int
    ///speed: unsigned int
    ///repeat: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCWriteFrame")]
    public static extern int RCWriteFrame(int handle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] 
        RCPoint[] points, uint count, uint speed, uint repeat);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCUniverseCount")]
    public static extern int RCUniverseCount(int handle);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///universeName: char*
    ///maxLength: unsigned int
    ///pUniverseDirection: RCUniverseDirection*
    ///pChannelCount: unsigned int*
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCUniverseQuery")]
    public static extern int RCUniverseQuery(int handle, uint universeIndex, System.IntPtr universeName, uint maxLength, ref RCUniverseDirection pUniverseDirection, ref uint pChannelCount);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///startChannel: unsigned int
    ///data: unsigned char*
    ///count: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCUniverseWrite")]
    public static extern int RCUniverseWrite(int handle, uint universeIndex, uint startChannel, System.IntPtr data, uint count);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///startChannel: unsigned int
    ///data: unsigned char*
    ///count: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCUniverseRead")]
    public static extern int RCUniverseRead(int handle, uint universeIndex, uint startChannel, System.IntPtr data, uint count);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute(winStringDLL, EntryPoint = "RCUniverseUpdate")]
    public static extern int RCUniverseUpdate(int handle, uint universeIndex);

}
