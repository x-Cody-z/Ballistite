using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    private Transform projectilePos;
    public void DestroyTiles(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            projectilePos = projectileData.HitPosition; // gets position of event's projectile

            //TODO: check radius around projectile

            Debug.Log("Projectile Hit Terrain at " + projectilePos.position);
        }
    }
}
