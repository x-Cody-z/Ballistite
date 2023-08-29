using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ListScenesCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "list_scenes", "list-scenes", "listscenes", "scenes", "ls" }; }
        set { }
    }
    public override string description
    {
        get { return "List Scenes: displays a list of all scenes in the build. (list_scenes)"; }
        set { }
    }

    public override string processComand(string[] input)
    {
        string output = "";
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            output += "index: [" + i.ToString() + "] Name: ";
            output += sceneName(i);
            output += "\n";
        }
        return output;
    }

    private string sceneName(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

}
