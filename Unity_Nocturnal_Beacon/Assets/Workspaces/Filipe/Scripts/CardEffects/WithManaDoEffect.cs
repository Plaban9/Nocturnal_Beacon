using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WithManaDoEffect : CardEffect
{
    [SerializeReference, SubclassSelector]
    public CardEffect UseEffectBasedOnThis = new CardEffect();
    public CardVariable affects = CardVariable.VAL1;

    public enum CardVariable
    {
        VAL1,
        VAL2,
        VAL3,
        VAL4
    }

  

    public override string LocalizationKey => "CE_DESC_WithManaDoEffect";
    public override string EffectDescription
    {
        get
        {
            UseEffectBasedOnThis._withManaDoEffectAffecting = affects;
            return UseEffectBasedOnThis.EffectDescription;
        }
    }
    public WithManaDoEffect() { }


    public WithManaDoEffect(EffectTarget target, int val1, int val2)
    {
        this.target = target;
        this.val1 = val1;
        this.val2 = val2;
    }


    public override int GetEffectCost()
    {
        int result;
        float multiplier = 1;

        if (val1 > 0)
        {
            int baseVal = 20;
            int scaling = (int) Mathf.Pow(4, 1 + val1);
            result = val1 > 0 ? (int)baseVal + scaling : 0;
            return result;
        }
        else {
            int baseVal = 5;
            int scaling = 3 * val1;
            result = val1 != 0 ? (int)baseVal + scaling : 0;
            return result;
        }
    }


    override public void OnUse(Card card, BattleUnit owner, List<BattleUnit> targets)
    {
        int availableMana = BattleManager.Instance.GetMana();

        BattleManager.Instance.ModifyMana(-availableMana);
        Debug.Log($"{availableMana}");
        UseEffectBasedOnThis.SetValue(affects, availableMana);

        foreach (BattleUnit target in targets)
        {
            BattleManager.Instance.RunEffect(owner, target, card, UseEffectBasedOnThis);
        }

        
    }


}
