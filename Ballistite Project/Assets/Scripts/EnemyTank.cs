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

    private enum State {Idle, Alert, Search, Destroyed}
    [SerializeField] private State m_State;

    public GameObject aimPoint;

    [SerializeField] private GameObject player;
    [SerializeField] private float fireRate;
    [SerializeField] private float maxRange;

    //debug check
    [Space]
    [SerializeField] private Vector3 normalizedDirection; // just to see stats, later on delete this line and put the data type in the Update()
    [SerializeField] private float distance;              // ditto ^

    private int layerMask;
    private List<Collider2D> ignoreColliders = new List<Collider2D>();

    void Start()
    {
        m_State = State.Idle;
        player = GameObject.Find("Player");

        // To avoid raycasting on the Cinemachine collider
        layerMask = ~LayerMask.GetMask("IgnoreRaycast");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, normalizedDirection);
        Gizmos.color = Color.green;
    }

    void Update() //fixedupdate?
    {
        Vector3 direction = player.transform.position - transform.position;

        distance = direction.magnitude;

        normalizedDirection = direction.normalized;

        float clampedDistance = Mathf.Min(distance, maxRange);
        Vector3 raycastDirection = normalizedDirection * clampedDistance;

        switch (m_State)
        {
            case State.Idle:
                RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, clampedDistance, LayerMask.GetMask("Player", "Level"));

                if (hit.transform != null && hit.transform.gameObject == player)
                {
                    Debug.Log("Player detected within range");
                    m_State = State.Alert;
                }

                break;

           case State.Alert:
                float angleInRadians = MoveBarrel(player.transform.position) * Mathf.Deg2Rad;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, MoveBarrel(player.transform.position)));
                if (distance > maxRange)
                {
                    m_State = State.Idle;
                }
                break;

            case State.Search:
                break;

            case State.Destroyed:
                break;

            default:
                break;

        }
    }
}
