using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHInge : MonoBehaviour
{
    private HingeJoint2D hinge;
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            hinge.enabled = false;
        }
    }
}
