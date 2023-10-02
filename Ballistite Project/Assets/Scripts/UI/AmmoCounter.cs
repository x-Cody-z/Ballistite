using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    public Shooter playerObject;
    public TextMeshProUGUI ammoText;
    public Animator shellAnimator;
    private int ammoCount;

    // Update is called once per frame
    void Update()
    {
        ammoCount = playerObject.ShotCount;
        shellAnimator.SetInteger("ammo", ammoCount);
        ammoText.text = ammoCount.ToString() + "/2";
        
    }
}
