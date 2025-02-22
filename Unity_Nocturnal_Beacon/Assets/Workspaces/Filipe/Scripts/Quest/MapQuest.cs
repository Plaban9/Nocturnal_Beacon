using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New MapEvent", menuName = "Map Event")]
public class MapQuest : ScriptableObject
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
    public List<MapQuestConditional> conditions;
    [SerializeField]
    [TextArea(5, 5)]
    public string questEnd = "This event has no end description.";
    public EventRewards rewards ;

    [Serializable]
    public class EventRewards
    {
        [SerializeField]
        public int neutralReward = 5;
        [SerializeField]
        public int decentReward = 10;
        [SerializeField] public int goodReward = 20;
        [SerializeField] public int greatReward = 40;
        [SerializeField] public int perfectReward = 60;


        public int GetReward(int i)
        {
            switch (i)
            {
                case 0:
                    return neutralReward;
                case 1:
                    return decentReward;
                case 2:
                    return goodReward;
                case 3:
                    return greatReward;
                case 4:
                    return perfectReward;
            }
            return neutralReward;
        }
    }


    public int GetResultRewards(Card card)
    {
        int i = 0;
        foreach (MapQuestConditional condition in conditions)
        {
            i += condition.GetResult(card);
        }
        return i;
    }
    
}


