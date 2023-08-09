using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    private bool triggered = false;
    public bool respawnable = false;

    [SerializeField] private float collapseDelay = 1f;
    private float cdTimer;
    [SerializeField] private float respawnTime = 1f;
    private float rtTimer;

    [SerializeField] private GameObject objectToRespawn;
    private GameObject spawnedObject;


    // Start is called before the first frame update
    void Start()
    {

        // Spawn the initial object
        spawnedObject = Instantiate(objectToRespawn, this.transform.position, Quaternion.identity);
        spawnedObject.transform.localScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCollapse()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine(DestroyObject());
        }
        
    }

    private IEnumerator DestroyObject()
    {
        SpriteRenderer render = spawnedObject.GetComponent<SpriteRenderer>();
        render.color = Color.blue;
        // Wait for the specified amount of time
        for (cdTimer = collapseDelay; cdTimer > 0; cdTimer -= Time.deltaTime)
            yield return null;

        // Destroy the existing object
        Destroy(spawnedObject);

        if (respawnable)
            StartCoroutine(RespawnObject());
                
    }
    private IEnumerator RespawnObject()
    {
        // Wait for the specified amount of time
        for (rtTimer = respawnTime; rtTimer > 0; rtTimer -= Time.deltaTime)
            yield return null;

        // Spawn a new instance of the object
        spawnedObject = Instantiate(objectToRespawn, this.transform.position, Quaternion.identity);
        spawnedObject.transform.localScale = this.transform.localScale;
        triggered = false;
    }
}
