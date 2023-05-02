using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    [Tooltip("The tag that the object colliding with the hitbox needs to have. e.g. Player, Bullet")]
    public string requiredTag = "Bullet";

    [Tooltip("If you want an object to be destroyed when the trigger activates, add it here")]
    public GameObject destroyObject;

    [Tooltip("If you want an object's hitbox to be destroyed, but not the object itself, then add it here. This should probably be combined with some kind of visual change")]
    public GameObject destroyObjectHitbox;

    [Tooltip("Add object here if you want it's colour to change, mostly a placeholder visual change for testing")]
    public GameObject changeObjectColour;
    public Color newColour;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(requiredTag))
        {
            Debug.Log("Trigger entered by object with tag: " + requiredTag);

            if (destroyObject != null)
            {
                Destroy(destroyObject);
            }

            else if (destroyObjectHitbox != null)
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
        }
    }

}
