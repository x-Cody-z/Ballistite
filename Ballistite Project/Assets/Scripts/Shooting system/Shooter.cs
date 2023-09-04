using Platformer.Mechanics;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class contains logic relating to shooting and reloading
/// </summary>
public class Shooter : MonoBehaviour
{
    [Header("Shooting params")]
    [SerializeField][Tooltip("every one increase in this value is one grid unit of vertical height to the shot")]
    [Range(1f, 20f)] 
    float shotForce = 4f;
    [SerializeField][Tooltip("every one increase in this value is one grid unit of horizontal movement")]
    [Range(1f, 20f)]
    float shotRecoil = 1f;
    private bool shotCooldown = false;


    [Header("Reload params")]
    [SerializeField][Tooltip("time in seconds for one shot to be reloaded")]
    private float reloadTime = 1f;
    [SerializeField][Tooltip("the number of shots that can be loaded at once")]
    private int shotNumber = 1;
    [SerializeField][Tooltip("minimum time in seconds between each shot, think of it as fire rate")]
    private float fireRate = 0.5f;
    private float reloadDelay;
    //this is what actually keeps track of the number of shots, shotNumber is more like a static variable that shotCount gets set to
    private int shotCount;
    //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
    private float reloadTimer;
    private bool reloading = false;

    [Header("Transforms")]
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform barrelPivot;
    BespokePlayerController playerController;

    [Header("Audio")]
    public AudioClip gunAudio;
    public AudioClip reloadAudio;
    public AudioSource soundMachine;
    [SerializeField] private float volumeScale = 1;

    [Header("Event Data")]
    public GameEvent onBlastEvent;
    float blastValue;

    [Header("Scripts")]
    [SerializeField] private GameObject SlowdownTrigger;
    private SlowdownTrigger SlowmoScript;

    //These are just properties for the variables above
    public bool Reloading
    {
        get { return reloading; }
        set { reloading = value; }
    }
    public int ShotNumber
    {
        get { return shotNumber; }
        set { shotNumber = value; }
    }
    public int ShotCount
    {
        get { return shotCount; }
        set { shotCount = value; }
    }
    public bool ShotCooldown
    {
        get { return shotCooldown; }
    }
    public float ReloadDelay
    {
        get { return reloadDelay; }
    }

    public float ReloadTime
    {
        get { return reloadTime; }
    }

    public float ReloadTimer
    {
        get { return reloadTimer; }
        set { reloadTimer = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null)
        playerController = GetComponent<BespokePlayerController>();

        if (soundMachine == null)
        soundMachine = GetComponent<AudioSource>();

        if (SlowdownTrigger != null)
            SlowmoScript = SlowdownTrigger.GetComponent<SlowdownTrigger>();
    }

    /// <summary>
    /// Calculates the angle of the barrel pivot to hit a target
    /// </summary>
    /// <param name="target"></param>
    /// <returns>Returns an angle in radians for the barrel to pivot to</returns>
    public float GetBarrelAngle(Vector2 target)
    {
        float barrelAngle = Mathf.Atan2(target.y - barrelPivot.position.y, target.x - barrelPivot.position.x) * Mathf.Rad2Deg;
        return barrelAngle * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Starts the reload process if there are no shots left
    /// </summary>
    public void StartReload()
    {
        if (ShotCount < ShotNumber && !Reloading && ReloadDelay <= 0)
        {
            Reloading = true;
            StartCoroutine(GunReloadV2());
        }
    }

    public IEnumerator GunReloadV2()
    {
        for (reloadTimer = reloadTime; reloadTimer > 0; reloadTimer -= Time.deltaTime)
            yield return null;
        shotCount++;
        reloadTimer = reloadTime;


        if (shotCount < shotNumber && playerController.isGrounded)
        {
            soundMachine?.PlayOneShot(reloadAudio);
            StartCoroutine(GunReloadV2());
        }
        else if (shotCount < shotNumber)
        {
            yield return new WaitUntil(() => playerController.isGrounded);
            soundMachine?.PlayOneShot(reloadAudio, volumeScale);
            StartCoroutine(GunReloadV2());
        }
        if (shotCount == shotNumber)
        {
            soundMachine?.PlayOneShot(reloadAudio);
            reloading = false;
        }
    }

    /// <summary>
    /// Starts the timer to control fire rate
    /// </summary>
    public IEnumerator StartFireDelay()
    {
        shotCooldown = true;
        yield return new WaitForSeconds(fireRate);
        shotCooldown = false;
    }

    /// <summary>
    /// Starts a timer to delay the start of the reload process
    /// </summary>
    public IEnumerator StartReloadDelay()
    {

        for (reloadDelay = 0.05f; reloadDelay > 0; reloadDelay -= Time.deltaTime)
            yield return null;

    }

    /// <summary>
    /// Shoots a projectile
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="spawnPos"></param>
    /// <param name="powerMod"></param>
    /// <param name="projectile"></param>
    public void Shoot(float angle, Vector3 spawnPos, float powerMod, GameObject projectile)
    {
        soundMachine?.PlayOneShot(gunAudio, volumeScale);
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
        //increases force if slow - mo is enabled
        if (SlowmoScript != null)
                if (SlowmoScript.slowed)
                    adjustmentFactor *= 1.5f;
        return -adjustmentFactor * 2.83f;
    }

    public float calcForce()
    {
        float adjustmentFactor = Mathf.Pow(shotForce, 0.5f);
        return adjustmentFactor * 0.48f;
    }
}