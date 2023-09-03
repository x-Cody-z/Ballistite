using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SetSceneCommand : Command
{
    public override string[] ids
    {
        get { return new string[] { "set_scene", "setscene", "set-scene", "scene_set", "sceneset", "scene-set", "scene", "ss" }; }
        set { }
    }

    public override string description
    {
        get { return "Set Scene: loads a scene based on its index in the build settings. (set_scene [index])"; }
        set { }
    }
        

    public override string processComand(string[] input)
    {
        if (int.TryParse(input[1], out int result))
        {
            if (SceneManager.sceneCountInBuildSettings > result)
            {
                SceneManager.LoadScene(result);
                return "loading scene " + input[1];
            }
            else
                return "selected scene does not exist";

        }
        else
            return "selected scene must be an integer";
    }
}
