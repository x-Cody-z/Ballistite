using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySurface : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Rigidbody2D playerRb;
    private bool stuck = false;

    private enum modes // your custom enumeration
    {
        MaterialOnly,
        Gradual,
        GradualToFull,
        InstantFull,
    };

    [Tooltip("Material Only is least sticky, only uses material friction.\nGradual reduces speed and rotation over time, not fully freezing.\nGradual To Full is a combination of modes 2 and 4.\nInstant Full will completly freeze the player when they collide.\n")]
    [SerializeField] private modes mode = modes.Gradual;  // this public var should appear as a drop down

    [Tooltip("The time in seconds the Gradual To Full mode takes before completely freezing. (default 0.3)")]
    private float stickDelayTime = 0.3f;

    

    void Start()
    {
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    // when colliding with the player, stick them
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (mode != modes.MaterialOnly)
                stuck = true;
            if (mode == modes.GradualToFull)
                StartCoroutine(stickDelay(stickDelayTime));
            else if (mode == modes.InstantFull)
                playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //will unstick the player when a shot is fired
    public void Unstick(GameEventData eventData)
    {
        if (eventData is PlayerEventData blastEvent)
        {
            stuck = false;
            playerRb.constraints = RigidbodyConstraints2D.None;
        }
    }

    
    public IEnumerator stickDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(stuck)
        {
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    //when the playergets stuck, gradually reduce speed and rotation
    private void FixedUpdate()
    {
        if (stuck)
        {
            if (mode == modes.Gradual || mode == modes.GradualToFull)
            {
                if (playerRb.velocity.x != 0f || playerRb.velocity.y != 0f)
                    playerRb.velocity = playerRb.velocity * 0.75f;

                if (playerRb.angularVelocity != 0f)
                    playerRb.angularVelocity = playerRb.angularVelocity * 0.9f;
            }
        }
    }
}
