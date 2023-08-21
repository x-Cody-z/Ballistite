using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : MonoBehaviour
{
    public abstract string[] ids { get; set; }

    public abstract string description { get; set; }

    public abstract string processComand(string[] input);
}
