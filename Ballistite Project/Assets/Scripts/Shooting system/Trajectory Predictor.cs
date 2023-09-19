using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{

    LineRenderer trajectoryLine;
    Transform muzzle;
    [SerializeField] int resolution = 10;
    [SerializeField] float sampleRate = 0.1f;

    public int Resolution { get => resolution; set => resolution = value; }

    //Time in this case is the time in the future that the script is calculating the velocity for
    public Vector2 CalculateProjectileVelocity(Vector2 velocity, float time, float drag)
    {
        velocity += Physics2D.gravity * time;
        velocity *= Mathf.Clamp01(1f - drag * time);
        return velocity;
    }

    private void UpdateLineRenderer(int count, (int pointNum, Vector2 pos) pointPos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.pointNum, pointPos.pos);
    }

    public void CalculateTrajectory(ProjectileData data)
    {
        //calculate position of projectile over its flight time as a set of points
        Vector2 velocity = data.initialSpeed / data.mass * data.direction;
        Vector2 pos = data.initialPosition;
        Vector2 nextPos;

        UpdateLineRenderer(resolution, (0, pos));
        for (int i = 1; i < resolution; i++)
        {
            velocity = CalculateProjectileVelocity(velocity, sampleRate, data.drag);
            nextPos = pos + velocity * sampleRate;

            //if collision is detected, draw a line to the point of collision and stop drawing the line
            if (Physics.Raycast(pos, velocity.normalized, out RaycastHit hit, Vector2.Distance(pos, nextPos)))
            {
                UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }

            //Linerenderer between each point to draw a line
            pos = nextPos;
            UpdateLineRenderer(resolution, (i, pos));

        }

    }
    public void CalculateTrajectory(ProjectileData data, Vector2 endPos)
    {
        //calculate position of projectile over its flight time as a set of points
        Vector2 velocity = data.initialSpeed / data.mass * data.direction;
        Vector2 pos = data.initialPosition;
        Vector2 nextPos;

        UpdateLineRenderer(resolution, (0, pos));
        for (int i = 1; i < resolution; i++)
        {
            velocity = CalculateProjectileVelocity(velocity, sampleRate, data.drag);
            nextPos = pos + velocity * sampleRate;

            //if collision is detected, draw a line to the point of collision and stop drawing the line
            if (Physics.Raycast(pos, velocity.normalized, out RaycastHit hit, Vector2.Distance(pos, nextPos)))
            {
                UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }

            //Linerenderer between each point to draw a line
            pos = nextPos;
            UpdateLineRenderer(resolution, (i, pos));
            if (i == resolution - 1){
                UpdateLineRenderer(resolution, (i, endPos));
                break;
            }

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        if (trajectoryLine == null)
            trajectoryLine = GetComponent<LineRenderer>();
    }
}
