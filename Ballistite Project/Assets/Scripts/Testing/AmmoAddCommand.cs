using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddAmmoCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "add_ammo", "addammo", "add-ammo", "ammo", "aa" }; }
        set { }
    }
    public override string description
    {
        get { return "Add Ammo: adds x bullets to the players ammo count (add_ammo x)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        if (GameObject.Find("Player"))
        {
            GameObject p = GameObject.Find("Player");
            int ammo = 1;
            if (input.Length > 1)
            {
                if (int.TryParse(input[1], out int result))
                {
                    ammo = result;
                }
            }
            Shooter pShooter = p.GetComponent<Shooter>();
            pShooter.shotCount += ammo;
            return "Added " + ammo.ToString() + " bullet. Player's ammo count is now " + pShooter.shotCount.ToString();
        }
        else
        {
            return "Unable to find Player object";
        }
    }
}
