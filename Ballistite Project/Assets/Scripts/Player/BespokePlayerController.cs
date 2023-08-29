using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>]
    [RequireComponent(typeof(Shooter))]
    [RequireComponent(typeof(TrajectoryPredictor))]
    public class BespokePlayerController : MonoBehaviour
    {
        //UI object and script, probably easier than linking every text field in the UI
        //Being set automatically based on name in Awake(), could be set to public if this needs to be changed
        private GameObject chargeBar;
        private GameObject UIObject;
        private uiController UIScript;

        [Header("VCams")]
        [SerializeField] CinemachineVirtualCamera vcamMouse;
        [SerializeField] CinemachineVirtualCamera vcamPlayer;

        //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
        private float reloadTimeActive;

        [Header("Audio")]
        public AudioClip gunAudio;
        public AudioClip reloadAudio;
        public AudioClip landingAudio;
        public AudioSource soundMachine;
        [SerializeField] private float volumeScale = 1;

        [Header("Game objects")]
        public Transform barrel;
        public Transform barrelPivot;
        public Transform muzzle;
        public GameObject projectile;

        private TrajectoryPredictor trajectoryPredictor;

        //bools used for toggling the charge indicators
        private bool charge1;
        private bool charge2;
        private bool charge3;

        private Shooter shooter;

        [Header("Reload Params")]
        [SerializeField] private float shotPower = 1f;
        [SerializeField] private float shotMod = 0.5f;

        [Tooltip("time in seconds for one shot to be reloaded")]
        public float reloadTime = 1f;

        [Tooltip("minimum time in seconds between each shot, think of it as fire rate")]
        public float fireRate = 0.5f;

        [Tooltip("the number of shots that can be loaded at once")]
        public int shotNumber = 1;

        [Tooltip("the rate at which the charge bar fills (higher is faster)")]
        public float chargeRate = 1;

        //this is what actually keeps track of the number of shots, shotNumber is more like a static variable that shotCount gets set to
        private int shotCount;

        //timer stuff used for charging shot power
        public float chargeTimer;
        
        private float power;

        public bool chargePaused = true;
        private bool firstShot = true; //Boolean to disable start tutorial


        public bool controlEnabled = false;
        public bool notInsideCutscene = false;
        private bool grounded = true;
        private bool reloading = false;
        private bool shotCancel = false;

        [SerializeField] private GameObject SlowdownTrigger;
        private SlowdownTrigger SlowmoScript;

        private static Vector3 mousePos;

        private GameObject mouse;
        private Vector3 Worldpos;
        private Vector2 Worldpos2D;
        private float barrelAngle;
        private Vector3 spawn;
        private Quaternion spawnRot;

        private bool debug = false;


        [Header("Tutorial GameObjects")]
        [Tooltip("Basic shooting tutorial")]
        public GameObject shootingTutorial;

        void Awake()
        {
            power = 1;
            if (trajectoryPredictor == null)
                trajectoryPredictor = GetComponent<TrajectoryPredictor>();

            if (shooter == null)
                shooter = GetComponent<Shooter>();

            shotCount = shotNumber;
            spawn = transform.position;
            spawnRot = transform.rotation;
            soundMachine = GetComponent<AudioSource>();

            mouse = GameObject.Find("mousePos");
            UIObject = GameObject.Find("UI");
            chargeBar = GameObject.Find("chargePanel");
            if (UIObject != null)
            {
                UIScript = UIObject.GetComponent<uiController>();
            }

            reloadTimeActive = reloadTime;

            if (SlowdownTrigger != null)
            {
                SlowmoScript = SlowdownTrigger.GetComponent<SlowdownTrigger>();
            }
        }

        public void EnableControl(GameEventData eventData)
        {
            if (eventData.Sender is CutsceneController)
            {
                notInsideCutscene = true;
                controlEnabled = true;
            }
        }

        public void ToggleDebug()
        {
            debug = !debug; // toggle the debug variable
            Debug.Log("Debug mode is now " + debug);
        }

        ProjectileData projectileData() {
            ProjectileData data = new ProjectileData();
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            data.direction = muzzle.transform.right;
            data.initialPosition = muzzle.position;
            data.initialSpeed = shooter.calcForce() * power;
            data.mass = rb.mass;
            data.drag = 0;

            return data;
        }
        

        void Update()
        {
            trajectoryPredictor.CalculateTrajectory(projectileData());
            //take control away when paused & not in cutscene
            if (Time.timeScale == 0 || !notInsideCutscene)
            {
                controlEnabled = false;
            } else
            {
                controlEnabled = true;
            }

            //pan camera when holding right click
            if (vcamMouse != null && vcamPlayer != null)
            { 
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    vcamMouse.m_Priority = 1;
                    vcamPlayer.m_Priority = 0;
                }
                else
                {
                    vcamMouse.m_Priority = 0;
                    vcamPlayer.m_Priority = 1;
                }
            }

            //start timer when holding left click
            if (!chargePaused)
            {
                if (SlowmoScript != null)
                {
                    if (SlowmoScript.slowed)
                    {
                        chargeTimer += Time.deltaTime * chargeRate * (0.5f / SlowmoScript.slowdownAmount);
                    }
                    else
                        chargeTimer += Time.deltaTime * chargeRate;
                }

                else
                    chargeTimer += Time.deltaTime * chargeRate;
            }

            //checks for starting reload
            if (grounded)
            {
                if (shotCount < shotNumber && !reloading && shooter.ReloadDelay <=0)
                {
                    reloading = true;
                    StartCoroutine(GunReloadV2());
                }
            }
            if (controlEnabled)
            {

                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, GetBarrelAngle()* Mathf.Rad2Deg));

                Vector3 shotSpawnPos = muzzle.transform.position;
                //this is for charging power
                if (Input.GetButton("Fire1") && shotCount > 0 && !shooter.ShotCooldown && !shotCancel)
                {
                    chargePaused = false;
                    switch (chargeTimer)
                    {
                        case < 1:
                            if (!charge1)
                            {
                                power = shotPower;
                                charge1 = true;
                            }
                            break;
                        case >= 1 and < 2:
                            if (!charge2)
                            {
                                power = shotPower + shotMod;
                                charge2 = true;
                            }
                            break;
                        case >= 2 and < 3:
                            if (!charge3)
                            {
                                power = shotPower + 2*shotMod;
                                charge3 = true;
                            }
                            break;
                        case > 6:
                            shooter.Shoot(GetBarrelAngle(), shotSpawnPos, power, projectile);
                            StartCoroutine(shooter.StartReloadDelay());
                            StartCoroutine(shooter.StartFireDelay(fireRate));
                            FirstShot();
                            ResetCharge();
                            shotCount--;
                            power = 1;
                            break;
                    }

                }
                if (Input.GetButtonUp("Fire1") && shotCount > 0 && !shooter.ShotCooldown && !shotCancel)
                {
                    shooter.Shoot(GetBarrelAngle(), shotSpawnPos, power, projectile);
                    StartCoroutine(shooter.StartReloadDelay());
                    StartCoroutine(shooter.StartFireDelay(fireRate));
                    ResetCharge();
                    FirstShot();
                    shotCount--;
                    power = 1;
                }
                    

                //cancel shot
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //StartCoroutine(Cooldown(0.2f));
                    shotCancel = true;
                    ResetCharge();
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    shotCancel = false;
                }


                //key zero respawns player back at the beginning of level
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    transform.position = spawn;
                    transform.rotation = spawnRot;
                    GameObject.Find("win panel")?.SetActive(false);
                }
                
            }
            UpdateUIValues();
        }

        void ResetCharge()
        {
            chargeTimer = 0;
            chargePaused = true;
            charge1 = false;
            charge2 = false;
            charge3 = false;
        }

        float GetBarrelAngle()
        {
            mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
            mouse.transform.position = Worldpos2D;
            barrelAngle = Mathf.Atan2(Worldpos2D.y - barrelPivot.position.y, Worldpos2D.x - barrelPivot.position.x) * Mathf.Rad2Deg;
            barrel.rotation = Quaternion.Euler(new Vector3(0, 0, barrelAngle));
            return barrelAngle * Mathf.Deg2Rad;
        }
        void FirstShot()
        {
            if (firstShot)
            {
                firstShot = false;
                if (shootingTutorial != null)
                {
                    shootingTutorial.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("No basic shooting tutorial is loaded into the bespoke player controller, disregard if not on the first level.");
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Level"))
            {
                soundMachine.PlayOneShot(landingAudio, volumeScale);
                grounded = true;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Level"))
            {
                grounded = false;
            }
        }

        IEnumerator GunReloadV2()
        {
            for (reloadTimeActive = reloadTime; reloadTimeActive > 0; reloadTimeActive -= Time.deltaTime)
                yield return null;
            shotCount++;
            reloadTimeActive = reloadTime;


            if (shotCount < shotNumber && grounded)
            {
                soundMachine.PlayOneShot(reloadAudio);
                StartCoroutine(GunReloadV2());
            }
            else if (shotCount < shotNumber)
            {
                yield return new WaitUntil(() => grounded);
                soundMachine.PlayOneShot(reloadAudio, volumeScale);
                StartCoroutine(GunReloadV2());
            }
            if (shotCount == shotNumber)
            {
                soundMachine.PlayOneShot(reloadAudio);
                reloading = false;
                //Debug.LogError("CLICK");
            }
        }

        //calls functions from the UI's script to update values. Is called in update
        private void UpdateUIValues()
        {
            if (UIScript != null)
            {
                UIScript.updateAmmoValues(shotCount, reloadTimeActive);
                UIScript.updateVelocityValues(this.GetComponent<Rigidbody2D>().velocity.magnitude, this.GetComponent<Rigidbody2D>().velocity.x, this.GetComponent<Rigidbody2D>().velocity.y);
                UIScript.updateChargeValues(charge1, charge2, charge3);
            }
        }
    }
}