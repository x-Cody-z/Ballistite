using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventData
{
    public Component Sender { get; set; }
}
public class ProgressEventData : GameEventData // You can expand the event system like so
{
    public float Goal { get; set; }
}
public class CutsceneEventData : GameEventData
{
    public string CutsceneName { get; set; }
}
public class PlayerEventData : GameEventData
{
    public float BlastValue { get; set; }
}

public class ProjectileEventData : GameEventData
{
    public Transform HitPosition { get; set; }
    public Vector2 velocity { get; set; }
    public float radius { get; set; }

    public Vector2 hitNormal { get; set; }
}

public class ObjectDesotryedEventData : GameEventData
{
    public int destructionValue { get; set; }
}
