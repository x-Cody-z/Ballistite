using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public GameEvent onCutsceneEndedEvent;
    public Animator cutsceneAnimator;

    private string cutsceneName;

    public void TriggerIntro()
    {
        //TODO: Change camera mode
        cutsceneName = "Intro";
        cutsceneAnimator.SetTrigger("PlayIntro");
    }

    public void CutsceneEnded()
    {
        CutsceneEventData eventData = new CutsceneEventData { Sender = this, CutsceneName = cutsceneName };
        onCutsceneEndedEvent.Raise(eventData);

        // TODO: Change camera mode.
    }
}
