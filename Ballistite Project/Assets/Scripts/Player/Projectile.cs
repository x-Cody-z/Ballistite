using Platformer.Mechanics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float explosionRadius = 2f;
    [Tooltip("every one increase in this value is one grid unit of vertical movement")]
    public float explosionForce = 2f;
    private float chargeScale;
    public LayerMask explosionLayers;
    public AudioClip explosionSound;

    private AudioSource soundMachine;
    public ParticleSystem explosionEffectMain;
    public ParticleSystem explosionEffectSmoke;
    public ParticleSystem explosionEffectSpark;
    private GameObject PlayerObject;
    private BespokePlayerController PlayerScript;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sp;
    private ParticleSystem.MainModule explosionmain;
    private ParticleSystem.MainModule explosionsmoke;
    private ParticleSystem.MainModule explosionspark;
    public GameObject graphic;

    public event EventHandler OnProjectileHitTerrain;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.Find("Player");
        if (PlayerObject) {
            PlayerScript = PlayerObject.GetComponent<BespokePlayerController>();
        }
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponentInChildren<SpriteRenderer>();
        soundMachine = GetComponent<AudioSource>();
        explosionmain = explosionEffectSpark.main;
        explosionsmoke = explosionEffectSmoke.main;
        explosionspark = explosionEffectSpark.main;
        chargeScale = PlayerScript.LastBlastValue;
        Debug.Log("Timer = " + chargeScale);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 flightDir = rb.velocity.normalized;
        if(flightDir.x > 0 )
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, flightDir);
            graphic.transform.rotation = Quaternion.Lerp(graphic.transform.rotation, targetRotation, Time.deltaTime * 0.6f);
        } else
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -flightDir);
            graphic.transform.rotation = Quaternion.Lerp(graphic.transform.rotation, targetRotation, Time.deltaTime * 0.6f);
        }
        if (chargeScale < 1) {
            explosionmain.startSize = 10;
            explosionsmoke.startSize = 4;
            explosionspark.startSize = 4;
        } else {
            explosionmain.startSize = 12;
            explosionsmoke.startSize = 8;
            explosionspark.startSize = 8;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get all colliders within explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayers);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (collider.CompareTag("Level"))
            {
                OnProjectileHitTerrain?.Invoke(this, null);
            }
            else
            {
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
        }

        explosionEffectMain.Play();
        Debug.Log(chargeScale);
        soundMachine.PlayOneShot(explosionSound);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        bc.enabled = false;
        sp.enabled = false;
        Destroy(gameObject, explosionEffectMain.main.duration);
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
