using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pUiScript : MonoBehaviour
{
    private GameObject PlayerObject;
    private Shooter shooter;
    private BespokePlayerController PlayerScript;
    public Image chargeBar;
    public Image chargeBar1;
    public Image chargeBar2;
    private float chargeScale;
    private float chargeScale1;
    private float chargeScale2;

    private Color orangeOff;
    private Color orangeOn;
    private Color redOff;
    private Color redOn;

    [Tooltip("1 for the new charge bar, 2 for a mix of old and new, 3 for fun")]
    public int visualMode = 1;
    private float x;
    private float y;

    // Start is called before the first frame update
    
    void Start()
    {
        //gets the player controller script for access to charge timer values
        PlayerObject = GameObject.Find("Player");
        if (PlayerObject)
        {
            PlayerScript = PlayerObject.GetComponent<BespokePlayerController>();
            shooter = PlayerObject.GetComponent<Shooter>();
        }

        //setting a few colour to use for the charge bar
        orangeOff = new Color(0.58f, 0.39f, 0f);
        orangeOn = new Color(1f, 0.68f, 0f);
        redOff = new Color(0.54f, 0f, 0.1f);
        redOn = new Color(1f, 0f, 0.2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //used for testing/ review so modes can be change in the editor
        if (visualMode == 2)
            calculateChargeNew2();
        else if (visualMode == 3)
            calculateChargeThree();
        else
            calculateChargeNew();

        //stops the bar from rotating with the tank and offsets its position
        this.transform.rotation = Quaternion.Euler(0, 0, 180);
        this.transform.position = (PlayerObject.transform.position) + new Vector3(0, 1, 0);

    }

    //keeps the entire bar the same colour, changing when different segments are reached, a mix between old and new
    private void calculateChargeNew2()
    {
        if (PlayerScript.chargeTimer <= 1)
        {
            setValues(PlayerScript.chargeTimer * 100, 0, Color.white, redOff);
        }

        else if (PlayerScript.chargeTimer > 1)
        {
            setValues(100, (PlayerScript.chargeTimer - 1) * 100, orangeOn, orangeOn);
        }

        if (chargeScale2 >= 100)
        {
            setValues(100, 100, redOn, redOn);
        }
    }


    //while filling, each segment is a darker colour
    //once a segment is completely filled, it becomes brighter and the next segments starts filling in a different colour
    private void calculateChargeNew()
    {
        if (PlayerScript.chargeTimer <= 1)
        {
            setValues(PlayerScript.chargeTimer * 100, 0, orangeOff, redOff);
        }

        else if (PlayerScript.chargeTimer > 1)
        {
            setValues(100, (PlayerScript.chargeTimer - 1) * 100, orangeOn, redOff);
        }
        
        if (chargeScale2 >= 100)
        {
            setValues(100, 100, orangeOn, redOn);
        }
    }

    //rainbow
    private void calculateChargeThree()
    {
        float hue = (Mathf.Cos(3.14f * PlayerScript.chargeTimer) + 1) / 2;
        Color dark = Color.HSVToRGB(hue, 1f, 0.6f);
        Color light = Color.HSVToRGB(hue, 1f, 1f);
        if (PlayerScript.chargeTimer <= 1)
        {
            setValues(PlayerScript.chargeTimer * 100, 0, dark, dark);
        }

        else if (PlayerScript.chargeTimer > 1)
        {
            setValues(100, (PlayerScript.chargeTimer - 1) * 100, light, dark);
        }

        if (chargeScale2 >= 100)
        {
            setValues(100, 100, light, light);
        }
    }

    //used to set the scale, colour and size
    private void setValues(float scale1, float scale2, Color bar1, Color bar2)
    {
        chargeScale1 = scale1;
        chargeScale2 = scale2;
        chargeBar1.color = bar1;
        chargeBar2.color = bar2;
        chargeBar1.rectTransform.sizeDelta = new Vector2(chargeScale1 * 2, 100);
        chargeBar2.rectTransform.sizeDelta = new Vector2(chargeScale2 * 2, 100);
    }
}
