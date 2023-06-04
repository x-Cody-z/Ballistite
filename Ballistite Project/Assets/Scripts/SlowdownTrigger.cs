using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownTrigger : MonoBehaviour
{
    public bool slowed = false;
    [Tooltip("1.0 is normal speed, 0.5 for half speed, 2.0 for 2x speed, etc.")]
    public float slowdownAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1") && slowed)
        {
            resetSlowdown();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startSlowdown(slowdownAmount);
        }
    }

    void startSlowdown(float ts)
    {
        Time.timeScale = ts;
        slowed = true;
    }

    void resetSlowdown()
    {
        Time.timeScale = 1;
        slowed = false;
    }
}
