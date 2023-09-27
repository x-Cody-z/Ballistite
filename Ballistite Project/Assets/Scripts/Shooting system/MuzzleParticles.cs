using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleParticles : MonoBehaviour
{
    ParticleSystem smokeParticles;
    [SerializeField] Light2D muzzleLight;
    [SerializeField] ParticleSystem muzzleTrail;
    Rigidbody2D rb;
    float defaultIntensity;
    float intensityTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        smokeParticles = GetComponent<ParticleSystem>();
        rb = GetComponentInParent<Rigidbody2D>();
        defaultIntensity = muzzleLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        var emission = smokeParticles.emission;
        emission.rateOverTime = rb.velocity.magnitude * 20;
        if (muzzleLight.gameObject.activeSelf && intensityTimer < 0.2f)
        {
            intensityTimer += Time.deltaTime;
            muzzleLight.intensity = Mathf.Lerp(defaultIntensity, 0, intensityTimer);
        }
        else
        {
            muzzleLight.gameObject.SetActive(false);
            intensityTimer = 0f;
        }
    }

    public void EmitParticlesBurst()
    {
        smokeParticles.Emit(20);
        muzzleTrail.Play();
        muzzleLight.intensity = defaultIntensity;
        muzzleLight.gameObject.SetActive(true);
    }
}
