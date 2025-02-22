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


    [SerializeField] public List<MonsterData> enemies;
    [SerializeField] public List<Vector2> positionsPercentage;

    
}
