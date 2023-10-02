using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOnCol : MonoBehaviour
{
    private Animator anim;
    public string animationName;
   
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision!");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player!");
            anim.SetTrigger("PlayerDetected");
        }
    }
}
