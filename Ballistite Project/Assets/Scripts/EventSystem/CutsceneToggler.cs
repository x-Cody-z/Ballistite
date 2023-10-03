using Cinemachine;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineToControl : MonoBehaviour
{
    public BespokePlayerController PlayerControl;
    public LineRenderer PlayerLine;
    public CinemachineVirtualCamera CutsceneCamera;
    public GameObject PlayerObject;
    public string CutsceneName;
    public GameEvent onCutsceneEndedEvent;
    public Animator LetterboxAnim;
    public bool isStartingCutscene;

    public GameObject FakeBarrel;
    public GameObject RealBarrel;
    public GameObject ChargeUI;

    public void CutsceneStarted()
    {
        PlayerControl.EnableCutscene(true);
        RealBarrel.SetActive(false);
        FakeBarrel.SetActive(true);
        ChargeUI.SetActive(false);

        if (isStartingCutscene)
        {
            LetterboxAnim.SetTrigger("Default");
        } else
        {
            LetterboxAnim.SetTrigger("Entry");
            PlayerObject.SetActive(false);
        }

        CutsceneCamera.Priority = 13;
        PlayerLine.textureScale = new Vector2(0f, 0f);
    }

    public void CutsceneEnded()
    {
        PlayerControl.EnableCutscene(false);
        RealBarrel.SetActive(true);
        FakeBarrel.SetActive(false);
        ChargeUI.SetActive(true);
        CutsceneCamera.Priority = 8;
        PlayerLine.textureScale = new Vector2(1.75f, 1f);
        LetterboxAnim.SetTrigger("Exit");
    }
}
