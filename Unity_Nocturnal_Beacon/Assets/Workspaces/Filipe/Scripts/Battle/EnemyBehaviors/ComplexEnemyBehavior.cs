using CardAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewMonsterBehavior", menuName = "ComplexEnemyBehavior")]
public class ComplexEnemyBehavior : EnemyBehavior
{
    [SerializeField] List<ComplexBehaviorObject> _complexBehavior;
    [SerializeField] Card _cantActCard;
    [SerializeField] int _stagger = 0;
    public override List<Card> GetCardsUsed(BattleUnit owner, int turn)
    {
        List<Card> currentTurnCards = new List<Card>();
        int noAct = owner.GetUnitStatusData().OnGetNoAct();
        if(noAct > 0)
        {
            return new List<Card> { _cantActCard };
        }

        foreach (ComplexBehaviorObject complexBehavior in _complexBehavior)
        {
            if (complexBehavior.criteria.FitsCriteria(owner, BattleManager.Instance, _stagger)){
                currentTurnCards.Add(complexBehavior.playedCard);
            }
        }
        return currentTurnCards;
    }
}

[Serializable]
public class ComplexBehaviorObject
{
    [SerializeField] public Card playedCard;
    [SerializeReference, SubclassSelector] public IBattleFitsCriteria criteria;
}

public interface IBattleFitsCriteria
{
    public bool FitsCriteria(BattleUnit owner, BattleManager battleManager, int stagger = 0);
}

[Serializable]
public class BattleCriteria_CurrentTurn : IBattleFitsCriteria
{
    [SerializeField] int turn = 2;

    public bool FitsCriteria(BattleUnit owner, BattleManager battleManager, int stagger)
    {
        int currTurn = battleManager.GetCurrentTurn();
        if(currTurn + stagger == turn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
[Serializable]
public class BattleCriteria_CurrentTurnModulo : IBattleFitsCriteria
{
    [SerializeField] int turnModulo = 2;

    public bool FitsCriteria(BattleUnit owner, BattleManager battleManager, int stagger)
    {
        int currTurn = battleManager.GetCurrentTurn();
        if ((currTurn + stagger) % turnModulo == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[Serializable]
public class BattleCriteria_HPIs : IBattleFitsCriteria
{
    [SerializeField] BattleCriteria_Target whoseHp;
    [SerializeField] public ValueComparator valueComparator;
    [SerializeField][Range(0f, 1f)] public float threshold;

    public bool FitsCriteria(BattleUnit owner, BattleManager battleManager, int stagger)
    {
        BattleUnit unit = whoseHp == BattleCriteria_Target.SELF ? owner : battleManager.GetPlayerbattleUnit();
        float percent = ((float)unit.GetHPData().GetCurrentHP() / (float)unit.GetUnitData().maxHp);

        switch (valueComparator)
        {
            case ValueComparator.GreaterThan:
                return threshold < percent ? true : false;
            case ValueComparator.LessThan:
                return threshold > percent ? true : false;
            case ValueComparator.EqualTo:
                return threshold == percent ? true : false;
            case ValueComparator.NotEqualTo:
                return threshold != percent ? true : false;
            case ValueComparator.GreaterThanOrEqualTo:
                return threshold <= percent ? true : false;
            case ValueComparator.LessThanOrEqualTo:
                return threshold >= percent ? true : false;
        }
        return false;
    }
}

[Serializable]
public class BattleCriteria_ElementalAffinityTo : IBattleFitsCriteria
{
    [SerializeField] BattleCriteria_Target whoseAffinity;
    [SerializeField] BattleCriteria_Affinity resistance;
    [SerializeField] Element element;

    public bool FitsCriteria(BattleUnit owner, BattleManager battleManager, int stagger)
    {
        BattleUnit judgedUnit = whoseAffinity == BattleCriteria_Target.SELF ? owner : battleManager.GetPlayerbattleUnit();

        switch(resistance)
        {
            case BattleCriteria_Affinity.WEAK:
                return (int) judgedUnit.GetElementalAffinity(element) < (int) ElementalEffectivity.NEUTRAL;
            case BattleCriteria_Affinity.NEUTRAL:
                return (int)judgedUnit.GetElementalAffinity(element) == (int)ElementalEffectivity.NEUTRAL;
            case BattleCriteria_Affinity.RESISTANT:
                return (int)judgedUnit.GetElementalAffinity(element) > (int)ElementalEffectivity.NEUTRAL;
        }    
        
        return false;
    }
}


public enum BattleCriteria_Affinity
{
    WEAK,
    NEUTRAL,
    RESISTANT
}

public enum BattleCriteria_Target
{
    SELF,
    PLAYER
}