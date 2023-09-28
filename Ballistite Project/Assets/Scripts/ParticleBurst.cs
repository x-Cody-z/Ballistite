using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ParticleBurst : MonoBehaviour
{
    [SerializeField] ParticleSystem burst;
    [SerializeField] int burstAmount = 10;
    PolygonCollider2D c;

    private void Start()
    {
        c = GetComponent<PolygonCollider2D>();
    }

    public void PlayBurst(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            if(projectileData.HitPosition == null)
            {
                return;
            }
            else
            {

                
                if (projectileData.HitPosition.position.x < c.bounds.max.x && projectileData.HitPosition.position.x > c.bounds.min.x)
                {
                    burst.transform.position = projectileData.HitPosition.position+ new Vector3(0,0.2f,0);
                    burst.Emit(burstAmount);
                }
            }
        }
    }
}
