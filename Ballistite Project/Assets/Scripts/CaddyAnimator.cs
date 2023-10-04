using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaddyAnimator : MonoBehaviour
{
    public string variableName;
    public Animator controlCaddy;
    public bool isOnTrigger;
    public bool isOnClick;
    private bool activated;
    private bool enable;
    
    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        enable = false;
    }

    private void Update()
    {
        if (isOnClick && !activated && enable)
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
        enable = true;
    }
}
