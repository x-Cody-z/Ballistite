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
            
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DimLight());
            Debug.Log("collided with player");
        }
    }

    private void OnTriggerExit2D()
    {
        StartCoroutine(BrightenLight());
    }

    IEnumerator DimLight()
    {
        yield return new WaitForSeconds(dimDelay);
        while (globalLight.intensity > dimAmount)
        {
            globalLight.intensity -= dimSpeed * Time.deltaTime;
            foreach (Light2D light in otherLights)
            {
                light.intensity += dimSpeed * 10 * Time.deltaTime;
            }
            yield return null;
        }
    }

    IEnumerator BrightenLight()
    {
        while (globalLight.intensity < 1)
        {
            globalLight.intensity += dimSpeed * Time.deltaTime;
            foreach (Light2D light in otherLights)
            {
                light.intensity -= dimSpeed * 10 * Time.deltaTime;
            }
            yield return null;
        }
    }
}
