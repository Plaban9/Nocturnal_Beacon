using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifyHand : CardEffect
{
    [SerializeField] int amount;
    [SerializeField] Card speficicCard;
    [SerializeField] List<Card> fromList;
    [SerializeField] IModifyHandStrategies strategy;

    public override string LocalizationKey => "CE_DESC_ModifyHand";

    public override string EffectDescription
    {
        get
        {
            string result = string.Empty;

            result = strategy.ToString();

            return result;
        }
    }

    public override string EffectDetailDescription
    {
        get
        {
            string result = strategy.ToString();
            return result;
        }
    }

    public override int GetEffectCost()
    {
        int val = 0;

        //switch (statusEffect)
        //{
        //    case StatusEffect.Strength:
        //        val = (5 + 3 * (val - 1)) * val;
        //        break;
        //    case StatusEffect.Dexterity:
        //        val = (5 + 3 * (val - 1)) * val;
        //        break;
        //    case StatusEffect.Regenerate:
        //        val = (4 + 2 * (val - 1)) * val;
        //        break;
        //    default:
        //        break;
        //}

        return val;
    }

    public ModifyHand() { }
    public ModifyHand(StatusEffectObject statusObj, int duration, AppMechanic appMechanic, int val1 = -1, int val2 = -1, int val3 = -1, int val4 = -1)
    {

    }

    public void OnUse(EffectTarget target)
    {
       
    }

    public void AfterCast(EffectTarget target, int amount)
    {
        
    }

    public override void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        base.OnUse(card, owner, targets);
        
    }

}


