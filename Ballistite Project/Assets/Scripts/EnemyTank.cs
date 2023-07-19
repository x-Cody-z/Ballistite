using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyTank : Tank
{
    // requirements: Cannot see player through wall, have a raycast with the player and a certain distance specified in order to 'detect' the player and change phases.
    //               Have an empty gameObject to represent what it is aiming at and have that slowly follow the player while it is in detect mode.
    //               When losing sight of player, enter 'Search' mode, putting itself into overwatch toward's the player's last known location seen by this enemy.
    //               When upside-down or HP depleted, destroyed state. Explosion and smoke trailing from wreakage.
    // extras:       Possibility of sharing information with each other? RadioSFX? A dedicated 'spotter' enemy? Mortar/Artillary a looming key feature of the city level in the background?

    //TODO:
    //      - Complete code for other states
    //      DONE - Add code to control fire rate
    //      DONE - add code to control shooting and target lead prediction
    //           - add code to control accuracy
    //      - add code to control tank movement
    //      - enemy shouldn't be able to see player through walls
    //      - add code to control enemy health/destruction
    //      DONE - update projectile to hit player if enemy has shot
    private enum State {Idle, Alert, Search, Destroyed}
    [SerializeField] private State m_State;

    [SerializeField] private GameObject player;
    [SerializeField] private float fireRate = 3f;
    [SerializeField] private float nextFire;
    [SerializeField] private float maxRange;

    private int layerMask;
    private List<Collider2D> ignoreColliders = new List<Collider2D>();

    private float CalculateProjectileSpeed()
    {
        return (calcForce() / projectile.GetComponent<Rigidbody2D>().mass);
    }

    private Vector3 CalculateLead(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        float distance = targetDirection.magnitude;
        float time = distance / projectileSpeed;

        return targetPosition + targetVelocity * time;
    }

    private void OnDrawGizmos()
    {
        // Draw a wire sphere around the explosion object to visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CalculateLead(GameObject.Find("Player").transform.position, GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity, CalculateProjectileSpeed()), 0.5f);
    }

    private RaycastHit2D DetectPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;

        float distance = direction.magnitude;

        Vector3 normalizedDirection = direction.normalized;

        float clampedDistance = Mathf.Min(distance, maxRange);
        Vector3 raycastDirection = normalizedDirection * clampedDistance;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, clampedDistance, LayerMask.GetMask("Player", "Level"));

        return hit;
    }

    void Start()
    {
        m_State = State.Idle;
        player = GameObject.Find("Player");

        // To avoid raycasting on the Cinemachine collider
        layerMask = ~LayerMask.GetMask("IgnoreRaycast");
    }

    void Update() //fixedupdate?
    {
        RaycastHit2D hit = DetectPlayer();

        switch (m_State)
        {
            case State.Idle:

                if (hit.transform != null && hit.transform.tag == "Player")
                {
                    Debug.Log("Player detected within range");
                    m_State = State.Alert;
                }

                break;

           case State.Alert:
                float angleInRadians = MoveBarrel(CalculateLead(player.transform.position, player.GetComponent<Rigidbody2D>().velocity, CalculateProjectileSpeed())) * Mathf.Deg2Rad;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, MoveBarrel(CalculateLead(player.transform.position, player.GetComponent<Rigidbody2D>().velocity, CalculateProjectileSpeed()))));
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Shoot(angleInRadians, muzzle.transform.position, 1f);
                    Console.WriteLine("Enemy fired");
                }
                if (hit.distance > maxRange)
                {
                    m_State = State.Search;
                }
                if (hit.transform != null && hit.transform.tag == "Level")
                {
                    Debug.Log("Player detected within range");
                    m_State = State.Search;
                }
                break;

            case State.Search:
                if (hit.transform != null && hit.transform.tag == "Player")
                {
                    Debug.Log("Player detected within range");
                    m_State = State.Alert;
                } 
                Debug.Log("Searching for player");
                break;

            case State.Destroyed:
                break;

            default:
                break;

        }
    }
}
