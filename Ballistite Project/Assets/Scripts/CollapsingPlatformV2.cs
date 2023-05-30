using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatformV2 : MonoBehaviour
{
    private bool triggered = false;
    public bool respawnable = false;

    [SerializeField] private float collapseDelay = 3f;
    private float cdTimer;
    [SerializeField] private float respawnTime = 3f;
    private float rtTimer;

    private Animator anim;
    public string animationName;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && triggered == false)
        {
            triggered = true;
            anim.Play(animationName);
            Debug.Log("I'm playing!");
            cdTimer = collapseDelay;
            rtTimer = respawnTime;
            StartCoroutine(DestroyObject());
        }

    }

    private IEnumerator DestroyObject()
    {
        // Wait for the specified amount of time
        for (cdTimer = 1f; cdTimer > 0; cdTimer -= Time.deltaTime)
            yield return null;

        // Hides the existing object
        SwapState(false);

        if (respawnable)
            StartCoroutine(RespawnObject());

    }
    private IEnumerator RespawnObject()
    {
        // Wait for the specified amount of time
        for (rtTimer = 1f; rtTimer > 0; rtTimer -= Time.deltaTime)
            yield return null;

        // Show the object
        SwapState(true);
        anim.StopPlayback();
        Debug.Log("I've stopped!");
        triggered = false;
    }

    private void SwapState(bool state)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = state;
        gameObject.GetComponent<Collider2D>().enabled = state;
    }
}
