using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    [SerializeField]
    //private Sprite sprite;
    private GameObject crater;

    public void DestroyTiles(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            Transform projectilePos = projectileData.HitPosition; // gets position of event's projectile
            Vector2 velocity = projectileData.velocity;
            float radius = projectileData.radius/5;

            GameObject maskObject = Instantiate(crater, projectilePos.position, Quaternion.FromToRotation(Vector3.up, projectileData.hitNormal));
            maskObject.transform.localScale = new Vector2(radius, radius);

            //GameObject mask = new GameObject("craterMask");
            //mask.AddComponent<SpriteMask>().sprite = sprite;

            //mask.transform.position = projectilePos.position;
            //mask.transform.localScale = new Vector2(radius, radius);
            //mask.transform.rotation = Quaternion.FromToRotation(Vector3.up, projectileData.hitNormal);
            //mask.tag = "Crater";
        }
    }
}
