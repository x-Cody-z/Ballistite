using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingTools : MonoBehaviour
{
    private int sceneCount;
    private static TestingTools testingInstance;
    [SerializeField] private TMPro.TMP_InputField consoleInput;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMPro.TMP_Text resultText;

    private string inputString;
    private Command[] commandArray;

    public bool hotkeysEnabled = false;

    private List<string> inputLog = new List<string>();
    private int logIndex = 0;
    private List<string> outputLog = new List<string>();

    
    private List<float> frameTimes = new List<float>();
    public bool fpsLogging = false;

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

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        commandSetup();
    }

    // Update is called once per frame
    void Update()
    {
        //displays the console if its not visible and vice versa
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            consoleToggle();
        }

        //submits the current text in the console
        if (canvas.enabled == true && Input.GetKeyDown(KeyCode.Return))
        {
            //adds the input to a log so we can backtrack commands
            inputLog.Add(consoleInput.text);
            logIndex = inputLog.Count;

            //start processing the input and reset the input field
            resultText.text = processInput();
            consoleInput.text = "";
            consoleInput.ActivateInputField();

            //adds the result text to a list for logging
            outputLog.Add(resultText.text);
        }

        //increments through the input log to autofill the input field with a previous command
        if (canvas.enabled == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            logIndex -= 1;
            //wraps around when out of range
            if (logIndex < 0)
                logIndex = inputLog.Count - 1;
            //checks that input log has anything in it
            if (inputLog.Count > 0)
                consoleInput.text = inputLog[logIndex];
        }
        //same as above but in opposite direction
        if (canvas.enabled == true && Input.GetKeyDown(KeyCode.DownArrow))
        {
            logIndex += 1;
            if (logIndex > inputLog.Count - 1)
                logIndex = 0;

            if (inputLog.Count > 0)
                consoleInput.text = inputLog[logIndex];
        }



        //hotkeys for changing scene and teleporting
        if (canvas.enabled == false && hotkeysEnabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && SceneManager.sceneCountInBuildSettings > 0)
                SceneManager.LoadScene(0);
            if (Input.GetKeyDown(KeyCode.Alpha2) && SceneManager.sceneCountInBuildSettings > 1)
                SceneManager.LoadScene(1);
            if (Input.GetKeyDown(KeyCode.Alpha3) && SceneManager.sceneCountInBuildSettings > 2)
                SceneManager.LoadScene(2);
            if (Input.GetKeyDown(KeyCode.Alpha4) && SceneManager.sceneCountInBuildSettings > 3)
                SceneManager.LoadScene(3);
            if (Input.GetKeyDown(KeyCode.Alpha5) && SceneManager.sceneCountInBuildSettings > 4)
                SceneManager.LoadScene(4);


            if(Input.GetKeyDown(KeyCode.RightArrow) && GameObject.Find("Player"))
                GameObject.Find("Player").transform.position += new Vector3(10, 0, 0);
            if (Input.GetKeyDown(KeyCode.LeftArrow) && GameObject.Find("Player"))
                GameObject.Find("Player").transform.position += new Vector3(-10, 0, 0);
            if (Input.GetKeyDown(KeyCode.UpArrow) && GameObject.Find("Player"))
                GameObject.Find("Player").transform.position += new Vector3(0, 10, 0);
            if (Input.GetKeyDown(KeyCode.DownArrow) && GameObject.Find("Player"))
                GameObject.Find("Player").transform.position += new Vector3(0, -10, 0);

            
        }

        if (fpsLogging)
        {
            frameTimes.Add(Time.deltaTime);
        }
    }

    //enables or disables the console's canvas object, resets input text when closed
    private void consoleToggle()
    {
        if (canvas.enabled == false)
        {
            canvas.enabled = true;
            //so you don't have to click in the text box
            consoleInput.ActivateInputField();
        }
        else
        {
            canvas.enabled = false;
            consoleInput.text = "";
        }
        
    }

    //splits the input string and compares the first word with all command ids
    private string processInput()
    {
        inputString = consoleInput.text.ToLower();
        string[] inputArray = inputString.Split(' ');
        foreach (Command c in commandArray)
        {
            foreach (string s in c.ids)
            {
                if (s == inputArray[0])
                {
                    return c.processComand(inputArray);
                }
            }
        }
        return "Invalid command. Type \"help\" to get a list of possible commands.";
    }

    //create an array of commands, 1 of each type of command
    private void commandSetup()
    {
        commandArray = new Command[]
        {
            new HelpCommand(),
            new SetSceneCommand(),
            new ListScenesCommand(),
            new HotKeyEnableCommand(),
            new GetPosCommand(),
            new SetPosCommand(),
            new LogCommand(),
            new PerformanceLogCommand(),
            new AddAmmoCommand(),
            new SetDestructionScore(),
            new UnfreezeCommand()
        };
    }

    //function used by the help command to list the description of all commands
    public string help()
    {
        string s = "";
        foreach (Command c in commandArray)
        {
            s += " - " + c.description + "\n\n";
        }
        return s;
    }

    //function used by log command to get all the input and output text
    public string logtext()
    {
        string result = "";
        string[] combinedLog = new string[inputLog.Count + outputLog.Count + 1];
        for (int i = 0; i < inputLog.Count; i++)
        {
            combinedLog[i * 2] = "INPUT (" + i.ToString() + "): " + inputLog[i];
        }
        for (int j = 0; j < outputLog.Count; j++)
        {
            combinedLog[(j * 2) + 1] = "OUTPUT (" + j.ToString() + "): " + outputLog[j] + "\n";
        }

        foreach (string s in combinedLog)
        {
            result += s + "\n";
        }

        return result;
    }

    //function used by performance log command
    public void logFpsStart()
    {
        frameTimes.Clear();
        fpsLogging = true;
    }

    public List<float> logFpsStop()
    {
        fpsLogging = false;
        return frameTimes;
    }
}
