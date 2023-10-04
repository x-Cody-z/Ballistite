using Cinemachine;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera normalCam;
    [SerializeField] CinemachineVirtualCamera shakeCam;
    [SerializeField] BespokePlayerController player;
    [SerializeField] float shakeTime = 2;
    float shakeTimer = 0;
    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        if (triggered)
        {
            player.EnableCutscene(true);
            while (shakeTimer <= shakeTime)
            {
                shakeCam.Priority = 12;
            }
            shakeCam.Priority = 10;
            player.EnableCutscene(false);
        } 
    }

    private void Update()
    {
        if (triggered)
        {
            shakeTimer += Time.deltaTime;
        }
    }
}
