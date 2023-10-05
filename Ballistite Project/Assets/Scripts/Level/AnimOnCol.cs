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

        if (collision.gameObject.CompareTag("Player"))
        {
            anim.Play(animationName);
        }
    }
}
