using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    [SerializeField] private GameObject piecesPrefab;


    public void DestroySelf(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData) // TODO: Break only when projectileData.HitPosition is close to the object
        {
            GameObject pieces = (GameObject)Instantiate(piecesPrefab);
            pieces.transform.position = this.transform.position;
            //pieces.transform.localScale = this.transform.localScale;
            Destroy(gameObject);
        }
    }
}
