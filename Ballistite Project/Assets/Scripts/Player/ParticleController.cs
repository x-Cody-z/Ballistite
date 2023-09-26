using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] ParticleSystem skid;

    [Header("Parameters")]
    [SerializeField] float skidSpeed = 1f;
    [SerializeField] float skidVolumeModifier = 5f;
    [SerializeField] float skidAmountModifier = 10f;
    [SerializeField] float emitRate = 0.1f;

    [Header("Debug")]
    [SerializeField] bool particlesPlaying = false;
    [SerializeField] bool particlesPaused = false;
    [SerializeField] bool particlesStopped = false;

    float emitTimer = 0f;
    Rigidbody2D playerRB;
    BespokePlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerController = GetComponent<BespokePlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRB.position, Vector2.down, 2);
        particlesPlaying = skid.isPlaying;
        particlesPaused = skid.isPaused;
        particlesStopped = skid.isStopped;
        emitTimer += Time.deltaTime;
        if (playerRB.velocity.normalized.x > 0)
        {
            skid.gameObject.transform.rotation = Quaternion.Euler(-150f, 90, 90);
            skid.gameObject.transform.position = playerRB.ClosestPoint(playerRB.position + new Vector2(14, 7));
        }
        else
        {
            skid.gameObject.transform.rotation = Quaternion.Euler(-24.46f, 90, 90);
            skid.gameObject.transform.position = playerRB.ClosestPoint(playerRB.position + new Vector2(-14, 7));
        }
        if (playerRB.velocity.magnitude > skidSpeed && playerController.isGrounded && emitTimer > emitRate)
        {
            var main = skid.main;
            skid.Emit(1);
            main.startSpeed = playerRB.velocity.magnitude/skidVolumeModifier;
            main.maxParticles = (int)(playerRB.velocity.magnitude * skidAmountModifier);
            emitTimer = 0f;
        }
    }
}
