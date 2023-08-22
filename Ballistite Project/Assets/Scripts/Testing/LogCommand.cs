using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LogCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "log", "l" }; }
        set { }
    }
    public override string description
    {
        get { return "Log: gets all inputs and outputs from the console and creates a text document (log)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        GameObject ttObj = GameObject.Find("TestingTools");
        TestingTools ttScript = ttObj.GetComponent<TestingTools>();

        string path = @"console_log.txt";


        if (!File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Dispose();
        }


        TextWriter tw = new StreamWriter(path);
        tw.WriteLine(ttScript.logtext());
        tw.Close();

        return "Log saved to: " + Path.GetFullPath(path);
    }

}
