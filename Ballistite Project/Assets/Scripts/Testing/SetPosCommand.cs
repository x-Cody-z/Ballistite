using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetPosCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "setpos", "set_pos", "set-pos", "posset", "pos_set", "pos-set", "sp" }; }
        set { }
    }
    public override string description
    {
        get { return "Set Position: sets the player's co-ordinates to the give x,y,z position (setpos 0 0 0)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        if (GameObject.Find("Player"))
        {
            GameObject p = GameObject.Find("Player");
            float[] coords = new float[] { 0, 0, 1 };
            for (int i = 1; i < input.Length; i++)
            {
                if (float.TryParse(input[i], out float result))
                {
                    coords[i - 1] = result;
                }
                else
                {
                    return "Invalid co-ordinate input, must be whole or decimal numbers";
                }
            }
            p.transform.position = new Vector3(coords[0], coords[1], coords[2]);
            return "Teleported player to " + coords[0].ToString() + ", " + coords[1].ToString() + ", " + coords[2].ToString();
        }
        else
        {
            return "Unable to find Player object";
        }
    }
}
