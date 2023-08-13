using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    public int destructionScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseScore(GameEventData eventData)
    {
        if(eventData is ObjectDesotryedEventData objDestroyedEventData)
        {
            destructionScore += objDestroyedEventData.destructionValue;
        }
    }
}
