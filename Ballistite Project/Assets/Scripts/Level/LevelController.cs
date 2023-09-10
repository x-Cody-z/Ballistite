using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Events for each condition
    public GameEvent enemyDestroyedEvent;
    public GameEvent enviroDestroyedEvent;
    public GameEvent timeMetEvent;
    public GameEvent progressMetEvent;

    [Header("Conditions")]
    public int enemyDestroyGoal;
    public int enviroDestroyGoal;
    public float timeGoal;

    public List<float> progressGoals = new List<float> { 0.4f, 0.6f, 0.8f };
    private HashSet<float> reachedGoals = new HashSet<float>();

    public GameObject startPointObject;
    public GameObject finishPointObject;
    private Vector2 startPoint;
    private Vector2 finishPoint;

    private int enemyDestroyCount = 0;
    private int enviroDestroyCount = 0;
    private float levelStartTime;
    private float progress = 0.0f;

    private bool gameStarted;
    private bool timeMet = false;

    public Transform tank;

    private void Start()
    {
        startPoint = startPointObject.transform.position;
        finishPoint = finishPointObject.transform.position;
        gameStarted = false;
    }

    private void Update()
    {
        if (gameStarted)
        {
            levelStartTime += Time.deltaTime;
        }
        float totalDistance = Vector2.Distance(startPoint, finishPoint);
        float currentDistance = Vector2.Distance(tank.position, finishPoint);
        progress = 1.0f - (currentDistance / totalDistance);
        CheckConditions();
    }

    private void CheckConditions()  // Resets to avoid multiple triggers per event once met
    {
        GameEventData eventData = new GameEventData { Sender = this };

        if (enemyDestroyCount >= enemyDestroyGoal)
        {
            enemyDestroyCount = 0;
            enemyDestroyedEvent.Raise(eventData);
        }

        if (levelStartTime >= timeGoal && !timeMet)
        {
            levelStartTime = float.MaxValue;
            timeMet = true;
            timeMetEvent.Raise(eventData);
        }

        if (enviroDestroyCount >= enviroDestroyGoal)
        {
            enviroDestroyCount = 0;
            enviroDestroyedEvent.Raise(eventData);
        }

        foreach (float goal in progressGoals)
        {
            if (progress >= goal && !reachedGoals.Contains(goal))
            {
                reachedGoals.Add(goal);
                ProgressEventData progressEventData = new ProgressEventData { Sender = this, Goal = goal };
                progressMetEvent.Raise(progressEventData);
            }
        }
    }

    public void OnEnemyDestroyed()
    {
        enemyDestroyCount++;
    }

    public void OnEnvironmentDestroyed()
    {
        enviroDestroyCount++;
    }

    public void PlayGameTime()
    {
        gameStarted = true;
        levelStartTime = 0.0f;
    }
}