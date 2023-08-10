using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;

    public void DestroyTiles(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            Transform projectilePos = projectileData.HitPosition; // gets position of event's projectile
            Vector2 velocity = projectileData.velocity;
            float radius = projectileData.radius;

            GameObject mask = new GameObject("craterMask");
            mask.AddComponent<SpriteMask>().sprite = sprite;
            mask.transform.position = projectilePos.position;
            mask.transform.localScale = new Vector2(radius + Mathf.Abs(velocity.x /12), radius + Mathf.Abs(velocity.y /12));
            Debug.Log("Projectile Hit Terrain at " + projectilePos.position + " and radius x:" + radius + velocity.x +" and radius y:" + radius + velocity.y);
        }
    }
}
