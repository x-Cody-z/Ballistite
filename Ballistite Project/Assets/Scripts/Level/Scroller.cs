using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    Material mat;
    float distance;
    [Range(0f, 0.5f)]
    public float speed = 0.2f;
    public GameObject BackgroundObj;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    public void DisableBG() {
        BackgroundObj.SetActive(false);
    }
    void Update()
    {
        distance += Time.deltaTime*speed;
        mat.SetTextureOffset("_MainTex", Vector3.right * distance);
    }
}
