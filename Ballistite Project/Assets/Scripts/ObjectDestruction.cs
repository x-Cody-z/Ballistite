using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    [SerializeField] private GameObject piecesPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        GameObject pieces = (GameObject)Instantiate(piecesPrefab);
        pieces.transform.position = this.transform.position;
        //pieces.transform.localScale = this.transform.localScale;
        Destroy(gameObject);
    }
}
