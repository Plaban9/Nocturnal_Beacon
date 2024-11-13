using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayableData", menuName = "Unit/Monster")]
[Serializable]
public class MonsterData : UnitData
{
    [SerializeField] public EnemyBehavior behavior;
    [SerializeField] public bool isBoss = false;
    [SerializeField] public Color recolor = Color.white;



    //[Header("Monster Data")]
    //[SerializeField] public int startingHp = 100;
    // Start is called before the first frame update
}
