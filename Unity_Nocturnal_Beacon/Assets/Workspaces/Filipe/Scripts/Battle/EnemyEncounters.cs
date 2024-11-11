using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New EnemyEncounter", menuName = "Enemy Encounter")]
public class EnemyEncounter : ScriptableObject
{
    [SerializeField] public List<MonsterData> enemies;
    [SerializeField] public List<int> positions;
}
