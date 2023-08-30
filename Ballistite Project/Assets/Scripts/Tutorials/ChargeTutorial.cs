using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTutorial : MonoBehaviour
{

    private LevelController levelController;
    private TutorialState state;
    private Rigidbody2D playerRB;

    public Collider2D trigger;
    public Platformer.Mechanics.BespokePlayerController playerObject;
    public Collider2D playerCollider;
    public float pSlowdownRate;


    enum TutorialState
    {
        Untouched,
        Activated,
        Grounded,
        Charging,
        Released,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = TutorialState.Untouched;
        levelController = FindObjectOfType<LevelController>();
        playerObject = FindObjectOfType<Platformer.Mechanics.BespokePlayerController>();
        playerCollider = playerObject.GetComponent<Collider2D>();
        playerRB = playerObject.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TutorialState.Activated || state == TutorialState.Grounded || state == TutorialState.Charging)
        {
            playerObject.shotCount = playerObject.shotNumber;
        }
        if (state == TutorialState.Activated)
        {
            if(playerObject.grounded == true)
            {
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                state = TutorialState.Grounded;
            }
        }
        if (state == TutorialState.Grounded)
        {
            if (Input.GetButtonDown("Fire1") && !playerObject.charge3)
            {
                state = TutorialState.Charging;
            }
        }
        if (state == TutorialState.Charging)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                if (playerObject.charge3)
                {
                    state = TutorialState.Released;
                    playerRB.constraints = 0| 0;
                }
                else
                {
                    playerObject.Timer = 0;
                    playerObject.paused = true;
                    //StartCoroutine(Cooldown(0.2f));
                    playerObject.shotCancel = true;

                    playerObject.charge1 = false;
                    playerObject.charge2 = false;
                    playerObject.charge3 = false;
                    state = TutorialState.Charging;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == TutorialState.Untouched)
        {
            state = TutorialState.Activated;
        }
    }
}
