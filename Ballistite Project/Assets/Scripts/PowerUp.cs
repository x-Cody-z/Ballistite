using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private AudioSource soundMachine;
    public AudioClip collectSFX;

    private GameObject PlayerObject;
    private uiController UiScript;
    private BespokePlayerController PlayerScript;


    // Start is called before the first frame update
    void Start()
    {
        soundMachine = GetComponent<AudioSource>();

        PlayerObject = GameObject.Find("Player");
        if (PlayerObject)
        {
            PlayerScript = PlayerObject.GetComponent<BespokePlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Coin()
    {
        soundMachine.PlayOneShot(collectSFX, 0.4f);
    }
    void Ammo()
    {
        soundMachine.PlayOneShot(collectSFX, 0.4f);
        PlayerScript.IncreaseAmmo(1, "");

    }

    void Ricochet()
    {
        soundMachine.PlayOneShot(collectSFX, 0.4f);
        PlayerScript.IncreaseAmmo(1, "ricochet");
    }
}
