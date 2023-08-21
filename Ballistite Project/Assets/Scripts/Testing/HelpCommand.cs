using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelpCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "help" }; }
        set { }
    }
    public override string description
    {
        get { return "Commands:"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        GameObject ttObj = GameObject.Find("TestingTools");
        TestingTools ttScript = ttObj.GetComponent<TestingTools>();
        return ttScript.help();
    }
}
