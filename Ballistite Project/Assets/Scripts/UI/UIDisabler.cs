using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisabler : MonoBehaviour
{
    public GameObject[] videoObjects;
    public bool videoBool;

    public GameObject caddy;
    public bool caddyBool;
    private bool caddyState;
    private bool switched;

    // Start is called before the first frame update
    void Start()
    {
        caddyState = caddy.activeSelf;
        caddyBool = true;
        videoBool = false;
    }

    public bool CaddyBool
    {
        get { return caddyBool; }
        set { caddyBool = value; }
    }

    public bool VideoBool
    {
        get { return videoBool; }
        set { videoBool = value; }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (videoBool)
            {
                foreach (GameObject o in videoObjects)
                {
                    if (o != null)
                    {
                        o.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (GameObject o in videoObjects)
                {
                    if (o != null)
                    {
                        o.SetActive(true);
                    }
                }
            }
            caddy.SetActive(caddyBool);
        }
        catch(Exception e)
        {
            
        }
    }

    public void ReenableVideos()
    {
        videoBool = false;
        foreach(GameObject o in videoObjects)
        {
            o.SetActive(true);
        }
    }

    public void CaddyState()
    {
        caddyState = caddy.activeSelf;
    }
}
