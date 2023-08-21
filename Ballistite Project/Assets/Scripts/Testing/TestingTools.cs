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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (testingInstance == null)
            testingInstance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        canvas.enabled = false;
        commandSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            consoleToggle();
        }

        if (canvas.enabled == true && Input.GetKeyDown(KeyCode.Return))
        {
            resultText.text = processInput();
            consoleInput.text = "";
            consoleInput.ActivateInputField();
        }

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
    }

    private void consoleToggle()
    {
        if (canvas.enabled == false)
        {
            canvas.enabled = true;
            consoleInput.ActivateInputField();
        }
        else
        {
            canvas.enabled = false;
            consoleInput.text = "";
        }
        
    }

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

    private void commandSetup()
    {
        commandArray = new Command[]
        {
            new HelpCommand(),
            new SetSceneCommand(),
            new ListScenesCommand(),
            new HotKeyEnableCommand()
        };
    }

    public string help()
    {
        string s = "";
        foreach (Command c in commandArray)
        {
            s += c.description + "\n";
        }
        return s;
    }
}
