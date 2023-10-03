using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public GameEvent onCutsceneEndedEvent;
    public Animator cutsceneAnimator;
    public Animator armyPathAnimator;
    public Animator tankPathAnimator;
    public Animator backgroundAnimator;
    public Animator tankPlayerAnimator;
    public Animator letterboxAnimator;

    public ParallaxController parallaxObject;
    public GameObject introObjects;
    public GameObject mineObject;
    public GameObject playerGun_Main;
    public GameObject playerGun_Fake;
    public GameObject chargeUI;
    public GameObject playerObj;
    public GameObject grasslandMusic;

    public bool isLevel1;

    public ParticleSystem explosionEffectMain;

    private string cutsceneName;

    public void CutsceneIntro()
    {
        //TODO: Change camera mode
        cutsceneName = "Intro";
        cutsceneAnimator.SetTrigger("PlayIntro");
        letterboxAnimator.SetTrigger("Entry");
    }
    
    public void Start() 
    {
        if (isLevel1) cutsceneAnimator.SetTrigger("MainMenu");
    }

    public void IntroCutsceneEnded()
    {
        CutsceneEventData eventData = new CutsceneEventData { Sender = this, CutsceneName = cutsceneName };
        onCutsceneEndedEvent.Raise(eventData);
        playerGun_Main.SetActive(true);
        playerGun_Fake.SetActive(false);
        playerObj.GetComponent<LineRenderer>().enabled = true;
        chargeUI.SetActive(true);
        grasslandMusic.SetActive(true);
        if (parallaxObject != null)
            parallaxObject.ScrollerMode = false;
        // TODO: Change camera mode.
    }

    public void ExitArmy()
    {
        armyPathAnimator.SetTrigger("Exit");
    }

    public void IntroTankPath()
    {
        tankPathAnimator.SetTrigger("SlowDown");
    }

    public void ExplodeMine()
    {

        explosionEffectMain.Play();
        mineObject.SetActive(false);
        tankPlayerAnimator.SetTrigger("BlowUp");
    }

    public void DisableIntroObjects()
    {
        introObjects.SetActive(false);
    }

    public void DisableFakeBG()
    {
        backgroundAnimator.SetTrigger("Exit");
    }

    public void AddLetterbox()
    {
        if (letterboxAnimator != null)
            letterboxAnimator.SetTrigger("Entry");
    }

    public void RemoveLetterbox()
    {
        if (letterboxAnimator != null)
            letterboxAnimator.SetTrigger("Exit");
    }
}
