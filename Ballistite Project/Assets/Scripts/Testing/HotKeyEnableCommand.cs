using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HotKeyEnableCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "hotkey", "hotkey_toggle", "hotkey_enable", "toggle_hotkey", "enable_hotkey" }; }
        set { }
    }
    public override string description
    {
        get { return "Enable Hotkeys: Turns hotkeys on/ off, such as 1-5 for scenes or arrowkeys for teleporting. (hotkey)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        GameObject ttObj = GameObject.Find("TestingTools");
        TestingTools ttScript = ttObj.GetComponent<TestingTools>();
        if (ttScript.hotkeysEnabled)
        {
            ttScript.hotkeysEnabled = !ttScript.hotkeysEnabled;
            return "Hotkeys have been disabled";
        }
        else
        {
            ttScript.hotkeysEnabled = !ttScript.hotkeysEnabled;
            return "Hotkeys have been enabled";
        }
    }
}
