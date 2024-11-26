using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Unit/Monster")]
[Serializable]
public class MonsterData : UnitData
{
    [SerializeField] public EnemyBehavior behavior;
    [SerializeField] public bool isBoss = false;
    [SerializeField] public Color recolor = Color.white;
    [SerializeField] public List<Card> droppableCards = new List<Card>();
    [SerializeField] public int droppableGold = 0;


}
