using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundFade : MonoBehaviour
{
    public Animator foregroundImage;
    private bool inBuilding;

    // Start is called before the first frame update
    void Start()
    {
        inBuilding = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter building");
            if (!inBuilding)
            {
                foregroundImage.Play("Fade_Out_Image");
                inBuilding = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Exit building");
            if (inBuilding)
            {
                foregroundImage.Play("Fade_In_Image");
                inBuilding = false;
            }
        }
    }
}
