using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    private Transform projectilePos;
    private float radiusModifier;
    public float radius;
    public float radiusMax;

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

            //TODO: check radius around projectile.
            radius = radius + radiusModifier; // Blast amount adds to radius
            if (radius >= radiusMax) radius = radiusMax;

            Debug.Log("Projectile Hit Terrain at " + projectilePos.position + " and radius will be " + radius);
        }
    }
}
