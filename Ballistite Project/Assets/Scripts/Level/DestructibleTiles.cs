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
            float radius = projectileData.radius/5;

            GameObject mask = new GameObject("craterMask");
            mask.AddComponent<SpriteMask>().sprite = sprite;
            mask.transform.position = projectilePos.position;
            mask.transform.localScale = new Vector2(radius, radius);
        }
    }
}
