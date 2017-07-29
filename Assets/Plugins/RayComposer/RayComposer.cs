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

    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCInit")]
    public static extern int RCInit();


    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCExit")]
    public static extern int RCExit();


    /// Return Type: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCEnumerateDevices")]
    public static extern int RCEnumerateDevices();


    /// Return Type: int
    ///index: unsigned int
    ///deviceId: char*
    ///maxLength: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCDeviceID")]
    public static extern int RCDeviceID(uint index, System.Text.StringBuilder deviceId, uint maxLength);


    /// Return Type: int
    ///deviceId: char*
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCOpenDevice")]
    public static extern int RCOpenDevice(System.Text.StringBuilder deviceId);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCCloseDevice")]
    public static extern int RCCloseDevice(int handle);


    /// Return Type: int
    ///handle: int
    ///deviceLabel: char*
    ///maxLength: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCDeviceLabel")]
    public static extern int RCDeviceLabel(int handle, System.IntPtr deviceLabel, uint maxLength);


    /// Return Type: int
    ///handle: int
    ///deviceLabel: char*
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCSetDeviceLabel")]
    public static extern int RCSetDeviceLabel(int handle, System.IntPtr deviceLabel);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCStartOutput")]
    public static extern int RCStartOutput(int handle);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCStopOutput")]
    public static extern int RCStopOutput(int handle);


    /// Return Type: int
    ///handle: int
    ///timeout: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCWaitForReady")]
    public static extern int RCWaitForReady(int handle, int timeout);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCMaxSpeed")]
    public static extern int RCMaxSpeed(int handle);


    /// Return Type: int
    ///handle: int
    ///points: RCPoint*
    ///count: unsigned int
    ///speed: unsigned int
    ///repeat: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCWriteFrame")]
    public static extern int RCWriteFrame(int handle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] 
        RCPoint[] points, uint count, uint speed, uint repeat);


    /// Return Type: int
    ///handle: int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCUniverseCount")]
    public static extern int RCUniverseCount(int handle);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///universeName: char*
    ///maxLength: unsigned int
    ///pUniverseDirection: RCUniverseDirection*
    ///pChannelCount: unsigned int*
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCUniverseQuery")]
    public static extern int RCUniverseQuery(int handle, uint universeIndex, System.IntPtr universeName, uint maxLength, ref RCUniverseDirection pUniverseDirection, ref uint pChannelCount);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///startChannel: unsigned int
    ///data: unsigned char*
    ///count: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCUniverseWrite")]
    public static extern int RCUniverseWrite(int handle, uint universeIndex, uint startChannel, System.IntPtr data, uint count);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    ///startChannel: unsigned int
    ///data: unsigned char*
    ///count: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCUniverseRead")]
    public static extern int RCUniverseRead(int handle, uint universeIndex, uint startChannel, System.IntPtr data, uint count);


    /// Return Type: int
    ///handle: int
    ///universeIndex: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("RayComposer", EntryPoint = "RCUniverseUpdate")]
    public static extern int RCUniverseUpdate(int handle, uint universeIndex);

}
