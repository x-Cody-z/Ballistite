using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class BespokePlayerController : Tank
    {
        //UI object and script, probably easier than linking every text field in the UI
        //Being set automatically based on name in Awake(), could be set to public if this needs to be changed
        private GameObject chargeBar;
        private GameObject UIObject;
        private uiController UIScript;
        
        [SerializeField] CinemachineVirtualCamera vcamMouse;
        [SerializeField] CinemachineVirtualCamera vcamPlayer;
        [SerializeField] private float volumeScale = 1;
        
        public AudioClip gunAudio;
        public AudioClip reloadAudio;
        public AudioClip landingAudio;
        
        //bools used for toggling the charge indicators
        private bool charge1;
        private bool charge2;
        private bool charge3;

        public AudioSource soundMachine;

        public static Vector3 mousePos;
        private Vector3 Worldpos;
        private Vector3 spawn;
        private Quaternion spawnRot;

        private bool singlePower = false;
        private bool debug = false;
        [SerializeField]
        private float power;

        void Awake()
        {
            shotCount = shotNumber;
            spawn = transform.position;
            spawnRot = transform.rotation;
            soundMachine = GetComponent<AudioSource>();

            UIObject = GameObject.Find("UI");
            chargeBar = GameObject.Find("chargePanel");
            if (UIObject != null)
            {
                UIScript = UIObject.GetComponent<uiController>();
            }

            reloadTimeActive = reloadTime;
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
                float angleInRadians = MoveBarrel(Worldpos) * Mathf.Deg2Rad;
                barrel.rotation = Quaternion.Euler(new Vector3(0, 0, MoveBarrel(Worldpos)));


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
                                Shoot(angleInRadians, shotSpawnPos, power);
                                charge1 = false;
                                charge2 = false;
                                charge3 = false;
                                break;
                        }
                        LastBlastValue = Timer;
                    }
                    if (Input.GetButtonUp("Fire1") && shotCount > 0 && !cooldown && !shotCancel)
                    {
                        Shoot(angleInRadians, shotSpawnPos, power);
                        charge1 = false;
                        charge2 = false;
                        charge3 = false;   
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
                        Shoot(angleInRadians, shotSpawnPos, 1);
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