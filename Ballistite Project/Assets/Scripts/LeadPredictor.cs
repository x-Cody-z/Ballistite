using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadPredictor : MonoBehaviour
{
    public float CalculateProjectileSpeed(GameObject projectile, float power)
    {
        return (/*calcForce() **/ power / projectile.GetComponent<Rigidbody2D>().mass);
    }

    /// <summary>
    /// Calculates the position the projectile should be fired at to hit a moving target
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetVelocity"></param>
    /// <param name="projectileSpeed"></param>
    /// <returns>Returns a vector3 representing the position that should be aimed at to hit target</returns>
    public Vector2 CalculateLead(Vector2 targetPosition, Vector2 targetVelocity, float projectileSpeed)
    {
        Vector2 targetDirection = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        float distance = targetDirection.magnitude;
        float timeToTarget = distance / projectileSpeed;

        return targetPosition + targetVelocity * timeToTarget;
    }
}
