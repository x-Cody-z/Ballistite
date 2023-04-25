using System.Collections;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class BespokePlayerController : MonoBehaviour
    {
        public AudioClip gunAudio;
        public AudioClip reloadAudio;
        public AudioClip landingAudio;
        public Transform mouseIndicator;
        public Transform barrel;
        public Transform muzzle;
        public GameObject projectile;
        public float shotForce = 1f;
        public float shotRecoil = 2f;
        public float reloadTime = 1f;
        public float cooldownTime = 0.5f;
        public int shotNumber = 1;
        private int shotCount;

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
        }



        void Update()
        {
            if (grounded)
            {
                if (shotCount == 0 && !reloading)
                {
                    reloading = true;
                    StartCoroutine(GunReload());
                }
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
                if (Input.GetButtonDown("Fire1") && shotCount > 0 && !cooldown)
                {
                    soundMachine.PlayOneShot(gunAudio);
                    StartCoroutine(Cooldown());
                    shotCount--;
                    Rigidbody2D tankRB = GetComponent<Rigidbody2D>();
                    GameObject shotProjectile = Instantiate(projectile);
                    shotProjectile.transform.position = muzzle.transform.position;
                    shotProjectile.transform.rotation = muzzle.transform.rotation;
                    Rigidbody2D shotProjectileRB = shotProjectile.GetComponent<Rigidbody2D>();
                    float angleInRadians = barrelAngle * Mathf.Deg2Rad;
                    Vector2 forceDirection = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
                    shotProjectileRB.AddForce(forceDirection * shotForce, ForceMode2D.Impulse);
                    tankRB.AddForce(forceDirection * shotForce * -shotRecoil, ForceMode2D.Impulse);
                }
                if (Input.GetButtonDown("Jump"))
                {
                    transform.position = spawn;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Level")
            {
                soundMachine.PlayOneShot(landingAudio);
                grounded = true;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Level")
            {
                grounded = false;
            }
        }
        IEnumerator GunReload()
        {
            
            yield return new WaitForSeconds(reloadTime);
            shotCount++;
            
            if (shotCount<shotNumber && grounded)
            {
                soundMachine.PlayOneShot(reloadAudio);
                StartCoroutine(GunReload());
            }
            else if (shotCount<shotNumber)
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

        IEnumerator Cooldown()
        {
            //Debug.Log("Start Cooldown");
            cooldown = true;
            yield return new WaitForSeconds(0.5f);
            cooldown = false;
            //Debug.Log("End Cooldown");
        }
    }
}