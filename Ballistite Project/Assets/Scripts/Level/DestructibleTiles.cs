using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleTiles : MonoBehaviour
{
    [SerializeField]
    private GameObject crater;

    public void DestroyTiles(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            Transform projectilePos = projectileData.HitPosition; // gets position of event's projectile
            float radius = projectileData.radius/5;

            GameObject maskObject = Instantiate(crater, projectilePos.position, Quaternion.FromToRotation(Vector3.up, projectileData.hitNormal));
            maskObject.transform.localScale = new Vector2(radius, radius);
        }
    }
}
