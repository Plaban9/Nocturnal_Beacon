using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New EnemyEncounter", menuName = "Enemy Encounter")]
public class EnemyEncounter : ScriptableObject
{
    //THIS SHIT IS HARDCODED, IT IS TERRIBLE, BUT ITS EASIER TO KNOW HOW TO PLACE WITHIN THE FIELD LIKE THAT
    float minX = 0.23f;
    float maxX = 3.73f;
    float minZ = 0.47f;
    float maxZ = 2.75f;

    [SerializeField] public List<MonsterData> enemies;
    [SerializeField] public List<Vector2> positionsPercentage;

    public float GetX(int i)
    {
        if(i >= 0 && i < positionsPercentage.Count )
        {
            return minX + positionsPercentage[i].x * (maxX - minX);
        }
        else
        {
            return -1f;
        }
    }

    public float GetZ(int i)
    {
        if (i >= 0 && i < positionsPercentage.Count)
        {
            return minZ + positionsPercentage[i].y * (maxZ - minZ);
        }
        else
        {
            return -1f;
        }
    }
}
