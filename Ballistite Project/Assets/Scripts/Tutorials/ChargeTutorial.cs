using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTutorial : MonoBehaviour
{

    private LevelController levelController;
    private TutorialState state;
    private Rigidbody2D playerRB;
    private bool maxPower;

    public Platformer.Mechanics.BespokePlayerController playerObject;
    public Collider2D playerCollider;

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
        maxPower = false;
        StartCoroutine(StateUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject.charge3 == true)
        {
            maxPower = true;
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
                Debug.LogWarning("Charging button up");
                if (maxPower)
                {
                    Debug.LogWarning("Released");
                    state = TutorialState.Released;
                    playerRB.constraints = 0 | 0;
                }
                else
                {
                    Debug.LogWarning("Not charged");
                    playerObject.Timer = 0;
                    playerObject.paused = true;
                    playerObject.shotCancel = true;
                    playerObject.charge1 = false;
                    playerObject.charge2 = false;
                    playerObject.charge3 = false;
                    state = TutorialState.Grounded;
                }
                playerObject.shotCancel = false;
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

    IEnumerator StateUpdate()
    {
        Debug.Log("ChargeTutorial: " + state);
        Debug.Log("Charge 3 State: " + playerObject.charge3);
        yield return new WaitForSeconds(1f);
        StartCoroutine(StateUpdate());
    }
}
