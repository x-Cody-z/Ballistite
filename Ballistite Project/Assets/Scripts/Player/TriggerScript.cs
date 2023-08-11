using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class TriggerScript : MonoBehaviour
{
    [Tooltip("The tag that the object colliding with the hitbox needs to have. e.g. Player, Bullet")]
    public string requiredTag = "Bullet";

    [Tooltip("Delay before any of the other actions happen. Use for stuff getting shot so the bullet actually explodes")]
    public float delay;
    private float delayActive;

    [Tooltip("If you want an object to be destroyed when the trigger activates, add it here")]
    public GameObject destroyObject;

    [Tooltip("If you want an object's hitbox to be destroyed, but not the object itself, then add it here. This should probably be combined with some kind of visual change")]
    public GameObject destroyObjectHitbox;

    [Tooltip("Add object here if you want it's colour to change, mostly a placeholder visual change for testing")]
    public GameObject changeObjectColour;
    public Color newColour;

    [Tooltip("This object will set an object to be inactive at first, but activate on trigger, use this on object you want to appear on trigger")]
    public GameObject spawnObject;

    [Tooltip("Animation controller of object you want to do something when it is triggered")]
    public Animation ObjectAnimator;

    [Tooltip("To call a function from a script on another object, needs the object with the script, the script's name and the function's name")]
    public GameObject ObjectWithScript;

    //public string ObjectNameWithScript;
    //private GameObject ObjectWithScript;
    public string scriptName;
    private MonoBehaviour script;
    public string functionName;

    private void Awake()
    {
        if (spawnObject != null)
        {
            spawnObject.SetActive(false);
        }
    }

    private IEnumerator startDelay()
    {
        
        // Wait for the specified amount of time
        for (delayActive = delay; delayActive > 0; delayActive -= Time.deltaTime)
            yield return null;

        checkTriggers();
        

    }

    private void checkTriggers()
    {
        if (destroyObjectHitbox != null)
        {
            Collider2D collider = destroyObjectHitbox.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        if (newColour != null && changeObjectColour != null)
        {
            SpriteRenderer render = changeObjectColour.GetComponent<SpriteRenderer>();
            if (render != null)
            {
                render.color = newColour;
            }
        }

        if (spawnObject != null)
        {
            spawnObject.SetActive(true);
        }

        /*
        if (ObjectNameWithScript != null)
        {
            Debug.Log("activateObject not null");
            Type scriptType = Type.GetType(scriptName);
            if (scriptType != null && typeof(MonoBehaviour).IsAssignableFrom(scriptType))
            {
                Debug.Log("script not null");
                ObjectWithScript = GameObject.Find(ObjectNameWithScript);
                if (ObjectWithScript != null)
                {
                    script = ObjectWithScript.GetComponent(scriptType) as MonoBehaviour;
                    if (script != null)
                    {
                        Debug.Log("message sent");
                        script.SendMessage(functionName);
                    }
                }
            }
        }
        */

        if (ObjectWithScript != null)
        {
            Debug.Log("activateObject not null");
            Type scriptType = Type.GetType(scriptName);
            if (scriptType != null && typeof(MonoBehaviour).IsAssignableFrom(scriptType))
            {
                Debug.Log("script not null");
                script = ObjectWithScript.GetComponent(scriptType) as MonoBehaviour;
                if (script != null)
                {
                    Debug.Log("message sent");
                    script.SendMessage(functionName);
                }
            }
        }


        if (destroyObject != null)
        {
            Destroy(destroyObject);
        }

        if (ObjectAnimator != null)
        {
            ObjectAnimator.Play();
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(requiredTag))
        {
            Debug.Log("Trigger entered by object with tag: " + requiredTag);
            
            if(delay > 0f)
            {
                StartCoroutine(startDelay());
            }
            else
            {
                checkTriggers();
            }
        }
    }

}
