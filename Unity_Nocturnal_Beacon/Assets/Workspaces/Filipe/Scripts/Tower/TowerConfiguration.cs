using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New TowerConfiguration", menuName = "Tower Configuration")]
public class TowerConfiguration : ScriptableObject
{
    public int height { get {
            return GetMaxHeight();
        } }
    [SerializeField] List<int> restHeights = new List<int>();
    [SerializeField] List<int> shopHeights = new List<int>();
    [SerializeField] int extraRest = 1;
    [SerializeField] int extraShop = 1;
    [SerializeField][Range(0f,1f)] public float questChance = 0.25f;

    public List<TowerEncounterData> encounterList = new List<TowerEncounterData>();
    public List<EnemyEncounter> bossList = new List<EnemyEncounter>();
    public List<MapQuest> questList = new List<MapQuest>();

    

    public int GetMaxHeight()
    {
        return GetHighestElement();
    }

    private int GetHighestElement()
    {
        int maxHeight = -1;
        foreach (int element in restHeights)
        {
            if (element +1 > maxHeight) { maxHeight = element+1; }
        }
        foreach (int element in shopHeights)
        {
            if (element +1  > maxHeight) { maxHeight = element+1; }
        }
        foreach(TowerEncounterData encData in encounterList)
        {
            if(encData.GetMaxHeight()+1 > maxHeight) { maxHeight = encData.GetMaxHeight()+1; }
        }

        return maxHeight;

    }

    public NodeType GetFloorType(int height)
    {
        if (restHeights.Contains(height))
        {
            return NodeType.Rest;
        }
        else if (shopHeights.Contains(height))
        {
            return NodeType.Shop;
        }
        else
        {
            return NodeType.Combat;
        }
    }

    public bool TryGetEncounter(int height, out EnemyEncounter encounter)
    {
        List<EnemyEncounter> fullList = new List<EnemyEncounter>();
        foreach(TowerEncounterData ted in encounterList)
        {
            List<EnemyEncounter> possibleList = ted.GetValidEncounterList(height);
            if(possibleList.Count > 0)
            {
                fullList.AddRange(possibleList);
            }
        }
        if (fullList.Count > 0)
            encounter = fullList.ElementAt<EnemyEncounter>(UnityEngine.Random.Range(0, fullList.Count));
        else
            encounter = default;

        return fullList.Count > 0;
    }

    public bool TryGetBossEncounter(out EnemyEncounter bossEncounter)
    {
        EnemyEncounter obtainedEncounter = bossList.ElementAt<EnemyEncounter>(UnityEngine.Random.Range(0, bossList.Count));
        bossEncounter = obtainedEncounter;
        return obtainedEncounter != null;
    }
    
}

