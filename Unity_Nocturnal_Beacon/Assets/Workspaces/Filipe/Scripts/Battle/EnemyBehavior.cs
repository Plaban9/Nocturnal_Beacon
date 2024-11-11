using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehavior : ScriptableObject
{
    public virtual Card? GetCardUsed(BattleUnit owner, int turn)
    {
        return null;
    }
}
