using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelTutorial : MonoBehaviour
{
    private LevelController levelController;
    private TutorialState state;
    private Rigidbody2D playerRB;
    private Platformer.Mechanics.BespokePlayerController playerObject;
    private Collider2D playerCollider;

    public GameObject tutorialWindow;
    public Animator mouseAnimator;
    public TextMeshProUGUI textBox;

    enum TutorialState
    {
        Untouched,
        Activated,
        Grounded,
        Charging,
        Charged,
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
        tutorialWindow.SetActive(false);
        //StartCoroutine(StateUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject.ChargeTimer >= 5 && (state != TutorialState.Untouched || state != TutorialState.Released))
        {
            playerObject.ChargeTimer = 5;
        }
        if (playerObject.charge3 == true && tutorialWindow.activeSelf)
        {
            state = TutorialState.Charged;
            playerObject.EnableControl(false);
            playerObject.chargePaused = true;
            textBox.text = "Release";
            mouseAnimator.Play("Release");
        }
        if (state == TutorialState.Activated)
        {
            if (playerObject.grounded == true)
            {
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | 0;
                state = TutorialState.Grounded;
                textBox.text = "Press";
                mouseAnimator.Play("Prompt");
            }
        }
        if (state == TutorialState.Grounded)
        {
            if (Input.GetButtonDown("Fire1") && !playerObject.charge3)
            {
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                state = TutorialState.Charging;
                textBox.text = "Hold";
                mouseAnimator.Play("Charging");
            }
        }
        if (state == TutorialState.Charging)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                /*playerObject.shotCancel = true;
                //Debug.LogWarning("Not charged");
                playerObject.ResetCharge();
                state = TutorialState.Grounded;
                mouseAnimator.Play("Prompt");
                textBox.text = "Press";
                playerObject.shotCancel = false;*/
            }
        }
        if (state == TutorialState.Charged)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                //Debug.LogWarning("Released");
                /*state = TutorialState.Released;
                playerRB.constraints = 0 | 0;
                tutorialWindow.SetActive(false);*/
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == TutorialState.Untouched && collision.tag == "Player")
        {
            playerObject.charge3 = false;
            state = TutorialState.Activated;
            tutorialWindow.SetActive(true);
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
