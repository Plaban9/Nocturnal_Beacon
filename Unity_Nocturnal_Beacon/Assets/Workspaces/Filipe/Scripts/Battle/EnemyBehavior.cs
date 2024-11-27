using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehavior : ScriptableObject
{
    public virtual List<Card?> GetCardsUsed(BattleUnit owner, int turn)
    {
        return null;
    }
}
