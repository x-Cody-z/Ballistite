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

    [Header("Debug")]
    [SerializeField] bool isSkidding = false;
    Rigidbody2D playerRB;
    BespokePlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerController = GetComponent<BespokePlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        skid.gameObject.transform.position = playerRB.ClosestPoint(playerRB.position + new Vector2(14, 7));
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
        if (playerRB.velocity.magnitude > skidSpeed && playerController.isGrounded)
        {
            skid.gameObject.SetActive(true);
            var main = skid.main;
            main.startSpeed = playerRB.velocity.magnitude/skidVolumeModifier;
            main.maxParticles = (int)(playerRB.velocity.magnitude * skidAmountModifier);
            isSkidding = true;
        }
        else
        {
            skid.gameObject.SetActive(false);
            isSkidding = false;
        }
    }
}
