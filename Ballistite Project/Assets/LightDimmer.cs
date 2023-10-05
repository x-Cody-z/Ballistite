using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDimmer : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Light2D[] otherLights;
    [SerializeField] float dimAmount = 0.5f;
    [SerializeField] float dimSpeed = 0.5f;
    [SerializeField] float dimDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        StopCoroutine(BrightenLight());
        StartCoroutine(DimLight());
    }

    private void OnTriggerExit2D()
    {
        StopCoroutine(DimLight());
        StartCoroutine(BrightenLight());
    }

    IEnumerator DimLight()
    {
        yield return new WaitForSeconds(dimDelay);
        while (globalLight.intensity > dimAmount)
        {
            globalLight.intensity -= dimSpeed * Time.fixedDeltaTime;
            Mathf.Clamp(globalLight.intensity, 0, 1);
            foreach (Light2D light in otherLights)
            {
                light.intensity += dimSpeed * 10 * Time.fixedDeltaTime;
                Mathf.Clamp(light.intensity, 0, 10);
            }
            yield return null;
        }
    }

    IEnumerator BrightenLight()
    {
        while (globalLight.intensity < 1)
        {
            globalLight.intensity += dimSpeed * Time.fixedDeltaTime;
            Mathf.Clamp(globalLight.intensity, 0, 1);
            foreach (Light2D light in otherLights)
            {
                light.intensity -= dimSpeed * 10 * Time.fixedDeltaTime;
                Mathf.Clamp(light.intensity, 0, 10);
            }
            yield return null;
        }
    }
}
