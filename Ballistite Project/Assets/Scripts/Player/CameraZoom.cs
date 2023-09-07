using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private GameObject player;
    [SerializeField] CinemachineVirtualCamera vcamMouse;
    [SerializeField] CinemachineVirtualCamera vcamPlayer;

    private float orthoTargetSize = 4f;
    private float orthoCurrentSize;

    [SerializeField] private float veloScaling = 0.1f;
    private float baseZoom = 4f;

    private float zoomVelo = 0f;
    private int frameDelay = 10;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null && vcamMouse != null && vcamPlayer != null)
        {
            //zoom camera based on player velocity
            //multiply velocity to scale it down, so camera doesnt zoom out too much
            //add a base zoom level, so ortho size isnt 0 when stationary
            orthoCurrentSize = vcamPlayer.m_Lens.OrthographicSize;
            orthoTargetSize = (player.GetComponent<Rigidbody2D>().velocity.magnitude * veloScaling) + baseZoom;
            updateZoom();
        }
    }

    private void updateZoom()
    {
        float diff = orthoTargetSize - orthoCurrentSize;
        zoomVelo = diff / frameDelay;

        //limit extreme zoom level changes
        if (zoomVelo > 0.1f)
            zoomVelo = 0.1f;
        if (zoomVelo < -0.1f)
            zoomVelo = -0.1f;

        vcamMouse.m_Lens.OrthographicSize += zoomVelo;
        vcamPlayer.m_Lens.OrthographicSize += zoomVelo;
    }
}
