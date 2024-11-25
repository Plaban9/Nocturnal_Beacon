using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCards : MonoBehaviour
{
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "data/cards");

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            Card cardData = JsonUtility.FromJson<Card>(jsonString);
            Debug.Log("Player Name: " + cardData.name);
            Debug.Log("Player Score: " + cardData.GetManaCost());
        }
        else
        {
            Debug.LogError("JSON file not found at " + path);
        }
    }
}
