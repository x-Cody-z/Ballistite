using Platformer.Mechanics;
using System;
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
        string date = DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(":", ",");
        string path = @"Console Logs/log(" + date + ").txt";
        
        Directory.CreateDirectory(@"Console Logs");

        if (!File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Dispose();
        }


        TextWriter tw = new StreamWriter(path);
        tw.WriteLine("Console Text Log: ");
        tw.WriteLine(ttScript.logtext());

        if (GameObject.Find("Player"))
        {
            GameObject p = GameObject.Find("Player");
            Shooter psh = p.GetComponent<Shooter>();
            BespokePlayerController pcon = p.GetComponent<BespokePlayerController>();

            tw.WriteLine("\nShooter Debug Info:\n");
            tw.WriteLine(psh.getShooterState());

            tw.WriteLine("\nPlayer Controller Debug Info:\n");
            tw.WriteLine(pcon.getPlayerValues());

            tw.WriteLine("\nPlayer Object Debug Info:\n");
            tw.WriteLine("position: " + p.transform.position.ToString());
            tw.WriteLine("rotation: " + p.transform.eulerAngles.ToString());
            tw.WriteLine("scale: " + p.transform.localScale.ToString());
        }

        if (input.Length > 1)
        {
            tw.WriteLine("\nUser Written Error Message:");
            string errMsg = "";
            for(int i = 1; i < input.Length; i++)
            {
                errMsg += input[i] + " ";
            }
            tw.WriteLine(errMsg);
        }

        tw.Close();

        return "Log saved to: " + Path.GetFullPath(path);
    }

}
