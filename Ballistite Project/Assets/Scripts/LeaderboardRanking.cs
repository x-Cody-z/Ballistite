using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardRanking : MonoBehaviour
{
    public int leaderboardSize;
    public string leaderboardFilename;
    private Dictionary<float,string> AllTimes;
    private List<float> timeSortList;
    public TextMeshProUGUI[] leaderboardElements;

    // Start is called before the first frame update
    void Start()
    {
        AllTimes = new Dictionary<float, string>();
        gameObject.SetActive(false);
    }

    public void GenerateLeaderboard()
    {
        gameObject.SetActive(true);
        string[] lines = File.ReadAllLines(leaderboardFilename) ;
        foreach (string s in lines)
        {
            Debug.Log(s);
            string[] splitLine = s.Split(',');
            float key = float.Parse(splitLine[4]);
            if (AllTimes.ContainsKey(key))
            {
                while (AllTimes.ContainsKey(key))
                {
                    key += UnityEngine.Random.Range(0.00001f, 0.001f);
                }
            }
            AllTimes.Add(key, splitLine[0]);
        }
        timeSortList = AllTimes.Keys.ToList();
        timeSortList.Sort();
        for (int i = 0; i < timeSortList.Count(); i++)
        {
            leaderboardElements[i].text = (i+1).ToString() + ". " + AllTimes[timeSortList[i]] + " : " + ConvertFloatToTime(timeSortList[i]);
        }
        foreach (TextMeshProUGUI t in leaderboardElements)
        {
            if (t.text == "-")
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    string ConvertFloatToTime(float timeInSeconds)
    {

        int minutes = (int)(timeInSeconds / 60);
        int seconds = (int)(timeInSeconds % 60);
        int milliseconds = (int)((timeInSeconds * 1000) % 1000);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);

        return formattedTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
