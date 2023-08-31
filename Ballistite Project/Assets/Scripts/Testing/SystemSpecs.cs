using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;



public class SystemSpecs : MonoBehaviour
{
    public string getSpecs()
    {
        string output = "";

        output += "Device Model: " + SystemInfo.deviceModel + "\n";
        output += "Operating System: " + SystemInfo.operatingSystem + "\n\n";
        
        output += "GPU: " + SystemInfo.graphicsDeviceName + "\n";
        output += "GPU Memory (MB): " + SystemInfo.graphicsMemorySize + "\n\n";

        output += "CPU: " + SystemInfo.processorType + "\n";
        output += "CPU Cores: " + SystemInfo.processorCount + "\n";
        output += "CPU Frequency (MHz): " + SystemInfo.processorFrequency + "\n\n";

        output += "RAM (MB): " + SystemInfo.systemMemorySize + "\n";

        return output;
    }
}
