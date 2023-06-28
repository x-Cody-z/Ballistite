using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform barrel;
    public Transform barrelPivot;
    public Transform muzzle;
    public GameObject projectile;

    public float Timer;
    public float LastBlastValue;
    public bool paused = true;

    [SerializeField] private float shotPower = 1f;
    [SerializeField] private float shotMod = 0.5f;

    [Tooltip("every one increase in this value is one grid unit of vertical height to the shot")]
    public float shotForce = 4f;

    [Tooltip("every one increase in this value is one grid unit of horizontal movement")]
    public float shotRecoil = 1f;

    [Tooltip("time in seconds for one shot to be reloaded")]
    public float reloadTime = 1f;

    [Tooltip("minimum time in seconds between each shot, think of it as fire rate")]
    public float cooldownTime = 0.5f;

    [Tooltip("the number of shots that can be loaded at once")]
    public int shotNumber = 1;

    //this is what actually keeps track of the number of shots, shotNumber is more like a static variable that shotCount gets set to
    private int shotCount;

    //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
    private float reloadTimeActive;
    private float reloadDelay;

    public bool controlEnabled = true;
    public bool grounded = true;
    public bool reloading = false;
    public bool cooldown = false;
    private bool shotCancel = false;

    public float MoveBarrel(Vector3 target)
    {
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(target);
        Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
        float barrelAngle = Mathf.Atan2(Worldpos2D.y - barrelPivot.position.y, Worldpos2D.x - barrelPivot.position.x) * Mathf.Rad2Deg;
        barrel.rotation = Quaternion.Euler(new Vector3(0, 0, barrelAngle));
        return barrelAngle;
    }

    private void Shoot(float angle, Vector3 spawnPos, float powerMod)
    {
        //functionality for reload
        StartCoroutine(ReloadDelay());


        //soundMachine.PlayOneShot(gunAudio, volumeScale);
        StartCoroutine(Cooldown(cooldownTime));
        shotCount--;
        Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
        GameObject shotProjectile = Instantiate(projectile);
        Timer = 0;
        paused = true;

        //shotProjectile.transform.position = spawnPos;
        shotProjectile.transform.position = this.transform.position;

        shotProjectile.GetComponent<Projectile>().graphic.transform.rotation = muzzle.transform.rotation;
        Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
        Vector2 forceDirection = new(Mathf.Cos(angle), Mathf.Sin(angle));
        shotProjectileRB.AddForce(forceDirection * calcForce() * powerMod, ForceMode2D.Impulse);
        //tankRB.AddForce(forceDirection * calcRecoil() * powerMod, ForceMode2D.Impulse);
        tankRB.velocity = tankRB.velocity + forceDirection * calcRecoil() * powerMod;
    }

    IEnumerator GunReloadV2()
    {
        for (reloadTimeActive = 1f; reloadTimeActive > 0; reloadTimeActive -= Time.deltaTime)
            yield return null;
        shotCount++;
        reloadTimeActive = reloadTime;


        if (shotCount < shotNumber && grounded)
        {
            //soundMachine.PlayOneShot(reloadAudio);
            StartCoroutine(GunReloadV2());
        }
        else if (shotCount < shotNumber)
        {
            yield return new WaitUntil(() => grounded);
            //soundMachine.PlayOneShot(reloadAudio, volumeScale);
            StartCoroutine(GunReloadV2());
        }
        if (shotCount == shotNumber)
        {
            //soundMachine.PlayOneShot(reloadAudio);
            reloading = false;
            //Debug.LogError("CLICK");
        }
    }

    IEnumerator ReloadDelay()
    {

        for (reloadDelay = 0.05f; reloadDelay > 0; reloadDelay -= Time.deltaTime)
            yield return null;

    }


    IEnumerator Cooldown(float cd)
    {
        //Debug.Log("Start Cooldown");
        cooldown = true;
        yield return new WaitForSeconds(cd);
        cooldown = false;
        //Debug.Log("End Cooldown");
    }

    private float calcRecoil()
    {
        float adjustmentFactor = Mathf.Pow(shotRecoil, 0.5f);
        return -adjustmentFactor * 2.83f;
    }

    private float calcForce()
    {
        float adjustmentFactor = Mathf.Pow(shotForce, 0.5f);
        return adjustmentFactor * 0.48f;
    }

    private void Update()
    {       
        float angleInRadians = MoveBarrel(Input.mousePosition) * Mathf.Deg2Rad;
    }
}
