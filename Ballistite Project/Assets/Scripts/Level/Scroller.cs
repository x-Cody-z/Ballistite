using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;

    Material mat;
    float distance;

    [Range(0f, 0.5f)]
    public float speed = 0.2f;

    private void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(cam.position.x, cam.position.y, 5);
    }
}
