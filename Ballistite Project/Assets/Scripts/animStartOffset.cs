using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animStartOffset : MonoBehaviour
{
    public Animator anim;
    private float offset;
    private float speed;

    void Awake()
    {
        offset = Random.Range(0.1f,1.0f);
        speed = Random.Range(0.5f, 1.5f);
        StartCoroutine(AnimOffsetTime());
    }

    public IEnumerator AnimOffsetTime()
    {
        yield return new WaitForSeconds(offset);
        anim.Play("windmillAnim");
        anim.speed = speed;
    }
}
