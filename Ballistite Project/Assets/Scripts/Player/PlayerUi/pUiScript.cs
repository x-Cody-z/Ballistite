using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pUiScript : MonoBehaviour
{
    private GameObject PlayerObject;
    private BespokePlayerController PlayerScript;
    public Image chargeBar;
    private float chargeScale;
    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.Find("Player");
        if (PlayerObject)
        {
            PlayerScript = PlayerObject.GetComponent<BespokePlayerController>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerScript.Timer <= 1)
        {
            chargeScale = PlayerScript.Timer * 100;
            chargeBar.color = Color.white;
        }

        else if (PlayerScript.Timer > 1)
        {
            chargeScale = (PlayerScript.Timer - 1) * 100;
            chargeBar.color = Color.yellow;
        }
        /*
        else if (PlayerScript.Timer > 2)
            chargeScale = (PlayerScript.Timer - 2) * 100;
        */

        if (chargeScale >= 100)
        {
            chargeScale = 100;
            chargeBar.color = Color.red;
        }

        chargeBar.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.transform.position = (PlayerObject.transform.position) + new Vector3(0, 1, 0);
        chargeBar.rectTransform.sizeDelta = new Vector2(chargeScale, 100);
        
    }
}
