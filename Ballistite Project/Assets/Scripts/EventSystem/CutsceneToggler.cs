using Cinemachine;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineToControl : MonoBehaviour
{
    public BespokePlayerController Player;
    public LineRenderer PlayerLine;
    public CinemachineVirtualCamera CutsceneCamera;
    public string CutsceneName;
    public GameEvent onCutsceneEndedEvent;

    public GameObject FakeBarrel;
    public GameObject RealBarrel;
    public GameObject ChargeUI;

    public void CutsceneStarted()
    {
        Player.EnableCutscene(true);
        RealBarrel.SetActive(false);
        FakeBarrel.SetActive(true);
        ChargeUI.SetActive(false);
        CutsceneCamera.Priority = 13;
        PlayerLine.textureScale = new Vector2(0f, 0f);
    }

    public void CutsceneEnded()
    {
        Player.EnableCutscene(false);
        RealBarrel.SetActive(true);
        FakeBarrel.SetActive(false);
        ChargeUI.SetActive(true);
        CutsceneCamera.Priority = 8;
        PlayerLine.textureScale = new Vector2(1.75f, 1f);
    }
}
