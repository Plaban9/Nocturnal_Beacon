using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyHand : CardEffect
{
    [SerializeReference, SubclassSelector] IModifyHandStrategies strategy;

    public override string LocalizationKey => "CE_DESC_ModifyHand";

    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result = strategy.GetString();

            return result;
        }
    }

    public override string EffectDetailDescription
    {
        get
        {
            string result = strategy.GetString();
            return result;
        }
    }

    public override void SetMainValue(int val)
    {
        base.SetMainValue(val);
        
        if(val1 > 0)
        {
            strategy = new GetNextFromDeck();
            (strategy as GetNextFromDeck).amount = val1;
        }
        else
        {
            strategy = new DiscardFromHand();
            (strategy as DiscardFromHand).amount = -val1;
        }
    }
    public override int GetEffectCost()
    {
        int val = 0;

        return strategy.GetCost();
    }

    public ModifyHand()
    {
    }

    public ModifyHand(int i) {
        val1 = i;
        if(val1 > 0)
        {
            strategy = new GetNextFromDeck();
            (strategy as GetNextFromDeck).amount = val1;
            effectType = EffectType.DrawCard;
        }
        else
        {
            strategy = new DiscardFromHand();
            (strategy as DiscardFromHand).amount = -val1;
            effectType = EffectType.DiscardCard;
        }
    }
    public ModifyHand(StatusEffectObject statusObj, int duration, AppMechanic appMechanic, int val1 = -1, int val2 = -1, int val3 = -1, int val4 = -1)
    {

    }

    public override void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        strategy.GetCardList(BattleManager.Instance);

    }

    public override bool Compare(CardEffect e)
    {
        if(e is not ModifyHand) { return false; }
        ModifyHand other = (ModifyHand)e;
        if(other.strategy.CompareTo(strategy))
        {
            return true;
        }
        return false;
    }

}


