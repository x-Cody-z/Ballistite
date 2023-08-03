using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(LineRenderer))]
public class Tank : MonoBehaviour
{

    LineRenderer trajectoryLine;
    public Transform barrel;
    public Transform barrelPivot;
    public Transform muzzle;
    public GameObject projectile;

    public float Timer;
    public float LastBlastValue;
    public bool paused = true;

    public float shotPower = 1f;
    public float shotMod = 0.5f;

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
    public int shotCount;

    //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
    public float reloadTimeActive;
    public float reloadDelay;

    public bool controlEnabled = true;
    public bool grounded = true;
    public bool reloading = false;
    public bool cooldown = false;
    public bool shotCancel = false;

    private void Start()
    {
        if (trajectoryLine == null)
        {
            trajectoryLine = GetComponent<LineRenderer>();
        }
    }

    public float MoveBarrel(Vector3 target)
    {
        Vector2 Worldpos2D = new Vector2(target.x, target.y);
        float barrelAngle = Mathf.Atan2(Worldpos2D.y - barrelPivot.position.y, Worldpos2D.x - barrelPivot.position.x) * Mathf.Rad2Deg;
        return barrelAngle;
    }

    public float CalculateProjectileSpeed(GameObject projectile)
    {
        return (calcForce() / projectile.GetComponent<Rigidbody2D>().mass);
    }

    /// <summary>
    /// Calculates the position the projectile should be fired at to hit a moving target
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="projectileSpeed"></param>
    /// <returns>Returns a vector3 representing the position that should be aimed at to hit target</returns>
    public Vector3 CalculateLead(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        float distance = targetDirection.magnitude;
        float timeToTarget = distance / projectileSpeed;

        return targetPosition + targetVelocity * timeToTarget;
    }

    public Vector3 CalculateProjectileVelocity(Vector3 velocity, float time)
    {
        velocity += Physics.gravity * time;
        return velocity;
    }

    private void UpdateLineRenderer(int count, int pointNum, Vector3 pos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointNum, pos);
    }

    public void CalculateTrajectory(float projectileSpeed, Vector3 aimDirection, int resolution, float sampleRate)
    {
        //calculate position of projectile over its flight time as a set of points
        Vector3 velocity = projectileSpeed / aimDirection.magnitude * aimDirection;
        Vector3 pos = muzzle.position;
        Vector3 nextPos;

        //raycast between each point to check for collisions and draw a line
        UpdateLineRenderer(resolution, 0, pos);
        for (int i = 1; i < resolution; i++)
        {
            velocity = CalculateProjectileVelocity(velocity, sampleRate);
            nextPos = pos + velocity * sampleRate;

            pos = nextPos;
            UpdateLineRenderer(resolution, i, nextPos);
        }

        //if collision is detected, draw a line to the point of collision and stop drawing the line
    }

    public void Shoot(float angle, Vector3 spawnPos, float powerMod)
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
        tankRB.velocity = tankRB.velocity + forceDirection * calcRecoil() * powerMod;
    }

    public IEnumerator GunReloadV2()
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
        }
    }

    IEnumerator ReloadDelay()
    {

        for (reloadDelay = 0.05f; reloadDelay > 0; reloadDelay -= Time.deltaTime)
            yield return null;

    }


    IEnumerator Cooldown(float cd)
    {
        cooldown = true;
        yield return new WaitForSeconds(cd);
        cooldown = false;
    }

    public float calcRecoil()
    {
        float adjustmentFactor = Mathf.Pow(shotRecoil, 0.5f);
        return -adjustmentFactor * 2.83f;
    }

    public float calcForce()
    {
        float adjustmentFactor = Mathf.Pow(shotForce, 0.5f);
        return adjustmentFactor * 0.48f;
    }
}
