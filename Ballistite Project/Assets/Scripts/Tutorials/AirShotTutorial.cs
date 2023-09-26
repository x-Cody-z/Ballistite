using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShotTutorial : MonoBehaviour
{
    public GameObject tutorialWindow;
    public float freezeDelay = 0.5f;

    private TutorialState state;
    private Vector2 forceGrab;
    
    private Platformer.Mechanics.BespokePlayerController playerObject;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;    

    enum TutorialState
    {
        Untouched,
        Activated,
        Grabbed,
        Released,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = TutorialState.Untouched;
        playerObject = FindObjectOfType<Platformer.Mechanics.BespokePlayerController>();
        playerCollider = playerObject.GetComponent<Collider2D>();
        playerRB = playerObject.GetComponentInParent<Rigidbody2D>();
        tutorialWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TutorialState.Grabbed && Input.GetButtonUp("Fire1"))
        {
            playerRB.constraints = 0|0|0;
            playerRB.AddForce(forceGrab);
            state = TutorialState.Released;
            tutorialWindow.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == TutorialState.Untouched && collision.tag == "Player")
        {
            state = TutorialState.Activated;
            tutorialWindow.SetActive(true);
            StartCoroutine(WaitToFreeze());
        }
    }

    IEnumerator WaitToFreeze()
    {
        yield return new WaitForSeconds(freezeDelay);
        forceGrab = playerRB.totalForce;
        playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        state = TutorialState.Grabbed;
    }
}