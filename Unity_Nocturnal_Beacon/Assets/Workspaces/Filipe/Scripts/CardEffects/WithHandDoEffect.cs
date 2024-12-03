using CardAttribute;
using System;
using System.Collections.Generic;
using UnityEngine;
using static WithManaDoEffect;

[Serializable]
public class WithHandDoEffect : CardEffect
{
    [SerializeReference, SubclassSelector]
    public CardEffect UseEffectBasedOnThis = new CardEffect();
    public CardVariable affects = CardVariable.VAL1;


    public override string LocalizationKey => "CE_DESC_WithHandDoEffect";
    public override string EffectDescription
    {
        get
        {
            UseEffectBasedOnThis._withManaDoEffectAffecting = affects;
            return UseEffectBasedOnThis.EffectDescription;
        }
    }
    public WithHandDoEffect() { }


    public WithHandDoEffect(EffectTarget target, int val1, int val2)
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

        List<Card> cards = BattleManager.Instance._cardManager.CardsInHand();
        UseEffectBasedOnThis.SetValue(affects, availableMana);

        foreach (BattleUnit target in targets)
        {
            BattleManager.Instance.RunEffect(owner, target, card, UseEffectBasedOnThis);
        }
    }


}

