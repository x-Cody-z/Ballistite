using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    public ParticleSystem explosionEffectMain;
    public ParticleSystem explosionEffectSmoke;
    public ParticleSystem explosionEffectSpark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            //ParticleSystem p =  Instantiate(explosionEffectMain, collision.gameObject.transform);
            //p.Play();
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
