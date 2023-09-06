using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float parallaxStrength = 1f; // Adjust this value to control the parallax effect

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        transform.position += deltaMovement * parallaxStrength;
        previousCameraPosition = cameraTransform.position;
    }
}
