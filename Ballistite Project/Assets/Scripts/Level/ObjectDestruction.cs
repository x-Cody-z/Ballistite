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
            if ((transform.position - projectileData.HitPosition.position).magnitude < projectileData.radius*2)
            {
                GameObject pieces = (GameObject)Instantiate(piecesPrefab);
                updateChildrenData(pieces, projectileData.HitPosition.position);
                pieces.transform.position = this.transform.position;
                //pieces.transform.localScale = this.transform.localScale;
                Destroy(gameObject);
            }
        }
    }

    private void updateChildrenData(GameObject parent, Vector3 projectilePosistion)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform childTransform = parent.transform.GetChild(i);
            ObjectDestructibleForce childScript = childTransform.GetComponent<ObjectDestructibleForce>();

            if (childScript != null)
            {
                childScript.projectilePos = projectilePosistion; 
            }
        }
    }
}
