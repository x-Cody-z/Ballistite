using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestructibleForce : MonoBehaviour
{
    private Vector2 forceBase;
    [SerializeField] private int forceMultiplier = 100;
    private int torqueBase;
    private Rigidbody2D rb;

    [SerializeField] private GameObject parentObj;
    private Vector2 distFromParent;

    // Start is called before the first frame update
    void Start()
    {
        distFromParent = this.transform.position - parentObj.transform.position;
        forceBase = distFromParent.normalized;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(forceBase*forceMultiplier);
        //rb.AddTorque(torqueBase);
    }
}
