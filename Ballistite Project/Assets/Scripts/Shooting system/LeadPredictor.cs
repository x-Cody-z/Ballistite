using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadPredictor : MonoBehaviour
{

    /// <summary>
    /// Calculates the position the projectile should be fired at to hit a moving target
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="projectileSpeed"></param>
    /// <returns>Returns a vector2 representing the position that should be aimed at to hit target</returns>
    public Vector2 CalculateLead(Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
    {
        Vector2 targetDirection = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        float distance = targetDirection.magnitude;
        float timeToTarget = distance / projectileSpeed;

        return targetPosition + targetVelocity * timeToTarget;
    }

    /// <summary>
    /// Calculates the speed of a projectile
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="power"></param>
    /// <returns>a float representing projectile speed</returns>
    public float CalculateProjectileSpeed(ProjectileData projectile, float power)
    {
        return power / projectile.mass;
    }
}
