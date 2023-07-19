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
    /// </summary>
    public class BespokePlayerController : MonoBehaviour
    {
        //UI object and script, probably easier than linking every text field in the UI
        //Being set automatically based on name in Awake(), could be set to public if this needs to be changed
        private GameObject chargeBar;
        private GameObject UIObject;
        private uiController UIScript;
        
        [SerializeField] CinemachineVirtualCamera vcamMouse;
        [SerializeField] CinemachineVirtualCamera vcamPlayer;
       
        //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
        private float reloadTimeActive;
        private float reloadDelay;
        [SerializeField] private float volumeScale = 1;
        
        public AudioClip gunAudio;
        public AudioClip reloadAudio;
        public AudioClip landingAudio;
        public Transform barrel;
        public Transform barrelPivot;
        public Transform muzzle;
        public GameObject projectile;
        public GameObject projectile_ricochet;
        
        //bools used for toggling the charge indicators
        private bool charge1;
        private bool charge2;
        private bool charge3;



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

        //power-up shot counts
        private int ricochetAmmo;

        //timer stuff used for charging shot power
        public float Timer;
        public float LastBlastValue;
        public bool paused = true;

        public AudioSource soundMachine;

        public bool controlEnabled = true;
        public bool grounded = true;
        public bool reloading = false;
        public bool cooldown = false;
        private bool shotCancel = false;

        [SerializeField] private GameObject SlowdownTrigger;
        private SlowdownTrigger SlowmoScript;

        public static Vector3 mousePos;

        private GameObject mouse;
        private Vector3 Worldpos;
        private Vector2 Worldpos2D;
        private float barrelAngle;
        private Vector3 spawn;
        private Quaternion spawnRot;

        private bool singlePower = false;
        private bool debug = false;

        //should this be serialized?
        [SerializeField] private float power;

        void Awake()
        {
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

        
        public void ToggleDebug()
        {
            debug = !debug; // toggle the debug variable
            Debug.Log("Debug mode is now " + debug);
        }
        

        void Update()
        {
            //take control away when paused
            if (Time.timeScale == 0)
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
            if (!paused)
            {
                if (SlowmoScript != null)
                {
                    if (SlowmoScript.slowed)
                    {
                        Timer += Time.deltaTime * (0.5f / SlowmoScript.slowdownAmount);
                    }
                    else
                        Timer += Time.deltaTime;
                }

                else
                    Timer += Time.deltaTime;
            }

            //checks for starting reload
            if (grounded)
            {
                if (shotCount < shotNumber && !reloading && reloadDelay <=0)
                {
                    reloading = true;
                    StartCoroutine(GunReloadV2());
                }
            }
            chargeBar.SetActive(!singlePower);
            if (controlEnabled)
            {
                mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
                Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
                mouse.transform.position = Worldpos2D;
                barrelAngle = Mathf.Atan2(Worldpos2D.y - barrelPivot.position.y, Worldpos2D.x - barrelPivot.position.x) * Mathf.Rad2Deg;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, barrelAngle));
                float angleInRadians = barrelAngle * Mathf.Deg2Rad;

                
                Vector3 shotSpawnPos = muzzle.transform.position;
                if (!singlePower)
                {
                    //this is for charging power
                    if (Input.GetButton("Fire1") && shotCount > 0 && !cooldown && !shotCancel)
                    {
                        paused = false;
                        switch (Timer)
                        {
                            case < 1 :
                                //eventually this code needs to be refactored to not use the indicators if we arent gonna have them in games
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
                                shoot(angleInRadians, shotSpawnPos, power);
                                break;
                        }
                        LastBlastValue = Timer;
                    }
                    if (Input.GetButtonUp("Fire1") && shotCount > 0 && !cooldown && !shotCancel)
                    {
                        shoot(angleInRadians, shotSpawnPos, power);
                    }
                    

                    //cancel shot
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Timer = 0;
                        paused = true;
                        //StartCoroutine(Cooldown(0.2f));
                        shotCancel = true;

                        charge1 = false;
                        charge2 = false;
                        charge3 = false;
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        shotCancel = false;
                    }

                } else
                {
                    //This is for single shot
                    if (Input.GetButtonDown("Fire1") && shotCount > 0 && !cooldown)
                    {
                        shoot(angleInRadians, shotSpawnPos, 1);
                    }
                }

                //key zero respawns player back at the beginning of level
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    transform.position = spawn;
                    transform.rotation = spawnRot;
                    GameObject.Find("win panel").SetActive(false);
                }
                
            }
            UpdateUIValues();
        }

        private void shoot(float angle, Vector3 spawnPos, float powerMod)
        {
            //functionality for reload
            StartCoroutine(ReloadDelay());


            soundMachine.PlayOneShot(gunAudio, volumeScale);
            StartCoroutine(Cooldown(cooldownTime));
            shotCount--;
            Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
            GameObject shotProjectile = Instantiate(projectileManager());
            Timer = 0;
            paused = true;
            charge1 = false;
            charge2 = false;
            charge3 = false;

            //shotProjectile.transform.position = spawnPos;
            shotProjectile.transform.position = this.transform.position;

            //shotProjectile.GetComponent<Projectile>().graphic.transform.rotation = muzzle.transform.rotation;
            Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = new(Mathf.Cos(angle), Mathf.Sin(angle));
            shotProjectileRB.AddForce(forceDirection * calcForce() * powerMod, ForceMode2D.Impulse);
            //tankRB.AddForce(forceDirection * calcRecoil() * powerMod, ForceMode2D.Impulse);
            tankRB.velocity = tankRB.velocity + forceDirection * calcRecoil() * powerMod;
        }

        private float calcRecoil()
        {
            float adjustmentFactor = Mathf.Pow(shotRecoil, 0.5f);
            //increases force if slow-mo is enabled
            if (SlowmoScript != null)
                if (SlowmoScript.slowed)
                    adjustmentFactor *= 1.5f;
            return -adjustmentFactor * 2.83f;
        }

        private float calcForce()
        {
            float adjustmentFactor = Mathf.Pow(shotForce, 0.5f);
            return adjustmentFactor * 0.48f;
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
            for (reloadTimeActive = 1f; reloadTimeActive > 0; reloadTimeActive -= Time.deltaTime)
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

        //used by the power-up controller to increase and change ammo types
        public void IncreaseAmmo(int amount, string type)
        {
            if (type == "ricochet")
            {
                ricochetAmmo += amount;
            }

            else if (type == "normal" || type == "default" || type == "")
                shotCount += amount;

        }

        //used by shoot function to select which projectile type to spawn
        private GameObject projectileManager()
        {
            //if (selectedAmmoType == ricochet)         //if we want the player to select the ammo type with scroll wheel
            if (ricochetAmmo > 0)                       //if we want the player to immediately use the ammo they picked up
            {
                ricochetAmmo -= 1;
                return projectile_ricochet;
            }
            return projectile;
        }
    }
}