using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camResolutionScaler : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        int res = Screen.width;
        if (res == 1920)
        {
            vcam.m_Lens.OrthographicSize = 5f;
        }
        else if (res == 900)
        {
            vcam.m_Lens.OrthographicSize = 6f;
        }   
    }
}
