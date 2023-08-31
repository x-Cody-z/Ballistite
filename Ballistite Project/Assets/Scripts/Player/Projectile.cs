using Platformer.Mechanics;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float explosionRadius = 2f;
    [Tooltip("every one increase in this value is one grid unit of vertical movement")]
    public float explosionForce = 2f;
    public LayerMask explosionLayers;
    public AudioClip explosionSound;

    public float radiusModifier;
    public float radius;
    public float radiusMax;

    private AudioSource soundMachine;
    public ParticleSystem explosionEffectMain;
    public ParticleSystem explosionEffectSmoke;
    public ParticleSystem explosionEffectSpark;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sp;
    private ParticleSystem.MainModule explosionmain;
    private ParticleSystem.MainModule explosionsmoke;
    private ParticleSystem.MainModule explosionspark;
    public GameObject graphic;

    public float chargeScale;
    public GameEvent onProjectileHitTerrain;

    private bool isColliding;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sp = GetComponentInChildren<SpriteRenderer>();
        soundMachine = GetComponent<AudioSource>();
        explosionmain = explosionEffectSpark.main;
        explosionsmoke = explosionEffectSmoke.main;
        explosionspark = explosionEffectSpark.main;

        setVisuals();
        //Debug.Log("Timer = " + chargeScale);
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
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
    }

    public void CalcRadius(GameEventData eventData)
    {
        if (eventData is PlayerEventData playerData && radiusModifier == 0)
        {
            radiusModifier = playerData.BlastValue;
            chargeScale = playerData.BlastValue;
            Debug.Log("blast value and charge scale is " + playerData.BlastValue);
             
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isColliding)
            return;
        isColliding = true;
        // Get all colliders within explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayers);
        
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D crb = collider.GetComponent<Rigidbody2D>();
            if (collider.CompareTag("Level"))
            {
                radius = Mathf.Clamp(radiusModifier, 0, radiusMax); // Blast amount adds to radius
                ProjectileEventData projEventData = new ProjectileEventData { Sender = this, HitPosition = transform, velocity = rb.velocity, radius = radius };
                onProjectileHitTerrain.Raise(projEventData);
            } else
            {
                if (crb != null)
                {
                    // Calculate direction and distance from explosion center to collider
                    Vector2 direction = crb.transform.position - transform.position;
                    float distance = direction.magnitude;

                    if (distance < 0.5f)
                        distance = 0f;
                    else
                        distance -= 0.4f;

                    // Apply impulse force to collider based on distance and explosion force
                    crb.AddForce(direction.normalized * calcExplosion(distance), ForceMode2D.Impulse);
                }
            }
        }

        explosionEffectMain.Play();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    //this will change the size of the explosion visual effect based on the charge power when shot
    //the value for change scale is 1.0 for low, 1.6 for medium, 2.2 for high
    private void setVisuals()
    {
        //low power shot
        if (chargeScale < 1.6)
        {
            explosionmain.startSize = 10;
            explosionsmoke.startSize = 4;
            explosionspark.startSize = 4;
        }
        //medium power shot
        else if (chargeScale < 2.2)
        {
            explosionmain.startSize = 11;
            explosionsmoke.startSize = 6;
            explosionspark.startSize = 6;
        }

        //high power shot
        else
        {
            explosionmain.startSize = 12;
            explosionsmoke.startSize = 8;
            explosionspark.startSize = 8;
        }
    }
}
