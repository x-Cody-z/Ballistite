using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    float startTime;

    // Start is called before the first frame update
    void Awake()
    {
        startTime = Time.timeScale;
    }

    public void PauseGameTime()
    {
        Time.timeScale = 0;
    }

    public void PlayGameTime()
    {
        Time.timeScale = startTime;
    }
}
