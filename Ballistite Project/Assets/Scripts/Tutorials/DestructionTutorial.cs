using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestructionTutorial : MonoBehaviour
{
    private TutorialState state;
    private Rigidbody2D playerRB;
    private Platformer.Mechanics.BespokePlayerController playerObject;

    public GameObject tutorialWindow;
    public GameObject destructElement;
    public Animation siloOutside;
    //public Animator mouseAnimator;
    //public TextMeshProUGUI textBox;

    enum TutorialState
    {
        Untouched,
        Activated,
        Grounded,
        Released,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = TutorialState.Untouched;
        playerObject = FindObjectOfType<Platformer.Mechanics.BespokePlayerController>();
        playerRB = playerObject.GetComponentInParent<Rigidbody2D>();
        tutorialWindow.SetActive(false);
        //StartCoroutine(StateUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TutorialState.Activated)
        {
            if (playerObject.grounded == true)
            {
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | 0;
                state = TutorialState.Grounded;
            }
        }
        if (state == TutorialState.Grounded)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
        if (destructElement == null && state != TutorialState.Released)
        {
            playerRB.constraints = 0 | 0;
            state = TutorialState.Released;
            tutorialWindow.SetActive(false);
            siloOutside.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == TutorialState.Untouched && collision.CompareTag("Player"))
        {
            state = TutorialState.Activated;
            tutorialWindow.SetActive(true);
        }
    }

    IEnumerator StateUpdate()
    {
        Debug.Log("ChargeTutorial: " + state);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StateUpdate());
    }
}
