using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    private bool triggered = false;
    public bool respawnable = false;

    [SerializeField] private float collapseDelay = 3f;
    private float cdTimer;
    [SerializeField] private float respawnTime = 3f;
    private float rtTimer;

    private Animator anim;
    public string triggerAnimation;
    public string stopAnimation;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCollapse()
    {
        if (!triggered)
        {
            triggered = true;
            anim.Play(triggerAnimation);
            StartCoroutine(DestroyObject());
        }

    }

    private IEnumerator DestroyObject()
    {
        // Wait for the specified amount of time
        for (cdTimer = collapseDelay; cdTimer > 0; cdTimer -= Time.deltaTime)
            yield return null;

        // Hides the existing object
        ShowSelf(false);
        anim.Play(stopAnimation);

        if (respawnable)
            StartCoroutine(RespawnObject());

    }
    private IEnumerator RespawnObject()
    {
        // Wait for the specified amount of time
        for (rtTimer = respawnTime; rtTimer > 0; rtTimer -= Time.deltaTime)
            yield return null;

        // Show the object
        ShowSelf(true);
        triggered = false;
    }

    private void ShowSelf(bool state)
    {
        Debug.Log("ShowingSelf Active");
        if (state)
        {
            Debug.Log("Show");
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }
        else
        {
            Debug.Log("Hide");
            GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f); 
        }

        GetComponent<Collider2D>().enabled = state;
    }
}
