using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public GameEvent onCutsceneEndedEvent;
    public Animator cutsceneAnimator;
    public Animator letterboxAnimator;
    public ParallaxController parallaxObject;

    private string cutsceneName;

    public void CutsceneIntro()
    {
        //TODO: Change camera mode
        cutsceneName = "Intro";
        cutsceneAnimator.SetTrigger("PlayIntro");
    }

    public void CutsceneEnded()
    {
        CutsceneEventData eventData = new CutsceneEventData { Sender = this, CutsceneName = cutsceneName };
        onCutsceneEndedEvent.Raise(eventData);
        parallaxObject.ScrollerMode = false;

        // TODO: Change camera mode.
    }
    public void AddLetterbox()
    {
        letterboxAnimator.SetTrigger("Entry");
    }

    public void RemoveLetterbox()
    {
        letterboxAnimator.SetTrigger("Exit");
    }
}
