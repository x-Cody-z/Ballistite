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

    private float animTimer;

    public Vector3 projectilePos;
    private Vector2 distFromProj;
    private Vector2 forceExplosion;

    // Start is called before the first frame update
    void Start()
    {
        distFromParent = this.transform.position - parentObj.transform.position;
        forceBase = distFromParent.normalized;

        distFromProj = this.transform.position - projectilePos;
        forceExplosion = distFromProj.normalized;

        Vector2 forceTotal = ((forceExplosion * 1.5f) + forceBase) * forceMultiplier;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(forceTotal);
        //rb.AddTorque(torqueBase);

        StartCoroutine(AnimWait());
    }

    private IEnumerator AnimWait()
    {
        for (animTimer = 4f; animTimer > 0; animTimer -= Time.deltaTime)
            yield return null;

        Destroy(gameObject);
    }
}
