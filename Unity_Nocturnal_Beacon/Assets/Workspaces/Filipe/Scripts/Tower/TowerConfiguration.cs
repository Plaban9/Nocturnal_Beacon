using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New TowerConfiguration", menuName = "Tower Configuration")]
public class TowerConfiguration : ScriptableObject
{
    [SerializeField] int height = 1;
    [SerializeField] List<int> restHeights = new List<int>();
    [SerializeField] List<int> shopHeights = new List<int>();
    [SerializeField] int extraRest = 1;
    [SerializeField] int extraShop = 1;
    public List<TowerEncounterData> encounterList = new List<TowerEncounterData>();
    public List<EnemyEncounter> bossList = new List<EnemyEncounter>();

    public enum FLOOR_TYPE
    {
        MONSTER_ENCOUNTER = 0,
        REST_EVENT,
        SHOP_EVENT,
        DECISION_EVENT,
        BOSS_EVENT
    }

    public int GetMaxHeight()
    {
        return height;
    }

    public FLOOR_TYPE GetFloorType(int i)
    {
        if (restHeights.Contains(i))
        {
            return FLOOR_TYPE.REST_EVENT;
        }
        else if (shopHeights.Contains(i))
        {
            return FLOOR_TYPE.SHOP_EVENT;
        }
        else
        {
            return FLOOR_TYPE.MONSTER_ENCOUNTER;
        }
    }

    public bool TryGetEncounter(int i, out EnemyEncounter encounter)
    {
        List<EnemyEncounter> fullList = new List<EnemyEncounter>();
        foreach(TowerEncounterData ted in encounterList)
        {
            List<EnemyEncounter> possibleList = ted.GetValidEncounterList(i);
            if(possibleList.Count > 0)
            {
                fullList.AddRange(possibleList);
            }
        }
        encounter = fullList.ElementAt<EnemyEncounter>(UnityEngine.Random.Range(0, fullList.Count - 1));
        return fullList.Count > 0;
    }

    public bool TryGetBossEncounter(out EnemyEncounter bossEncounter)
    {
        EnemyEncounter obtainedEncounter = bossList.ElementAt<EnemyEncounter>(UnityEngine.Random.Range(0, bossList.Count - 1));
        bossEncounter = obtainedEncounter;
        return obtainedEncounter != null;
    }
    
}

