using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float explosionRadius = 2f;
    [Tooltip("every one increase in this value is one grid unit of vertical movement")]
    public float explosionForce = 2f;
    public LayerMask explosionLayers;
    public AudioClip explosionSound;

    private AudioSource soundMachine;
    private ParticleSystem explosionEffect;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        soundMachine = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get all colliders within explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayers);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculate direction and distance from explosion center to collider
                Vector2 direction = rb.transform.position - transform.position;
                float distance = direction.magnitude;

                Debug.Log("Distance from explosion center: " + distance);

                if (distance < 0.5f)
                    distance = 0f;
                else
                    distance -= 0.4f;

                // Apply impulse force to collider based on distance and explosion force
                rb.AddForce(direction.normalized * calcExplosion(distance), ForceMode2D.Impulse);
            }
        }
        explosionEffect.Play();
        soundMachine.PlayOneShot(explosionSound);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        bc.enabled = false;
        sp.enabled = false;
        Destroy(gameObject, explosionEffect.main.duration);
    }

    private float calcExplosion(float dist)
    {
        float adjustmentValue = 4.62f * Mathf.Pow(explosionForce,0.5f);
        return adjustmentValue * (1 - dist / explosionRadius);
    }

    private void OnDrawGizmos()
    {
        // Draw a wire sphere around the explosion object to visualize the explosion radius in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
