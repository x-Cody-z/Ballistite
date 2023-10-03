using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public AudioSource MpPlayer;
    public AudioClip FirstClip;
    public AudioClip SecondClip;

    void Start()
    {
        MpPlayer.clip = FirstClip;
        MpPlayer.loop = false;
        MpPlayer.Play();
        StartCoroutine(WaitForTrackTOend());
    }

    IEnumerator WaitForTrackTOend()
    {
        while (MpPlayer.isPlaying)
        {

            yield return new WaitForSeconds(0.01f);

        }
        MpPlayer.clip = SecondClip;
        MpPlayer.loop = true;
        MpPlayer.Play();

    }
}
