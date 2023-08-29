using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    public float destructionScore = 0;

    private float craterRadius = 4.2f;
    private float lowCraterScale = 0.2f;
    private float medCraterScale = 0.32f;
    private float highCraterScale = 0.44f;
    private GameObject[] craterMasks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getCraterScore(GameEventData eventData)
    {
        if (eventData is ProjectileEventData projectileData)
        {
            craterMasks = GameObject.FindGameObjectsWithTag("Crater");
            float selfSize = (projectileData.radius / 5) * craterRadius;
            float startScore = selfSize; // low = 0.84, med = 1.344, high = 1.848
            float finalScore = startScore;
            foreach (GameObject c in craterMasks)
            {
                //these calculations and names are kinda bad
                //selfsize and cratersize are more like radius, craterRadius is the radius of the sprite before scaling
                float dist = Vector3.Distance(projectileData.HitPosition.position, c.transform.position);
                float craterSize = c.transform.localScale.x * craterRadius;
                //basically if 2 craters are touching each other
                if (dist < selfSize + craterSize)
                {
                    //decrease the destruction score based on the amount of overlap with other craters
                    //should scale linarly from 0 to score as dist increases from 0 to selfSize + craterSize
                    //if scales are different then above is not 100% true
                    //need to consider multiple craters

                    float wiggleRoom = craterSize - selfSize; // gets the difference between self size and crater size
                    float distScaler = dist / (2 * selfSize); // 0 - 1 value, 0 being when dist is 0 (so the craters are on top of each other)
                    float adjustedValue = (distScaler * startScore) - wiggleRoom; //uses scaler and wiggle room to create adjusted score value
                    if (adjustedValue < 0) // sets the floor to zero if dist falls within the wiggle room
                        adjustedValue = 0;
                    finalScore = (finalScore/startScore) * adjustedValue; //scales adjusted score value based on total amount of score reduction already

                }
            }
            destructionScore += finalScore;
            Debug.Log("destruction score adjusted by: " + finalScore);

        }
        
    }

    public void increaseScore(GameEventData eventData)
    {
        if(eventData is ObjectDesotryedEventData objDestroyedEventData)
        {
            destructionScore += objDestroyedEventData.destructionValue;
        }
    }
}
