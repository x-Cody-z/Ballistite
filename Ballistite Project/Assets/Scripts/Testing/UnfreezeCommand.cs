using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnfreezeCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "unfreeze", "uf" }; }
        set { }
    }
    public override string description
    {
        get { return "Unfreeze: removes all contraints from the player rigidbody (unfreeze)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        if (GameObject.Find("Player"))
        {
            Rigidbody2D pRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            if (pRb != null)
            {
                pRb.constraints = RigidbodyConstraints2D.None;
                return "Player Unfrozen";
            }
            else
            {
                return "Player has no rigidbody component";
            }
        }
        else
        {
            return "Unable to find Player object";
        }
    }
}
