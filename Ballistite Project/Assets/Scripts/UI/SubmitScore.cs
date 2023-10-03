using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField textInput;
    [SerializeField] private TMPro.TMP_Text timeTmp;
    [SerializeField] private TMPro.TMP_Text confirmationTmp;
    private GameObject timeTracker;
    private TimeTracking timeTrackerScript;

    // Start is called before the first frame update
    void Start()
    {
        confirmationTmp.text = "";

        if (GameObject.Find("TimeTracker"))
        {
            timeTracker = GameObject.Find("TimeTracker");
        }

        if(timeTracker != null)
        {
            timeTrackerScript = timeTracker.GetComponent<TimeTracking>();
        }

        if (timeTrackerScript != null)
        {
            TimeSpan convertedTime = TimeSpan.FromSeconds(timeTrackerScript.getTotalTime());
            timeTmp.text = convertedTime.ToString("mm':'ss':'fff");
        }
    }

    // Update is called once per frame
    void Update()
    {
        textInput.Select();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //adds the input to a log so we can backtrack commands
            submitName(textInput.text);
        }
    }

    private void submitName(string name)
    {
        if (timeTrackerScript != null)
        {
            timeTrackerScript.saveRun(name);
            confirmationTmp.text = "Time Submitted!";
        }
    }

    public void selectInput()
    {
        textInput.ActivateInputField();
        textInput.Select();
    }

}
