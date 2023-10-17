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
    public Animation siloOutside;
    public bool freezeEnabled;
    //public Animator mouseAnimator;

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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == TutorialState.Untouched && collision.CompareTag("Player"))
        {
            if (freezeEnabled)
            {
                state = TutorialState.Activated;
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                tutorialWindow.SetActive(true);
            }
        }
    }

    IEnumerator StateUpdate()
    {
        Debug.Log("ChargeTutorial: " + state);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StateUpdate());
    }

    public void ReleasePlayer()
    {
        state = TutorialState.Released;
        playerRB.constraints = 0 | 0;
        tutorialWindow.SetActive(false);
        siloOutside.Play();
    }
}
