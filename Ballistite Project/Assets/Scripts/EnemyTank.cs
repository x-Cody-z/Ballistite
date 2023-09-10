using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(LeadPredictor))]
public class EnemyTank : MonoBehaviour
{
    // requirements: Cannot see player through wall, have a raycast with the player and a certain distance specified in order to 'detect' the player and change phases.
    //               Have an empty gameObject to represent what it is aiming at and have that slowly follow the player while it is in detect mode.
    //               When losing sight of player, enter 'Search' mode, putting itself into overwatch toward's the player's last known location seen by this enemy.
    //               When upside-down or HP depleted, destroyed state. Explosion and smoke trailing from wreakage.
    // extras:       Possibility of sharing information with each other? RadioSFX? A dedicated 'spotter' enemy? Mortar/Artillary a looming key feature of the city level in the background?

    //TODO:
    //      - Complete code for other states
    //      - Add code for search pattern
    //      DONE - Add code to control fire rate
    //      DONE - add code to control shooting and target lead prediction
    //      Optional? - add code to control accuracy
    //      Optional? - add code to control tank movement
    //      DONE s- enemy shouldn't be able to see player through walls
    //      DONE- add code to control enemy health/destruction
    //      DONE - update projectile to hit player if enemy has shot
    private enum State {Idle, Alert, Search, Destroyed}
    [SerializeField] private State m_State;

    [Header("Enemy Settings")]
    [SerializeField][Tooltip("The max range at which the enemy can detect the player")] private float maxRange;
    [SerializeField][Tooltip("power of enemy shot")]private float power;
    public event EventHandler OnEnemyDestroyed;

    private GameObject player;
    [Header("Game objects")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform barrel;

    [Header("Graphics")]
    [SerializeField] private SpriteRenderer[] tankGraphics;

    private Shooter shooter;
    private LeadPredictor leadPredictor;

    private int layerMask;
    private List<Collider2D> ignoreColliders = new List<Collider2D>();

    ProjectileData projectileData()
    {
        ProjectileData data = new ProjectileData();
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        data.direction = muzzle.transform.right;
        data.initialPosition = muzzle.position;
        data.initialSpeed = shooter.calcForce();
        data.mass = rb.mass;
        data.drag = rb.drag;

        return data;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leadPredictor.CalculateLead(GameObject.Find("Player").transform.position, GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity, leadPredictor.CalculateProjectileSpeed(projectileData(), power)), 0.5f);
    }


    private RaycastHit2D DetectPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;

        float distance = direction.magnitude;

        Vector2 normalizedDirection = direction.normalized;

        float clampedDistance = Mathf.Min(distance, maxRange);
        Vector2 raycastDirection = normalizedDirection * clampedDistance;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, clampedDistance, LayerMask.GetMask("Player", "Level"));
        Color color = Color.green;

        if (hit.transform != null && hit.transform.tag == "Player")
        {
            color = Color.red;
        }
        else
        {
            color = Color.green;
        }

        Debug.DrawRay(transform.position, raycastDirection, color);

        return hit;
    }

    void Start()
    {
        m_State = State.Idle;
        player = GameObject.Find("Player");

        if (shooter == null)
            shooter = GetComponent<Shooter>();

        if (leadPredictor == null)
            leadPredictor = GetComponent<LeadPredictor>();

        // To avoid raycasting on the Cinemachine collider
        layerMask = ~LayerMask.GetMask("IgnoreRaycast");
        OnEnemyDestroyed += EnemyTank_OnEnemyDestroyed;
    }

    private void EnemyTank_OnEnemyDestroyed(object sender, EventArgs e)
    {
        Debug.Log("Enemy destroyed");
        m_State = State.Destroyed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            OnEnemyDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }

    void Update() //fixedupdate?
    {
        RaycastHit2D hit = DetectPlayer();
        float leadAngle =
            shooter.GetBarrelAngle(leadPredictor.CalculateLead(
                    player.transform.position,
                    player.GetComponent<Rigidbody2D>().velocity,
                    leadPredictor.CalculateProjectileSpeed(projectileData(), power)));

        switch (m_State)
        {
            case State.Idle:

                if (hit.transform != null && hit.transform.tag == "Player")
                {
                    m_State = State.Alert;
                }

                break;

           case State.Alert:
                barrel.rotation = Quaternion.Euler(new Vector3(0,0,leadAngle * Mathf.Rad2Deg));

                if (!shooter.ShotCooldown)
                {
                    shooter.Shoot(leadAngle, muzzle.transform.position, power, projectile);
                    StartCoroutine(shooter.StartFireDelay());
                }
                if (!hit)
                {
                    m_State = State.Search;
                }
                if (hit.transform != null && hit.transform.tag == "Level")
                {
                    m_State = State.Search;
                }
                break;

            case State.Search:
                if (hit.transform != null && hit.transform.tag == "Player")
                {
                    m_State = State.Alert;
                } 
                break;

            case State.Destroyed:
                foreach (SpriteRenderer graphic in tankGraphics)
                {
                    graphic.color = Color.gray;
                }
                break;
        }
    }
}
