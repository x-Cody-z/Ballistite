using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        //part of new reload function, this is the value that changes as the reload time progresses, old reloadTime is used as a target value.
        private float reloadTimeActive;
        
        public AudioClip gunAudio;
        public AudioClip reloadAudio;
        public AudioClip landingAudio;
        public Transform mouseIndicator;
        public Transform barrel;
        public Transform muzzle;
        public GameObject projectile;
        //updated UI so these aren't needed for now
        /* 
        public TMP_Text velocity;
        public TMP_Text Hvelocity;
        public TMP_Text Vvelocity;
        */
        public GameObject indicator1;
        public GameObject indicator2;
        public GameObject indicator3;

        [SerializeField] private float shotPower = 1f;
        [SerializeField] private float shotMod = 0.5f;

        [Tooltip("every one increase in this value is one grid unit of vertical height to the shot")]
        public float shotForce = 4f;

        [Tooltip("every one increase in this value is one grid unit of horizontal movement")]
        public float shotRecoil = 1f;

        public float reloadTime = 1f;
        public float cooldownTime = 0.5f;
        public int shotNumber = 1;
        private int shotCount;
        private float Timer;
        public bool paused = true;

        public AudioSource soundMachine;

        public bool controlEnabled = true;
        public bool grounded = true;
        public bool reloading = false;
        public bool cooldown = false;

        public static Vector3 mousePos;

        private Vector3 Worldpos;
        private Vector2 Worldpos2D;
        private float barrelAngle;
        private Vector3 spawn;

        void Awake()
        {
            shotCount = shotNumber;
            spawn = transform.position;
            soundMachine = GetComponent<AudioSource>();

            UIObject = GameObject.Find("UI");
            chargeBar = GameObject.Find("chargePanel");
            if (UIObject != null)
            {
                UIScript = UIObject.GetComponent<uiController>();
            }
            reloadTimeActive = reloadTime;
        }

        private bool singlePower = false;
        private bool debug = false;
        [SerializeField] private float power;
        public void ToggleDebug()
        {
            debug = !debug; // toggle the debug variable
            Debug.Log("Debug mode is now " + debug);
        }

        void Update()
        {

            if (!paused)
            {
                Timer += Time.deltaTime;
            }


            //I've updated the method used to change UI values so this isn't needed, I'll if needed we can change back
            /*
            if (Input.GetButtonDown("Enable Debug Button 1"))
            {
                ToggleDebug();
            }
            if (debug)
            {
                velocity.gameObject.SetActive(true);
                Hvelocity.gameObject.SetActive(true);
                Vvelocity.gameObject.SetActive(true);
                velocity.text = "Velocity: " + this.GetComponent<Rigidbody2D>().velocity.magnitude.ToString();
                Hvelocity.text = "Horizontal Velocity: " + this.GetComponent<Rigidbody2D>().velocity.x.ToString();
                Vvelocity.text = "Vertical Velocity: " + this.GetComponent<Rigidbody2D>().velocity.y.ToString();
            }
            else
            {
                velocity.gameObject.SetActive(false);
                Hvelocity.gameObject.SetActive(false);
                Vvelocity.gameObject.SetActive(false);
            }
            
           */

            if (grounded)
            {
                if (shotCount < shotNumber && !reloading)
                {
                    reloading = true;
                    StartCoroutine(GunReloadV2());
                }
            }
            chargeBar.SetActive(!singlePower);
            if (Input.GetKeyDown("1"))
            {
                singlePower = !singlePower;
            }
            if (controlEnabled)
            {
                mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
                Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
                mouseIndicator.position = Worldpos2D;
                barrelAngle = Mathf.Atan2(Worldpos2D.y - transform.position.y, Worldpos2D.x - transform.position.x) * Mathf.Rad2Deg;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, barrelAngle));
                float angleInRadians = barrelAngle * Mathf.Deg2Rad;
                Vector3 shotSpawnPos = muzzle.transform.position;
                if (!singlePower)
                {
                    if (Input.GetButton("Fire1") && shotCount > 0 && !cooldown)
                    {
                        paused = false;
                        switch (Timer)
                        {
                            case < 1 :
                                if (!indicator1.activeInHierarchy)
                                {
                                    power = shotPower;
                                    indicator1.SetActive(true);
                                }
                                break;
                            case >= 1 and < 2:
                                if (!indicator2.activeInHierarchy)
                                {
                                    power = shotPower + shotMod;
                                    indicator2.SetActive(true);
                                }
                                break;
                            case >= 2 and < 3:
                                if (!indicator3.activeInHierarchy)
                                {
                                    power = shotPower + 2*shotMod;
                                    indicator3.SetActive(true);
                                }
                                break;
                            case > 6:
                                shoot(angleInRadians, shotSpawnPos, power);
                                break;
                        }

                    }
                    if (Input.GetButtonUp("Fire1") && shotCount > 0 && !cooldown)
                    {
                        shoot(angleInRadians, shotSpawnPos, power);
                    }
                } else
                {
                    if (Input.GetButtonDown("Fire1") && shotCount > 0 && !cooldown)
                    {
                        shoot(angleInRadians, shotSpawnPos, 1);
                    }
                }

                if (Input.GetButtonDown("Jump"))
                {
                    transform.position = spawn;
                }
                /*
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    shoot(0f, this.transform.position);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    shoot(1.57079f, this.transform.position);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    shoot(3.14159f, this.transform.position);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    shoot(4.71238f, this.transform.position);
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    shoot(0.785398f, this.transform.position);
                }
                */
            }
            UpdateUIValues();
        }

        private void shoot(float angle, Vector3 spawnPos, float powerMod)
        {
            soundMachine.PlayOneShot(gunAudio);
            StartCoroutine(Cooldown());
            shotCount--;
            Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
            GameObject shotProjectile = Instantiate(projectile);
            Timer = 0;
            paused = true;
            indicator1.SetActive(false);
            indicator2.SetActive(false);
            indicator3.SetActive(false);

            //shotProjectile.transform.position = spawnPos;
            shotProjectile.transform.position = this.transform.position;

            shotProjectile.GetComponent<Projectile>().graphic.transform.rotation = muzzle.transform.rotation;
            Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = new(Mathf.Cos(angle), Mathf.Sin(angle));
            shotProjectileRB.AddForce(forceDirection * calcForce() * powerMod, ForceMode2D.Impulse);
            //tankRB.AddForce(forceDirection * calcRecoil() * powerMod, ForceMode2D.Impulse);
            tankRB.velocity = tankRB.velocity + forceDirection * calcRecoil() * powerMod;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Level"))
            {
                soundMachine.PlayOneShot(landingAudio);
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
        //The reload function has been updated so that reload can be displayed in the UI
        /*
        IEnumerator GunReload()
        {

            yield return new WaitForSeconds(reloadTime);
            shotCount++;

            if (shotCount < shotNumber && grounded)
            {
                soundMachine.PlayOneShot(reloadAudio);
                StartCoroutine(GunReload());
            }
            else if (shotCount < shotNumber)
            {
                yield return new WaitUntil(() => grounded);
                soundMachine.PlayOneShot(reloadAudio);
                StartCoroutine(GunReload());
            }
            if (shotCount == shotNumber)
            {
                soundMachine.PlayOneShot(reloadAudio);
                reloading = false;
                //Debug.LogError("CLICK");
            }
        }
        */

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
                soundMachine.PlayOneShot(reloadAudio);
                StartCoroutine(GunReloadV2());
            }
            if (shotCount == shotNumber)
            {
                soundMachine.PlayOneShot(reloadAudio);
                reloading = false;
                //Debug.LogError("CLICK");
            }
        }

        IEnumerator Cooldown()
        {
            //Debug.Log("Start Cooldown");
            cooldown = true;
            yield return new WaitForSeconds(0.5f);
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
            }
        }
    }
}