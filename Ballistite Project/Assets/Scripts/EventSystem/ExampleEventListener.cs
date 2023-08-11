using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEventListener : MonoBehaviour 
    /* There's a lot of event listeners because any of these functions could be on any object you want to control.
     * It's just an example with a bunch of things put together in a single place, after all. */
{
    public void OnEnemyDestroyed(GameEventData eventData)
    {
        Debug.Log("Enemy destroy goal met.");
    }

    public void OnEnvironmentDestroyed(GameEventData eventData)
    {
        Debug.Log("Environment destroy goal met.");
    }

    public void OnTimeMet(GameEventData eventData)
    {
        Debug.Log("Time goal met.");
    }

    public void OnProgressMet(GameEventData eventData)
    {
        if (eventData is ProgressEventData progressData)
        {
            Debug.Log("Progress goal met. The goal met was " + progressData.Goal);
        }
    }

    public void PrintBlastValue(GameEventData eventData)
    {
        if (eventData is PlayerEventData playerData)
        {
            Debug.Log("Tank blast was a value of " + playerData.BlastValue);
        }
    }
}