using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    // requirements: Cannot see player through wall, have a raycast with the player and a certain distance specified in order to 'detect' the player and change phases.
    //               Have an empty gameObject to represent what it is aiming at and have that slowly follow the player while it is in detect mode.
    //               When losing sight of player, enter 'Search' mode, putting itself into overwatch toward's the player's last known location seen by this enemy.
    //               When upside-down or HP depleted, destroyed state. Explosion and smoke trailing from wreakage.
    // extras:       Possibility of sharing information with each other? RadioSFX? A dedicated 'spotter' enemy? Mortar/Artillary a looming key feature of the city level in the background?

    private enum State {Idle, Alert, Search, Destroyed}
    [SerializeField] private State m_State;

    public GameObject projectile;
    public GameObject aimPoint;

    [SerializeField] private GameObject player;
    [SerializeField] private float fireRate;
    [SerializeField] private float maxRange;

    void Start()
    {
        m_State = State.Idle;
        player = GameObject.Find("Player");
        Debug.DrawRay(transform.position, Vector3.forward, Color.green, 100f, false);
    }

    void Update() //fixedupdate?
    {
        switch (m_State)
        {
            case State.Idle:
                // waits in place or patrolling, once a raycast on the player is successful 
                RaycastHit hit;

                if (Vector2.Distance(transform.position, player.transform.position) < maxRange)
                {
                    if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, maxRange))
                    {
                        if (hit.transform == player)
                        {
                            Debug.Log("Your tank has been detected!");
                        }
                    }
                }
                
                break;

           case State.Alert:
                // checks raycast to see if player is still in view, continue attack
                break;

            case State.Search:
                // raycast is broken, keeps watch or atively searches, after an amount of time has elapsed, return to idle state
                break;

            case State.Destroyed:
                // tank HP is 0 or flipped and unable to roll back over, can sprite to destroyed and smoke VFX begins
                break;

            default:
                break;

        }
    }
}
