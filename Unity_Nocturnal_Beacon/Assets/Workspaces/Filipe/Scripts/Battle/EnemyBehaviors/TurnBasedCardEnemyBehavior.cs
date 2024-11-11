using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewMonsterBehavior", menuName = "TurnBasedCardEnemyBehavior")]
public class TurnBasedCardEnemyBehavior : EnemyBehavior
{
    [SerializeField] List<Card> _cards;

    public override Card GetCardUsed(BattleUnit owner, int turn)
    {
        return _cards[turn % _cards.Count];
    }
}
