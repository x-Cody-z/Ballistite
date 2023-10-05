using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// </summary>
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
        [SerializeField] CinemachineVirtualCamera shakeCam;

        [Header("Audio")]
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
        [HideInInspector] public bool charge1;
        [HideInInspector] public bool charge2;
        [HideInInspector] public bool charge3;

        public Shooter shooter;

        [Header("Shot Params")]
        [SerializeField][Tooltip("base power of the shot")] 
        private float shotPower = 1f;
        [SerializeField][Tooltip("value that modifies power per charge")] 
        private float shotMod = 0.5f;

        [Header("Charge Params")]
        [SerializeField][Tooltip("the rate at which the charge bar fills (higher is faster)")]
        private float chargeRate = 1;

        //timer stuff used for charging shot power
        private float chargeTimer = 0;
        private float power;
        private bool interior = false;
        public bool chargePaused = true;
        private bool firstShot = true; //Boolean to disable start tutorial

        [Header("control enablers")]
        [SerializeField]
        public bool controlEnabled = false;
        [SerializeField]
        private bool notInsideCutscene = false;
        [HideInInspector] public bool grounded = true;
        [SerializeField] private int groundedCounter = 0;
        //used to stop charging a second shot after canceling before re-pressing m1
        [HideInInspector] public bool shotCancel = false;

        [Header("sloMo")]
        [SerializeField] private GameObject SlowdownTrigger;
        private SlowdownTrigger SlowmoScript;

        private Vector3 spawn;
        private Quaternion spawnRot;
        private bool debug = false;


        [Header("Tutorial GameObjects")]
        [Tooltip("Basic shooting tutorial")]
        public GameObject shootingTutorial;

        public bool isGrounded
        {
            get { return grounded; }
        }

        public float ChargeTimer
        {
            get { return chargeTimer; }
            set { chargeTimer = value; }
        }

        void Start()
        {
            if (trajectoryPredictor == null)
                trajectoryPredictor = GetComponent<TrajectoryPredictor>();

            if (shooter == null)
                shooter = GetComponent<Shooter>();

            if (SlowdownTrigger != null)
                SlowmoScript = SlowdownTrigger.GetComponent<SlowdownTrigger>();
            
            if (UIObject != null)
                UIScript = UIObject.GetComponent<uiController>();

            if (soundMachine == null)
                soundMachine = GetComponent<AudioSource>();

            if (UIObject == null)
                UIObject = GameObject.Find("UI");

            if (chargeBar == null)
                chargeBar = GameObject.Find("chargePanel");
            
            power = 1;
            shooter.ShotCount = shooter.ShotNumber;
            shooter.ReloadTimer = shooter.ReloadTime;
            spawn = transform.position;
            spawnRot = transform.rotation;

            if (notInsideCutscene)
            {
                controlEnabled = true;
                GameObject.Find("barrelGraphic")?.SetActive(true);
                GameObject.Find("barrelGraphicFake")?.SetActive(false);
            }
            else
            {
                controlEnabled = false;
                GameObject.Find("barrelGraphic")?.SetActive(false);
                GameObject.Find("barrelGraphicFake")?.SetActive(true);
            }
        }


        void Update()
        {
            trajectoryPredictor.CalculateTrajectory(projectileData());

            if (groundedCounter > 0)
                grounded = true;
            else
                grounded = false;

            //take control away when paused & not in cutscene
            if (Time.timeScale == 0 || !notInsideCutscene)
            {
                controlEnabled = false;
            } else
            {
                controlEnabled = true;
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
                shooter.StartReload();
            }
            if (controlEnabled)
            {

                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, shooter.GetBarrelAngle(GetMousePos2d()) * Mathf.Rad2Deg));
                Vector3 shotSpawnPos = muzzle.transform.position;

                //this is for charging power
                if (Input.GetButton("Fire1") && shooter.ShotCount > 0 && !shooter.ShotCooldown && !shotCancel)
                {
                    ChargeShot(shotSpawnPos);
                }
                if (Input.GetButtonUp("Fire1") && shooter.ShotCount > 0 && !shooter.ShotCooldown && !shotCancel)
                {
                    shooter.Shoot(shooter.GetBarrelAngle(GetMousePos2d()), shotSpawnPos, power, projectile);
                    StartCoroutine(ShotShake());
                    StartCoroutine(shooter.StartReloadDelay());
                    StartCoroutine(shooter.StartFireDelay());
                    ResetCharge();
                    FirstShot();
                    shooter.ShotCount--;
                    power = 1;
                }
                    

                //cancel shot
                if (Input.GetKeyDown(KeyCode.Space))
                {
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
        public void EnableControl(GameEventData eventData)
        {
            if (eventData.Sender is CutsceneController)
            {
                notInsideCutscene = true;
                controlEnabled = true;
            }
        }

        public void EnableCutscene(bool cutsceneState)
        {
            notInsideCutscene = !cutsceneState;
        }


        public void EnableControl(bool controlState)
        {
            controlEnabled = controlState;
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
            data.drag = rb.drag;

            return data;
        }

        void ChargeShot(Vector3 shotSpawn)
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
                        power = shotPower + 2 * shotMod;
                        charge3 = true;
                    }
                    break;
                case > 6:
                    shooter.Shoot(shooter.GetBarrelAngle(GetMousePos2d()), shotSpawn, power, projectile);
                    StartCoroutine(ShotShake());
                    StartCoroutine(shooter.StartReloadDelay());
                    StartCoroutine(shooter.StartFireDelay());
                    FirstShot();
                    ResetCharge();
                    shooter.ShotCount--;
                    power = 1;
                    break;
            }
        }

        public void ResetCharge()
        {
            chargeTimer = 0;
            chargePaused = true;
            charge1 = false;
            charge2 = false;
            charge3 = false;
        }

        Vector2 GetMousePos2d()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
            return Worldpos2D;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Interior"))
            {
                interior = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Interior"))
            {
                interior = false;
            }
        }

        IEnumerator ShotShake()
        {
            if (interior)
            {
                shakeCam.Priority = 12;
                yield return new WaitForSeconds(0.5f);
                shakeCam.Priority = 10;
            }
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
                groundedCounter += 1;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Level"))
            {
                groundedCounter -= 1;
            }
        }

        //calls functions from the UI's script to update values. Is called in update
        private void UpdateUIValues()
        {
            if (UIScript != null)
            {
                UIScript.updateAmmoValues(shooter.ShotCount, shooter.ReloadTimer);
                UIScript.updateVelocityValues(this.GetComponent<Rigidbody2D>().velocity.magnitude, this.GetComponent<Rigidbody2D>().velocity.x, this.GetComponent<Rigidbody2D>().velocity.y);
                UIScript.updateChargeValues(charge1, charge2, charge3);
            }
        }

        public string getPlayerValues()
        {
            string result = "";

            result += "shotPower: " + shotPower.ToString() + "\n";
            result += "shotMod: " + shotMod.ToString() + "\n";
            result += "chargeRate: " + chargeRate.ToString() + "\n";
            result += "chargeTimer: " + chargeTimer.ToString() + "\n";
            result += "power: " + power.ToString() + "\n";
            result += "chargePaused: " + chargePaused.ToString() + "\n";
            result += "firstShot: " + firstShot.ToString() + "\n";
            result += "controlEnabled: " + controlEnabled.ToString() + "\n";
            result += "notInsideCutscene: " + notInsideCutscene.ToString() + "\n";
            result += "grounded: " + grounded.ToString() + "\n";
            result += "shotCancel: " + shotCancel.ToString() + "\n";

            return result;
        }
    }
}