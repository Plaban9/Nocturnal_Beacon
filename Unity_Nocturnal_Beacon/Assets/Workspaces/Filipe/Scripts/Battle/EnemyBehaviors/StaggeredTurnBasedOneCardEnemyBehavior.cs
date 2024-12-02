using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewMonsterBehavior", menuName = "StaggeredTurnBasedOneCardEnemyBehavior")]
public class StaggeredTurnBasedOneCardEnemyBehavior : EnemyBehavior
{
    [SerializeField] List<Card> _cards;
    [SerializeField] Card _cantActCard;
    [SerializeField] int _stagger = 0;
    public override List<Card> GetCardsUsed(BattleUnit owner, int turn)
    {
        int noAct = owner.GetUnitStatusData().OnGetNoAct();
        if(noAct > 0)
        {
            return new List<Card> { _cantActCard };
        }
        return new List<Card> { _cards[turn+_stagger % _cards.Count] };
    }
}
