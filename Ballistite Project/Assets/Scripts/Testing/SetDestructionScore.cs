using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetDestructionScore : Command
{
    public override string[] ids
    {
        get { return new string[] { "destruction_score", "destruction-score", "destructionscore", "ds" }; }
        set { }
    }
    public override string description
    {
        get { return "Set Destruction Score: sets the scene's destruction score which affects the colour correction strength. value should be 0-100 (destruction_score [float])"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        if (GameObject.Find("DestructionManager"))
        {
            GameObject dm = GameObject.Find("DestructionManager");
            DestructionManager dmScript = dm.GetComponent<DestructionManager>();
            if (input.Length > 1)
            {
                if (float.TryParse(input[1], out float result))
                {
                    dmScript.destructionScore = result;
                    return ("Destruction score has been set to " + dmScript.destructionScore.ToString());
                }
            }
            //if no score value is given or if the input given is not a float, then display current score instead of setting it
            return ("Current destruction score is " + dmScript.destructionScore.ToString());
        }
        else
        {
            return "Unable to find DestructionManger object in scene";
        }
    }
}
