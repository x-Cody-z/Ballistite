using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTutorial : MonoBehaviour
{
    public Collider2D trigger;
    private GameTime timeController;
    private bool playedBefore;
    private bool restoreControl;
    public Platformer.Mechanics.BespokePlayerController playerObject;
    public Collider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playedBefore = false;
        timeController = FindObjectOfType<GameTime>();
        playerObject = FindObjectOfType<Platformer.Mechanics.BespokePlayerController>();
        playerCollider = playerObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollider.IsTouching(trigger))
        {
            if (!playedBefore)
            {
                playedBefore = true;

            }
            if (Time.timeScale > 0)
            {
                
            }
        }
    }
}
