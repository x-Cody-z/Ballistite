using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    private Transform projectilePos;
    private Vector2 velocity;
    private float radiusModifier;
    public float radius;
    public float radiusMax;
    [SerializeField]
    private Sprite sprite;

    public void CalcRadius(GameEventData eventData)
    {
        if (eventData is PlayerEventData playerData)
        {
            radiusModifier = playerData.BlastValue;
        }
    }

    public void DestroyTiles(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            projectilePos = projectileData.HitPosition; // gets position of event's projectile
            velocity = projectileData.velocity;

            //TODO: check radius around projectile.
            radius = radiusModifier; // Blast amount adds to radius
            if (radius >= radiusMax) radius = radiusMax;

            GameObject mask = new GameObject("craterMask");
            mask.AddComponent<SpriteMask>().sprite = sprite;
            mask.transform.position = projectilePos.position;
            mask.transform.localScale = new Vector2(radius + Mathf.Abs(velocity.x /12), radius + Mathf.Abs(velocity.y /12));
            Debug.Log("Projectile Hit Terrain at " + projectilePos.position + " and radius x:" + radius + velocity.x +" and radius y:" + radius + velocity.y);
        }
    }
}
