using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaddyAnimator : MonoBehaviour
{
    public string variableName;
    public Animator controlCaddy;
    public bool isOnTrigger;
    public bool isOnClick;
    private bool enabled;
    private bool activated;
    
    // Start is called before the first frame update
    void Start()
    {
        controlCaddy = FindObjectOfType<Animator>();
        activated = false;
    }

    private void Update()
    {
        if (isOnClick && !activated && enabled)
        {
            if (Input.GetButton("Fire1"))
            {
                controlCaddy.SetBool(variableName, true);
                activated = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOnTrigger && !activated)
        {
            if (collision.CompareTag("Player"))
            {
                controlCaddy.SetBool(variableName, true);
                activated = true;
            }
        }
    }

    public void ActivateOnClick()
    {
        enabled = true;
    }
}
