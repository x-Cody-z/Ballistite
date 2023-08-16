using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Vector3 resetPoint;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        resetPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.velocity = Vector3.zero;
            transform.position = resetPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  //checkpoints just need an empty object and a collider with the 'Checkpoint' tag
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            resetPoint = collision.transform.position;
        }
    }
}
