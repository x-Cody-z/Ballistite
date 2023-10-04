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
    [SerializeField] bool playerControl = false;
    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            if (playerControl)
            {
                StartCoroutine(Shake());
            } else
            {
                StartCoroutine(ShakeHalt());
            }
            triggered = true;
        }
    }

    IEnumerator Shake()
    {
        shakeCam.Priority = 12;
        yield return new WaitForSeconds(shakeTime);
        shakeCam.Priority = 10;
    }

    IEnumerator ShakeHalt()
    {
        player.EnableCutscene(true);
        shakeCam.Priority = 12;
        yield return new WaitForSeconds(shakeTime);
        shakeCam.Priority = 10;
        player.EnableCutscene(false);
    }
}
