using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New MapEvent", menuName = "Map Event")]
public class MapEvent : ScriptableObject
{
    [SerializeField]
    public string title = "Nameless Encounter";
    [SerializeField]
    public Texture2D image;
    [SerializeField]
    [TextArea(5,5)]
    public string eventDescription = "This event has no description.";
    [SerializeField]
    public int randomCardsAvailable = 5;
    [SerializeField]
    public List<MapEventConditional> conditions;
    [SerializeField]
    [TextArea(5, 5)]
    public string eventEnd = "This event has no end description.";
    public EventRewards rewards ;

    [Serializable]
    public class EventRewards
    {
        [SerializeField]
        public int neutralReward = 5;
        [SerializeField]
        public int goodReward = 10;
        [SerializeField] public int greatReward = 20;
        [SerializeField] public int perfectReward = 40;
    }


    private int GetResultRewards(Card card)
    {
        int i = 0;
        foreach (MapEventConditional condition in conditions)
        {
            i += condition.GetResult(card);
        }
        return i;
    }
    
}


