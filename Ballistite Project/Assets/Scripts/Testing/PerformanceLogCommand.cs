using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PerformanceLogCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "performance_log", "pl", "performance-log", "performancelog", "performance" }; }
        set { }
    }
    public override string description
    {
        get { return "Performance Log: gets all inputs and outputs from the console and creates a text document (performance_log start/ stop)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        GameObject ttObj = GameObject.Find("TestingTools");
        TestingTools ttScript = ttObj.GetComponent<TestingTools>();
        if (input.Length > 1)
        {
            if (input[1] == "start")
            {
                if (!ttScript.fpsLogging)
                {
                    ttScript.logFpsStart();
                    return "Performance logging started!";
                }
                else
                {
                    return "Performance logging is already active, use \"performance_log stop\" to get results";
                }
            }
            else if (input[1] == "stop")
            {
                if (ttScript.fpsLogging)
                {
                    string path = @"performance_log.csv";

                    if (!File.Exists(path))
                    {
                        FileStream fs = new FileStream(path, FileMode.Create);
                        fs.Dispose();
                    }


                    List<float> data = ttScript.logFpsStop();
                    string outputLineFrameCount = "Frame Count,";
                    string outputLineFrameTime = "Frame Time,";
                    string outputLineFrameRate = "Frame Rate,";

                    string outputLineSeconds = "Seconds,";
                    string outputLineFrameRateAverage = "Average Frame Rate,";

                    float addedFrameTime = 0f;
                    int addedFrameCount = 0;
                    int addedIndex = 0;

                    for (int i = 0; i < data.Count; i++)
                    {
                        outputLineFrameCount += i + ",";
                        outputLineFrameTime += data[i].ToString() + ",";
                        outputLineFrameRate += (1 / data[i]).ToString() + ",";

                        if (addedFrameTime < 1f)
                        {
                            addedFrameTime += data[i];
                            addedFrameCount += 1;
                        }
                        else
                        {
                            addedIndex += 1;
                            outputLineFrameRateAverage += (addedFrameCount / addedFrameTime).ToString() + ",";
                            outputLineSeconds += addedIndex.ToString() + ",";
                            addedFrameCount = 0;
                            addedFrameTime = 0f;
                        }
                    }

                    TextWriter tw = new StreamWriter(path);
                    tw.WriteLine(outputLineFrameCount);
                    tw.WriteLine(outputLineFrameTime);
                    tw.WriteLine(outputLineFrameRate);
                    tw.WriteLine(outputLineSeconds);
                    tw.WriteLine(outputLineFrameRateAverage);
                    tw.Close();

                    return "Log saved to: " + Path.GetFullPath(path);
                }
                else
                {
                    return "Perfomance logging has not been started yet";
                }
            }
            else
            {
                return "Invalid use of performance_log command";
            }
        }
        else
            return "Invalid use of performance_log command";
    }

}
