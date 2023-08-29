using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shooting params")]
    [SerializeField][Tooltip("every one increase in this value is one grid unit of vertical height to the shot")]
    [Range(1f, 20f)] 
    float shotForce = 4f;
    [SerializeField][Tooltip("every one increase in this value is one grid unit of horizontal movement")]
    [Range(1f, 20f)]
    float shotRecoil = 1f;
    public bool ShotCooldown
    {
        get { return shotCooldown; }
    }
    private bool shotCooldown = false;

    public float ReloadDelay
    {
        get { return reloadDelay; }
    }
    private float reloadDelay;

    [Header("Transforms")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform barrelPivot;

    [Header("Event Data")]
    public GameEvent onBlastEvent;
    float blastValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator StartFireDelay(float cd)
    {
        //Debug.Log("Start Cooldown");
        shotCooldown = true;
        yield return new WaitForSeconds(cd);
        shotCooldown = false;
        //Debug.Log("End Cooldown");
    }

    public IEnumerator StartReloadDelay()
    {

        for (reloadDelay = 0.05f; reloadDelay > 0; reloadDelay -= Time.deltaTime)
            yield return null;

    }

    public void Shoot(float angle, Vector3 spawnPos, float powerMod, GameObject projectile)
    {
        //soundMachine.PlayOneShot(gunAudio, volumeScale);
        Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
        GameObject shotProjectile = Instantiate(projectile);

        LayerMask mask = LayerMask.GetMask("Level");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPos - barrelPivot.position, 5, mask);

        if (hit.collider != null && hit.collider.CompareTag("Level"))
        {
            Debug.DrawRay(transform.position, spawnPos - barrelPivot.position, Color.red, hit.distance);
            shotProjectile.transform.position = hit.point;
        }
        else
        {
            Debug.DrawRay(transform.position, spawnPos - barrelPivot.position, Color.green, 5);
            shotProjectile.transform.position = spawnPos;
        }

        Vector2 forceDirection = new(Mathf.Cos(angle), Mathf.Sin(angle));
        
        shotProjectile.GetComponent<Projectile>().graphic.transform.rotation = muzzle.transform.rotation;
        Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
        shotProjectileRB.AddForce(forceDirection * calcForce() * powerMod, ForceMode2D.Impulse);
        
        tankRB.velocity = tankRB.velocity + forceDirection * calcRecoil() * powerMod;
        
        blastValue = powerMod;
        PlayerEventData eventData = new PlayerEventData { Sender = this, BlastValue = blastValue };
        onBlastEvent.Raise(eventData);
    }

    public float calcRecoil()
    {
        float adjustmentFactor = Mathf.Pow(shotRecoil, 0.5f);
        //increases force if slow-mo is enabled
        //if (SlowmoScript != null)
        //    if (SlowmoScript.slowed)
        //        adjustmentFactor *= 1.5f;
        return -adjustmentFactor * 2.83f;
    }

    public float calcForce()
    {
        float adjustmentFactor = Mathf.Pow(shotForce, 0.5f);
        return adjustmentFactor * 0.48f;
    }
}
