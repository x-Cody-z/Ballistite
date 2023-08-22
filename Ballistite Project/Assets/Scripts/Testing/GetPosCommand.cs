using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GetPosCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "getpos", "get_pos", "get-pos", "posget", "pos_get", "pos-get", "gp" }; }
        set { }
    }
    public override string description
    {
        get { return "Get Position: returns the player object's current x,y and z co-ordinates (getpos)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        if (GameObject.Find("Player"))
        {
            GameObject p = GameObject.Find("Player");
            return "Player position: (" + p.transform.position.x.ToString() + ", " + p.transform.position.y.ToString() + ", " + p.transform.position.z.ToString() + ")";
        }
        else
        {
            return "Unable to find Player object";
        }
    }
}
