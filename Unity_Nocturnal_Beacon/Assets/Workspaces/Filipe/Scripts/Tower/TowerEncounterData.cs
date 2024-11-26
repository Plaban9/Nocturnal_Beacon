using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TowerEncounterData 
{

    [SerializeField] List<EnemyEncounter> _possibleEnemies;
    [SerializeField] int minHeight = 0;
    [SerializeField] int maxHeight = 0;

    public List<EnemyEncounter> GetValidEncounterList(int height)
    {
        if(height >= minHeight && maxHeight > height)
        {
            return _possibleEnemies;
        }
        else
        {
            return new List<EnemyEncounter>();
        }
    }

    public int GetMaxHeight() { return maxHeight; }

}
