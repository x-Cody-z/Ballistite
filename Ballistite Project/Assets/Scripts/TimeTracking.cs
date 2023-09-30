using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTracking : MonoBehaviour
{
    private static TimeTracking testingInstance;
    private List<float> levelTimes = new List<float>();
    private List<string> levelNames = new List<string>();
    private string currentLevel;

    private float levelStartTime = 0f;
    private float levelEndTime = 0f;
    private GameObject OutroObject;

    private void Awake()
    {
        //lets the testing object persist across scenes
        DontDestroyOnLoad(gameObject);

        //stops duplicate testing objects being created when scene gets reloaded
        if (testingInstance == null)
            testingInstance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevel = scene.name;
        OutroObject = null;
        levelStartTime = 0f;
        levelEndTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        setLevelEndTime();

        if (Input.GetKeyDown(KeyCode.T))
        {
            getAllLevelTimes();
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void storeLevelTime()
    {
        float t = levelEndTime - levelStartTime;
        levelTimes.Add(t);
        levelNames.Add(currentLevel);
    }

    private float getTotalTime()
    {
        float totalTime = 0f;

        foreach(float f in levelTimes)
        {
            totalTime += f;
        }

        return totalTime;
    }

    //sets start time at the end of the cutscene
    public void setLevelStartTime(GameEventData eventData)
    {
        if (eventData.Sender is CutsceneController)
        {
            levelStartTime = Time.timeSinceLevelLoad;
        }
    }

    private void setLevelEndTime()
    {
        if (OutroObject == null)
        {
            if (GameObject.Find("win panel"))
                OutroObject = GameObject.Find("win panel");
            else if (GameObject.Find("OutroCutscene"))
                OutroObject = GameObject.Find("OutroCutscene");
        }

        if (OutroObject != null)
        {
            if (OutroObject.activeSelf && levelEndTime == 0f)
            {
                Debug.Log("storing level time");
                levelEndTime = Time.timeSinceLevelLoad;
                storeLevelTime();
            }
        }
    }

    private void getAllLevelTimes()
    {
        string result = "";
        
        for (int i = 0; i < levelTimes.Count; i++)
        {
            result += levelNames[i] + " time: " + levelTimes[i].ToString() + " seconds\n";
        }
        
        result += "\nTotal time: " + getTotalTime().ToString() + " seconds";

        Debug.Log(result);
    }

}
