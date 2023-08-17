using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{

    LineRenderer trajectoryLine;
    Transform muzzle;
    [SerializeField] int resolution = 10;
    [SerializeField] float sampleRate = 0.1f;

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

    //Time in this case is the time in the future that the script is calculating the velocity for
    public Vector2 CalculateProjectileVelocity(Vector2 velocity, float time)
    {
        velocity += Physics2D.gravity * time;
        return velocity;
    }

    private void UpdateLineRenderer(int count, int pointNum, Vector2 pos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointNum, pos);
    }

    public void CalculateTrajectory(ProjectileData data)
    {
        //calculate position of projectile over its flight time as a set of points
        Vector2 velocity = data.initialSpeed / data.mass * data.direction;
        Vector2 pos = data.initialPosition;
        Vector2 nextPos;

        //raycast between each point to draw a line
        UpdateLineRenderer(resolution, 0, pos);
        for (int i = 1; i < resolution; i++)
        {
            velocity = CalculateProjectileVelocity(velocity, sampleRate);
            nextPos = pos + velocity * sampleRate;

            pos = nextPos;
            UpdateLineRenderer(resolution, i, nextPos);
        }

        //if collision is detected, draw a line to the point of collision and stop drawing the line
    }
    // Start is called before the first frame update
    void Start()
    {
        if (trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();
    }
}
