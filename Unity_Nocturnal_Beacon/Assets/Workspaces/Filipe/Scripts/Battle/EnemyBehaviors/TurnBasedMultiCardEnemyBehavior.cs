using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewMonsterBehavior", menuName = "TurnBasedMultiCardEnemyBehavior")]
public class TurnBasedMultiCardEnemyBehavior : EnemyBehavior
{
    [SerializeField] List<List<Card>> _cards;
    [SerializeField] Card _cantActCard;
    [SerializeField] int _stagger = 0;
    public override List<Card> GetCardsUsed(BattleUnit owner, int turn)
    {
        int noAct = owner.GetUnitStatusData().OnGetNoAct();
        if(noAct > 0)
        {
            return new List<Card> { _cantActCard };
        }
        return _cards[turn+_stagger % _cards.Count];
    }
}
