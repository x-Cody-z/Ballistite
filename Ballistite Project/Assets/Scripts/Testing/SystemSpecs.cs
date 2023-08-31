using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Management;
using Microsoft.Win32;
using System.IO;
using System;



public class SystemSpecs : MonoBehaviour
{
    public void getOperatingSystemInfo()
    {
        Console.WriteLine("Displaying operating system info....\n");
        //Create an object of ManagementObjectSearcher class and pass query as parameter.
        ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
        foreach (ManagementObject managementObject in mos.Get())
        {
            if (managementObject["Caption"] != null)
            {
                Console.WriteLine("Operating System Name  :  " + managementObject["Caption"].ToString());   //Display operating system caption
            }
            if (managementObject["OSArchitecture"] != null)
            {
                Console.WriteLine("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
            }
            if (managementObject["CSDVersion"] != null)
            {
                Console.WriteLine("Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString());     //Display operating system version.
            }
        }
    }

    public void getProcessorInfo()
    {
        Console.WriteLine("\n\nDisplaying Processor Name....");
        RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

        if (processor_name != null)
        {
            if (processor_name.GetValue("ProcessorNameString") != null)
            {
                Console.WriteLine(processor_name.GetValue("ProcessorNameString"));   //Display processor ingo.
            }
        }
    }
}
